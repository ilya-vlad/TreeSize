using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WPF.Common;

namespace WPF.Service
{
    public class FolderAnalyzer
    {
        private CancellationToken _cancellationToken;

        private SynchronizationContext _context;

        private readonly ILogger _logger;

        public FolderAnalyzer(ILogger logger)
        {            
            _logger = logger;
            _context = SynchronizationContext.Current;
            ThreadPool.SetMaxThreads(8, 1000);
        }

        public void StartScan(Node node, CancellationToken token)
        {
            _cancellationToken = token;
            _logger.LogInformation("Scanning starts...");

            Task.Run(() => Scan(node, _context), _cancellationToken);
        }

        public Task Scan(Node node, SynchronizationContext uiContext)
        {   
            try
            {
                foreach (var folderPath in Directory.GetDirectories(node.FullName))
                {
                    var dirInfo = new DirectoryInfo(folderPath);
                    uiContext.Post(AddFolder, (dirInfo, node));
                    Thread.Sleep(50);
                }

                List<FileInfo> listFiles = new();
                foreach (var filePath in Directory.GetFiles(node.FullName))
                {
                    listFiles.Add(new FileInfo(filePath));
                }
                uiContext.Post(AddFiles, (listFiles, node));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return Task.CompletedTask;
        }

        private void AddFolder(object dirInfoObject)
        {
            var dirInfo = (ValueTuple<DirectoryInfo, Node>)dirInfoObject;
            var newNode = new Node(dirInfo.Item1.Name, dirInfo.Item1.FullName, TypeNode.Folder, 0, 0, dirInfo.Item1.LastWriteTime, dirInfo.Item2);
            
            dirInfo.Item2.Children.Add(newNode);

            CalculationOfAttributesOfParentsOfFolder(newNode);
            
            Task.Run(() => Scan(newNode, _context), _cancellationToken);

            _logger.LogInformation($"Added new folder: {newNode.FullName}");
        }

        private void AddFiles(object filesInfoObject)
        {
            var filesInfo = (ValueTuple<List<FileInfo>, Node>)filesInfoObject;
            double commonSize = 0;
            double commonAllocated = 0;
            foreach (var file in filesInfo.Item1)
            {
                var allocated = SizeDeterminerOnDisk.GetFileSizeOnDisk(file.FullName);
                var newNode = new Node(file.Name, file.FullName, TypeNode.File, file.Length, allocated, file.LastWriteTime, filesInfo.Item2);
                filesInfo.Item2.Children.Add(newNode);
                commonSize += file.Length;
                commonAllocated += allocated;

                _logger.LogInformation($"Added new file: {newNode.FullName}");
            }

            CalculationOfAttributesOfParentsOfFiles(filesInfo.Item2, commonSize, commonAllocated, filesInfo.Item1.Count);
        }

        private Task CalculationOfAttributesOfParentsOfFiles(Node node, double size, double allocated, int countFiles)
        {
            Node parentNode = node;            
            return Task.Run(() =>
            {
                while (!_cancellationToken.IsCancellationRequested && parentNode != null)
                {
                    lock (parentNode)
                    {
                        parentNode.Allocated += allocated;
                        parentNode.Size += size;
                        parentNode.CountFiles += countFiles;

                        if(parentNode.LastModified < node.LastModified)
                        {
                            parentNode.LastModified = node.LastModified;
                        }

                        CalculatePercentOfParentForAllChildren(parentNode);
                    }
                    parentNode = parentNode.NodeParent;
                }
            }, _cancellationToken);
        }

        private Task CalculationOfAttributesOfParentsOfFolder(Node node)
        {
            Node parentNode = node.NodeParent;

            return Task.Run(() =>
            {
                while (!_cancellationToken.IsCancellationRequested && parentNode != null)
                {
                    lock (node)
                    {
                        parentNode.CountFolders++;
                    }
                    parentNode = parentNode.NodeParent;
                }
            }, _cancellationToken);
        }

        private void CalculatePercentOfParentForAllChildren(Node node)
        {
            try
            {
                Parallel.ForEach(
                    node.Children,
                    new ParallelOptions { CancellationToken = _cancellationToken, MaxDegreeOfParallelism = 8 },
                    CalculatePercentOfParentForChild);
            }
            catch(OperationCanceledException ex)
            {
                _logger.LogInformation(ex.Message);
            }           
        }

        private void CalculatePercentOfParentForChild(Node node)
        {
            node.CalculatePercentOfParent();           
        }
    }
}