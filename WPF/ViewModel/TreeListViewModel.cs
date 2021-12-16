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
using WPF.Infrastructure;
using WPF.Models;

namespace WPF.ViewModel
{
    public class TreeListViewModel : BasePropertyChanged, ITreeModel    
    {
        public TestModel TestModel { get; set; }

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
            TestModel = new TestModel(Root);
        }

        public void UpdateTree()
        {
            TestModel.SetRootPath(Root);
            //treeNodeModel.SetRootPath(Root);
            _tree.UpdateNodes();
            _logger.LogInformation("Update TREE");
        }
        
        public IEnumerable GetChildren(object parent)
        {
            //var children = treeNodeModel.GetChildrenNode(parent);
            ////TEST = new ObservableCollection<Node>(children.OfType<Node>());
            //return children;
            var children = TestModel.GetChildren(parent);            
            return children;
        }

        public bool HasChildren(object parent)
        {
            var node = parent as Node;
            return TestModel.HasChildren(node);
        }
    }
}