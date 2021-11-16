using Aga.Controls.Tree;
using System;
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
        private TreeNodeModel treeNodeModel { get; set; }

        public List<string> Drives
        {
            get => treeNodeModel.Drives;
        }

        public ICommand ScanningFolderCommand { get; }
        private void OnScanningFolderCommandExecuted(object p)
        {
            treeNodeModel.SetRootPath(p.ToString());
        }

        private bool CanScanningFolderCommandExecute(object p) => !string.IsNullOrEmpty((string)p);
        
        public MainWindowViewModel()
        {
            ScanningFolderCommand = new RelayCommand(OnScanningFolderCommandExecuted, CanScanningFolderCommandExecute);

            treeNodeModel = new TreeNodeModel();

            
            treeNodeModel.SetRootPath(@"C:\Windows\System32\drivers");
        }
        
        public IEnumerable GetChildren(object parent)
        {
            return treeNodeModel.GetChildren(parent);
        }

        public bool HasChildren(object parent)
        {
            return treeNodeModel.HasChildren(parent);
        }
    }
}
