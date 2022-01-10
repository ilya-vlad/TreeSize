using Aga.Controls.Tree;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.IO;
using WPF.Common;
using WPF.Infrastructure;
using WPF.Models;

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

        public TreeListViewModel(ILogger logger, TreeList tree)
        {
            _logger = logger;
            _tree = tree;            
            Root = Directory.GetCurrentDirectory();
            _treeNodeModel = new TreeNodeModel(Root, logger);
        }

        public void UpdateTree()
        {
            _logger.LogInformation("Update TREE");
            _treeNodeModel.SetRootPath(Root);
            _treeNodeModel.StartNewScan();               
            _tree.UpdateNodes();
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

        public bool StatusScan()
        {
            return _treeNodeModel.CancelTokenSource != null && !_treeNodeModel.CancellationToken.IsCancellationRequested;            
        }

        public void CancelScan()
        {
            _treeNodeModel.CancelTokenSource.Cancel();
            _treeNodeModel.CancelTokenSource = null;           
        }
    }
}