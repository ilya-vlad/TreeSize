using Aga.Controls.Tree;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WPF.Infrastructure;
using WPF.Models;

namespace WPF.ViewModel
{
    public class TreeListViewModel : BasePropertyChanged, ITreeModel    
    {
        public TreeNodeModel TreeNodeModel { get; set; }

        public string Root
        {
            get => _root;
            set => Set(ref _root, value);
        }

        private string _root;

        private TreeNodeModel _treeNodeModel { get; set; }

        private TreeList _tree;

        private readonly ILogger _logger;

        public TreeListViewModel(ILogger logger, TreeList tree)
        {
            _logger = logger;
            _tree = tree;            
            Root = Directory.GetCurrentDirectory();            
            TreeNodeModel = new TreeNodeModel(Root, logger);
        }

        public void UpdateTree()
        {
            TreeNodeModel.SetRootPath(Root);
            TreeNodeModel.StartNewScan();               
            _tree.UpdateNodes();
            _logger.LogInformation("Update TREE");
        }
        
        public IEnumerable GetChildren(object parent)
        {            
            var children = TreeNodeModel.GetChildren(parent);            
            return children;
        }

        public bool HasChildren(object parent)
        {
            var node = parent as Node;
            return TreeNodeModel.HasChildren(node);
        }

        public bool StatusScan()
        {
            return TreeNodeModel.CancelTokenSource != null && !TreeNodeModel.CancellationToken.IsCancellationRequested;            
        }

        public void CancelScan()
        {
            TreeNodeModel.CancelTokenSource.Cancel();
            TreeNodeModel.CancelTokenSource = null;           
        }
    }
}