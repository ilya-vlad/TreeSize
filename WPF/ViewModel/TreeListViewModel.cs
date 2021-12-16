using Aga.Controls.Tree;
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
        private TreeNodeModel treeNodeModel { get; set; }
        public TestModel testModel { get; set; }

        private TreeList Tree;

        private string _root;

        public string Root
        {
            get => _root;
            set => Set(ref _root, value);
        }

        public TreeListViewModel(TreeList tree)
        {
            Tree = tree;
            Root = Directory.GetCurrentDirectory();
            testModel = new TestModel(Root);
        }

        public void UpdateTree()
        {
            testModel.SetRootPath(Root);
            //treeNodeModel.SetRootPath(Root);
            Tree.UpdateNodes();
        }
        
        public IEnumerable GetChildren(object parent)
        {
            //var children = treeNodeModel.GetChildrenNode(parent);
            ////TEST = new ObservableCollection<Node>(children.OfType<Node>());
            //return children;
            var children = testModel.GetChildren(parent);            
            return children;
        }

        public bool HasChildren(object parent)
        {
            var node = parent as Node;
            return testModel.HasChildren(node);
        }
    }
}