using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WPF.Models;
using WPF.ViewModel;

namespace WPF.Service
{
    public class FolderAnalyzer
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public FolderAnalyzer(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
        public List<string> GetDrives()
        {
            return DriveInfo.GetDrives().Select(d => d.Name).ToList();
        }

        public double CalculateFolderSize(Node node, string path)
        {
            //double size = 0;
            //if (!Directory.Exists(path))
            //    return size;

            try
            {
                foreach (var filePath in GetFiles(path))
                {
                    var file = new FileInfo(filePath);
                    var newNode = new Node(_mainWindowViewModel , node)
                    {
                        Name = file.Name,
                        FullName = file.FullName,
                        Size = file.Length,
                        Level = node.Level - 1
                    };
                    node.Children.Add(newNode);                    
                    node.Size += file.Length;
                    
                }
                foreach (var folderPath in GetDirectories(path))
                {
                    var folder = new DirectoryInfo(folderPath);
                    var newNode = new Node(_mainWindowViewModel, node)
                    {
                        Name = folder.Name,
                        FullName = folder.FullName,
                        Level = node.Level + 1
                    };

                    newNode.Size += CalculateFolderSize(newNode, folderPath);                    
                    node.Size += newNode.Size;
                    node.Children.Add(newNode);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                //Debug.WriteLine(ex.Message);
            }            
            return node.Size;
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
