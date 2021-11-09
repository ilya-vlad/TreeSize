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
    public class MainWindowViewModel : BasePropertyChanged
    {
        //private Node node;     
        //public Node Node { get => node; set => Set(ref node, value); }


        private ObservableCollection<Node> _items;

        public ObservableCollection<Node> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }
        //public List<string> Drives
        //{
        //    get => folderAnalyzer.GetDrives();
        //}

        public ICommand ScanningFolderCommand { get; }

        public MainWindowViewModel()
        {
            ScanningFolderCommand = new RelayCommand(OnScanningFolderCommandExecuted, CanScanningFolderCommandExecute);
            folderAnalyzer = new FolderAnalyzer(this);            
        }
        private FolderAnalyzer folderAnalyzer;
        private void OnScanningFolderCommandExecuted(object p)
        {
            Items = new ObservableCollection<Node>();



           Task.Run(async () =>
           {
               var node = new Node(this, null)
               {
                   Name = p.ToString(),
                   FullName = p.ToString(),
                   Level = 0
               };
               folderAnalyzer.CalculateFolderSize(node, p.ToString());
               Items.Add(node);
           });
           
        }
        private bool CanScanningFolderCommandExecute(object p) => !string.IsNullOrEmpty((string)p);

    }
}
