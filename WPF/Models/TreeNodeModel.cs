using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WPF.Infrastructure;
using WPF.Service;

namespace WPF.Models
{
    public class TreeNodeModel : BasePropertyChanged
    {
        private string _rootPath;

        private FolderAnalyzer folderAnalyzer;
        public TreeNodeModel(string rootPath)
        {
            folderAnalyzer = new FolderAnalyzer();
            _rootPath = rootPath;
        }


        public void SetRootPath(string rootPath)
        {
            if (!string.IsNullOrEmpty(rootPath))
            {
                Set(ref _rootPath, rootPath);
            }
        }

        public IEnumerable GetChildrenNode(object parent)
        {
            return null;
            //if (_rootPath == null) return null;

            //var nodeParent = parent as Node;
            //if (parent == null)
            //{
            //    var dir = new DirectoryInfo(_rootPath);
            //    nodeParent = new Node(dir.Name, dir.FullName, TypeNode.Folder, 0, null);
            //    var p = new HierarchicalObservableCollection<Node> { folderAnalyzer.GetFolderInfo(nodeParent) };
            //    return p;
            //}
            //return folderAnalyzer.GetFolderInfo(nodeParent).Children;
        }

        public bool HasChildren(object parent)
        {
            var node = parent as Node;
            return node.Type == TypeNode.Folder && node.Children.Count > 0;
        }
    }
}
