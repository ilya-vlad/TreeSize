using System;
using System.Windows;
using Aga.Controls.Tree;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using WPF.Common;
using WPF.Infrastructure;
using WPF.Models;
using WPF.Service;

namespace WPF.ViewModel
{
    public class TreeListViewModel : BasePropertyChanged, ITreeModel    
    {
        public string Root
        {
            get => _root;
            set => Set(ref _root, value);
        }

        private string _root;

        private TreeNodeModel _treeNodeModel { get; set; }

        private TreeList _tree;

        private readonly ILogger _logger;

        public TreeListViewModel(ILogger logger, TreeList tree, DirectoryService directoryService)
        {
            _logger = logger;
            _tree = tree;            
            Root = Directory.GetCurrentDirectory();
            _treeNodeModel = new TreeNodeModel(Root, logger, directoryService);
        }

        public async Task UpdateTree()
        {
            _treeNodeModel.SetRootPath(Root);
            await _treeNodeModel.StartNewScan(_tree).ConfigureAwait(false);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _tree.UpdateNodes();
                _tree.Nodes.First().IsExpanded = true;
            });
        }

        public IEnumerable GetChildren(object parent)
        {            
            return _treeNodeModel.GetChildren(parent);
        }

        public bool HasChildren(object parent)
        {
            var node = parent as Node;
            return _treeNodeModel.HasChildren(node);
        }
        
        public void CancelScan()
        {
            _treeNodeModel.CancelTokenSource.Cancel();
            _treeNodeModel.CancelTokenSource = null;           
        }
    }
}