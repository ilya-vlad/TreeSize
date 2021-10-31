using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Infrastructure;
using WPF.Infrastructure.Command;
using WPF.Models;
using WPF.Service;

namespace WPF.ViewModel
{
    internal class MainWindowViewModel : BasePropertyChanged
    {
        private Node node;
        public Title Title { get; set; }

        //private ObservableCollection<Node> children;

        //public ObservableCollection<Node> Children
        //{
        //    get => children;
        //    set => Set(ref children, value);
        //}
        public Node Node { get => node; set => Set(ref node, value); }
        public List<string> Drives
        {
            get => folderAnalyzer.GetDrives();
        }

        public ICommand ScanningFolderCommand { get; }

        public MainWindowViewModel()
        {
            ScanningFolderCommand = new RelayCommand(OnScanningFolderCommandExecuted, CanScanningFolderCommandExecute);
            folderAnalyzer = new FolderAnalyzer();
            Title = new Title() { Name = "TreeSize Application" };
        }
        private FolderAnalyzer folderAnalyzer;
        private void OnScanningFolderCommandExecuted(object p)
        {




           Task.Run(async () =>
           {
               Node = new Node()
               {
                   Name = p.ToString(),
                   FullName = p.ToString()
               };
               folderAnalyzer.CalculateFolderSize(Node, p.ToString());
           });
           
        }
        private bool CanScanningFolderCommandExecute(object p) => !string.IsNullOrEmpty((string)p);
    }
}
