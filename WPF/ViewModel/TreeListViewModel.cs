using Aga.Controls.Tree;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using WPF.Models;

namespace WPF.ViewModel
{
    public class TreeListViewModel : ITreeModel, INotifyPropertyChanged    {

        private TreeNodeModel treeNodeModel { get; set; }
        public TestModel testMODEL { get; set; }

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
            //Root = Directory.GetCurrentDirectory();
            //Root = @"C:\Users\ilya\Desktop";
            //Root = @"D:\Android_studio";
            //Root = @"D:\SteamLibrary";
            Root = @"C:\Windows\apppatch";
            //Root = @"C:\Users\ilya\Desktop\One file and folder";
            //treeNodeModel = new TreeNodeModel(Root);
            testMODEL = new TestModel(Root);
        }
        public void UpdateTree()
        {
            testMODEL.SetRootPath(Root);
            //treeNodeModel.SetRootPath(Root);
            Tree.UpdateNodes();
        }
        
        public IEnumerable GetChildren(object parent)
        {
            //var children = treeNodeModel.GetChildrenNode(parent);
            ////TEST = new ObservableCollection<Node>(children.OfType<Node>());
            //return children;
            var children = testMODEL.GetChildren(parent);            
            return children;
        }

        public bool HasChildren(object parent)
        {
            var node = parent as Node;
            return testMODEL.HasChildren(node);
        }

       

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            //Debug.WriteLine(PropertyName + " = " + value);
            return true;
        }
    }
}