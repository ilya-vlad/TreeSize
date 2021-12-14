using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WPF.Models;

namespace WPF.Service
{
    public class FolderAnalyzer
    {
        private SynchronizationContext context;
        public FolderAnalyzer()
        {
            context = SynchronizationContext.Current;
            ThreadPool.SetMaxThreads(8, 1000);
        }

        public void StartScan(Node node)
        {            
            //Debug.WriteLine($"MAIN id = {Thread.CurrentThread.ManagedThreadId}");
            Task.Run(() => Scan(node, context));                      
        }

        public void Scan(Node node, SynchronizationContext uiContext)
        {
            //Debug.WriteLine($"id = {Thread.CurrentThread.ManagedThreadId}");            
            try
            {
                foreach (var folderPath in Directory.GetDirectories(node.FullName))
                {                    
                    var folderInfo = new NodeInfo(new DirectoryInfo(folderPath), node);
                    uiContext.Post(AddNode, folderInfo);
                }

                foreach (var filePath in Directory.GetFiles(node.FullName))
                {
                    var fileInfo = new NodeInfo(new FileInfo(filePath), node);
                    uiContext.Post(AddNode, fileInfo);
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }                 
        }
        
        private void AddFile(NodeInfo newNodeInfo)
        {
            newNodeInfo.ParentNode.CountFiles++;
            newNodeInfo.ParentNode.Size += ((FileInfo)newNodeInfo.Info).Length;
            var newNode = new Node(newNodeInfo.Info.Name, newNodeInfo.Info.FullName, TypeNode.File, ((FileInfo)newNodeInfo.Info).Length, newNodeInfo.ParentNode);            
            newNodeInfo.ParentNode.Children.Add(newNode);
        }

        private void AddFolder(NodeInfo newNodeInfo)
        {            
            var newNode = new Node(newNodeInfo.Info.Name, newNodeInfo.Info.FullName, TypeNode.Folder, 0, newNodeInfo.ParentNode);
            newNodeInfo.ParentNode.Children.Add(newNode);

            Task.Run(() => Scan(newNode, context));            
        }


        private void AddNode(object nodeInfoObject)
        {                        
            var newNodeInfo = nodeInfoObject as NodeInfo;

            if (newNodeInfo.Info is DirectoryInfo)
            {
                AddFolder(newNodeInfo);                
            }
            else
            {
                AddFile(newNodeInfo);                
            }           
        }
    }
}