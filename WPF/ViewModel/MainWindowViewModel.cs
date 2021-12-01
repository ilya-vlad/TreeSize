using Aga.Controls.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.Infrastructure;
using WPF.Infrastructure.Command;
using WPF.Models;
using WPF.Service;

namespace WPF.ViewModel
{
    public class MainWindowViewModel : TreeList, ITreeModel, INotifyPropertyChanged
    {
        public static TreeNodeModel treeNodeModel { get; set; }

        private string _pathRoot;
        public string PathRoot
        {
            get => _pathRoot;
            set => Set(ref _pathRoot, value);
        }

        public static readonly DependencyProperty UpdateProperty;

        private bool Update
        {
            get => (bool)GetValue(UpdateProperty);
            set => SetValue(UpdateProperty, value);
        }
        public List<string> Drives
        {
            get => treeNodeModel.Drives;
        }



        public ICommand ScanningFolderCommand { get; }
        private void OnScanningFolderCommandExecuted(object p)
        {
            PathRoot = p.ToString();
            treeNodeModel.SetRootPath(PathRoot);
        }

        private static bool CanScanningFolderCommandExecute(object p) => !string.IsNullOrEmpty((string)p);
        
        public MainWindowViewModel()
        {
            //UpdateProperty = DependencyProperty.Register("Update", typeof(bool), typeof(MainWindowViewModel));
            ScanningFolderCommand = new RelayCommand(OnScanningFolderCommandExecuted, CanScanningFolderCommandExecute);

            treeNodeModel = new TreeNodeModel();


            //treeNodeModel.SetRootPath(@"C:\Users\ilya\Desktop\Корень");
            PathRoot = Directory.GetCurrentDirectory();
            treeNodeModel.SetRootPath(PathRoot);
        }
        
        public IEnumerable GetChildren(object parent)
        {
            return treeNodeModel.GetChildren(parent);
        }

        public bool HasChildren(object parent)
        {
            return treeNodeModel.HasChildren(parent);
        }





        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            Debug.WriteLine(PropertyName + " = " + value);
            return true;
        }
    }
}
