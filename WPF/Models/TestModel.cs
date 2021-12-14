using Aga.Controls.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WPF.Infrastructure;
using WPF.Service;

namespace WPF.Models
{
    public class TestModel : BasePropertyChanged
    {
        private string _rootPath;

        private FolderAnalyzer folderAnalyzer;        
        private Node RootNode;

        public TestModel(string rootPath)
        {
            folderAnalyzer = new FolderAnalyzer();            
            SetRootPath(rootPath);     
        } 


        public void SetRootPath(string rootPath)
        {
            if (!string.IsNullOrEmpty(rootPath))
            {
                Set(ref _rootPath, rootPath);
            }

            var dir = new DirectoryInfo(_rootPath);
            RootNode = new Node(dir.Name, dir.FullName, TypeNode.Folder, 0, null);
            folderAnalyzer.StartScan(RootNode);
        }

        public IEnumerable GetChildren(object parent)
        {
            if(parent == null)
            {                
                return RootNode.Children;
            }
            return (parent as Node).Children;
        }

        public void MyVoid()
        {
            RootNode.Children.Add(new Node("1111", "s", TypeNode.Folder, 33, RootNode));

            RootNode.Children.First().Children.Add(new Node("22222", "s", TypeNode.Folder, 33, RootNode.Children.First()));
            //RootNode.Children.Add(new Node("1", "", TypeNode.Folder, 33, RootNode));
            //RootNode.Children.First().Children.Add(new Node("1", "", TypeNode.Folder, 33, RootNode.Children.First()));
        }


        public bool HasChildren(Node node)
        {
            return node.Type == TypeNode.Folder && node.Children.Count > 0;
        }
    }
}