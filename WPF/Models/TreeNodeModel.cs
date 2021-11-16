using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aga.Controls.Tree;
using WPF.Infrastructure;
using WPF.Service;

namespace WPF.Models
{
    public class TreeNodeModel : BasePropertyChanged
    {
        private string _pathRoot;
        public TreeList TreeList { get; set; }

        private FolderAnalyzer folderAnalyzer;

        public List<string> Drives
        {
            get => folderAnalyzer.GetDrives();
        }
        public TreeNodeModel()
        {
            TreeList = new TreeList();
            folderAnalyzer = new FolderAnalyzer();
        }


        public void SetRootPath(string pathRoot)
        {
            if(!string.IsNullOrEmpty(pathRoot))
            {
                Set(ref _pathRoot, pathRoot);
                TreeList.UpdateNodes();
            }
        }

        public IEnumerable GetChildren(object parent)
        {
            var nodeParent = parent as Node;
            if (nodeParent == null)
            {
                nodeParent = new Node(_pathRoot, _pathRoot, TypeNode.Folder, 0);
            }
            Debug.WriteLine("Вызов GetChildren");
            return folderAnalyzer.GetChildren(nodeParent);
        }

        public bool HasChildren(object parent)
        {
            var node = parent as Node;
            return node.Type == TypeNode.Folder && node.Children.Count > 0;
        }


    }
}
