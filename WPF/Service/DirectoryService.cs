using System;
using System.IO;
using System.IO.Abstractions;

namespace WPF.Service
{
    public class DirectoryService
    {
        private readonly IFileSystem _fileSystem;

        public DirectoryService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IFileInfo[] GetFiles(string directoryInfo)
        {
            IFileInfo[] files = Array.Empty<IFileInfo>();
            try
            {
                files = _fileSystem.DirectoryInfo.New(directoryInfo).GetFiles();
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
            return files;
        }

        public IDirectoryInfo[] GetDirectories(string directoryInfo)
        {
            IDirectoryInfo[]? subdirectories = Array.Empty<IDirectoryInfo>();
            try
            {
                subdirectories = _fileSystem.DirectoryInfo.New(directoryInfo).GetDirectories();
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
            return subdirectories;
        }
    }
}