using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using WPF.Infrastructure;
using WPF.Service;
using WPF.Common;
using System.Threading.Tasks;
using Aga.Controls.Tree;

namespace WPF.Models
{
    public class TreeNodeModel : BasePropertyChanged
    {
        public CancellationToken CancellationToken { get; private set; }

        public CancellationTokenSource CancelTokenSource { get; set; }

        private string _rootPath;

        private FolderAnalyzer folderAnalyzer;        

        private Node RootNode;

        private readonly ILogger _logger;

        public TreeNodeModel(string rootPath, ILogger logger, DirectoryService directoryService)
        {
            _logger = logger;
            folderAnalyzer = new FolderAnalyzer(logger, directoryService);
            SetRootPath(rootPath);

            //CreateRootNode();
        }

        public async Task StartNewScan(TreeList tree)
        {
            CreateRootNode();

            CancelTokenSource = new CancellationTokenSource();
            CancellationToken = CancelTokenSource.Token;
            
            await folderAnalyzer.StartScan(RootNode, CancellationToken).ConfigureAwait(false);
        }

        public void SetRootPath(string rootPath)
        {
            if (string.IsNullOrEmpty(rootPath))
            {
                throw new ArgumentNullException(rootPath);
            }

            Set(ref _rootPath, rootPath);
        }

        public IEnumerable GetChildren(object parent)
        {
            if (parent == null)
            {
                var nodes = new List<Node>();
                if(RootNode != null) nodes.Add(RootNode);

                return nodes;
            }
            var node = parent as Node;
            //_logger.LogInformation($"Expanded node: {node.FullName}");
            return node.Children;
        }

        public bool HasChildren(Node node)
        {           
            return node.Children != null && node.Children.Count > 0;
        }

        private void CreateRootNode()
        {
            if (string.IsNullOrEmpty(_rootPath))
            {
                throw new ArgumentNullException(_rootPath);
            }

            var dir = new DirectoryInfo(_rootPath);
            RootNode = new Node(dir.Name, dir.FullName, TypeNode.Folder, 0, 0, dir.LastWriteTime, null);
        }
    }
}