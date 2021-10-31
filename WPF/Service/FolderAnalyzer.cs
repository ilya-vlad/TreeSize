using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WPF.Models;

namespace WPF.Service
{
    public class FolderAnalyzer
    {
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
                    var newNode = new Node()
                    {
                        Name = file.Name,
                        FullName = file.FullName,
                        Size = file.Length
                    };
                    node.Children.Add(newNode);                    
                    node.Size += file.Length;
                    
                }
                foreach (var folderPath in GetDirectories(path))
                {
                    var folder = new DirectoryInfo(folderPath);
                    var newNode = new Node()
                    {
                        Name = folder.Name,
                        FullName = folder.FullName                        
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
