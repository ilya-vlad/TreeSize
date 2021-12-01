using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using WPF.Models;
using WPF.ViewModel;

namespace WPF.Service
{
    public class FolderAnalyzer
    {
        private Dictionary<int, Node> dictionaryNode;
        public FolderAnalyzer()
        {
            dictionaryNode = new Dictionary<int, Node>();
        }
        public List<string> GetDrives()
        {
            //return DriveInfo.GetDrives().Select(d => d.Name).ToList();


            var list = new List<string>();
            var p = Directory.GetCurrentDirectory();
            p = Directory.GetParent(p).FullName;
            p = Directory.GetParent(p).FullName;
            p = Directory.GetParent(p).FullName;
            foreach (var e in Directory.GetDirectories(p))
                list.Add(e);
            return list;
        }

        public IEnumerable<Node> GetChildren(Node node)
        {
            CalculateFolderSize(node);
            return dictionaryNode[node.Id].Children;
        }
        public void CalculateFolderSize(Node node)
        {

            if (dictionaryNode.ContainsKey(node.Id))
                return;

            dictionaryNode.Add(node.Id, node);

            try
            {
                foreach (var folderPath in GetDirectories(node.FullName))
                {
                    var folder = new DirectoryInfo(folderPath);
                    var newNode = new Node(folder.Name, folder.FullName, TypeNode.Folder, 0);
                    CalculateFolderSize(newNode);
                    node.Children.Add(newNode);
                    node.Size += newNode.Size;

                    if (!dictionaryNode.ContainsKey(newNode.Id))
                        dictionaryNode.Add(newNode.Id, newNode);
                }

                foreach (var filePath in GetFiles(node.FullName))
                {
                    var file = new FileInfo(filePath);
                    var newNode = new Node(file.Name, file.FullName, TypeNode.File, file.Length);
                    node.Children.Add(newNode);
                    node.Size += newNode.Size;
                }
            }
            catch(UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
            }  
        }
         

        private string[] GetFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            return files;
        }

        private string[] GetDirectories(string path)
        {
            string[] folders = Directory.GetDirectories(path);
            return folders;
        }
    }
}
