//using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WPF.Common;

namespace WPF.Service
{
    public class FolderAnalyzer
    {
        private CancellationToken _cancellationToken;

        //private SynchronizationContext _context;

        private readonly ILogger _logger;

        private readonly DirectoryService _directoryService;

        public FolderAnalyzer(ILogger logger, DirectoryService directoryService)
        {
            _logger = logger;
            _directoryService = directoryService;
        }

        public async Task StartScan(Node node, CancellationToken token)
        {
            _cancellationToken = token;
            _logger.LogInformation("Scanning starts...");

            await ScanNode(node, token).ConfigureAwait(false);
        }

        public async Task ScanNode(Node node, CancellationToken token)
        {   
            try
            {
                var folders = _directoryService.GetDirectories(node.FullName);
                foreach (var dirInfo in folders)
                {
                    var newFolderNode = new Node(dirInfo.Name, dirInfo.FullName, TypeNode.Folder, 0, 0,
                        dirInfo.LastWriteTime, node);
                    node.Children.Add(newFolderNode);
                    await ScanNode(newFolderNode, token);

                    node.Size += newFolderNode.Size;
                    node.CountFiles += newFolderNode.CountFiles;
                    node.CountFolders += newFolderNode.CountFolders + 1;
                    node.Allocated += newFolderNode.Allocated;

                    if (!token.IsCancellationRequested) continue;
                    _logger.LogInformation("Scan stopped.");
                    return;
                }

                var files = _directoryService.GetFiles(node.FullName);
                foreach (var fileInfo in files)
                {
                    var newFileNode = new Node(fileInfo.Name, fileInfo.FullName, TypeNode.File, fileInfo.Length, SizeDeterminerOnDisk.GetFileSizeOnDisk(fileInfo.FullName),
                        fileInfo.LastWriteTime, node);
                    node.Children.Add(newFileNode);
                    node.CountFiles++;
                    node.Size += newFileNode.Size;
                    node.Allocated += newFileNode.Allocated;

                    if (!token.IsCancellationRequested) continue;
                    _logger.LogInformation("Scan stopped.");
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}