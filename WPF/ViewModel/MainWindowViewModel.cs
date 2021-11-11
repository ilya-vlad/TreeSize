using Aga.Controls.Tree;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Infrastructure;
using WPF.Infrastructure.Command;
using WPF.Models;
using WPF.Service;

namespace WPF.ViewModel
{
    public class MainWindowViewModel : BasePropertyChanged, ITreeModel
    {
        private string _path;
        public string Path
        {
            get => _path;
            set => Set(ref _path, value);
        }
       
        public List<string> Drives
        {
            get => folderAnalyzer.GetDrives();
        }

        public ICommand ScanningFolderCommand { get; }
        private void OnScanningFolderCommandExecuted(object p)
        {
            Path = p.ToString();
        }

        private bool CanScanningFolderCommandExecute(object p) => !string.IsNullOrEmpty((string)p);
        
        public MainWindowViewModel()
        {
            ScanningFolderCommand = new RelayCommand(OnScanningFolderCommandExecuted, CanScanningFolderCommandExecute);
            folderAnalyzer = new FolderAnalyzer();
            Path = "c:/users/ilya/desktop/";
            //Path = "d:/";
        }
        private FolderAnalyzer folderAnalyzer;
        

        public IEnumerable GetChildren(object parent)
        {
            var nodeParent = parent as Node;
            if (nodeParent == null)
            {
                nodeParent = new Node(Path, Path, TypeNode.Folder, 0);
            }
            
            Debug.WriteLine("Вызов GetChildren");
            return folderAnalyzer.GetChildren(nodeParent);
        }

        public bool HasChildren(object parent)
        {
            var node = parent as Node;
            return Directory.Exists(node.FullName);
        }
    }

   
}
