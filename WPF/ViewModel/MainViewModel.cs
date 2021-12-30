using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPF.Infrastructure.Command;
using System.Windows.Forms;
using System.Threading.Tasks;
using WPF.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace WPF.ViewModel
{
    public class MainViewModel : BasePropertyChanged
    {        
        public string[] Drives
        {
            get => DriveInfo.GetDrives()
                .Where(d => d.IsReady)
                .Select(x => x.Name)
                .ToArray();            
        }

        public ICommand SelectDriveCommand { get; }

        public ICommand SelectFolderCommand { get; }

        public ICommand StopScanCommand { get; }



        public bool EnableButtonStopScan
        {
            get => _enableButtonStopScan;
            set => Set(ref _enableButtonStopScan, value);
        }

        private bool _enableButtonStopScan;


        private readonly ILogger _logger;

        private TreeListViewModel _treeListViewModel;


        public MainViewModel(ILogger logger, TreeListViewModel treeListViewModel)
        {
            _logger = logger;
            _treeListViewModel = treeListViewModel;
            SelectDriveCommand = new RelayCommand(OnSelectDriveCommandExecuted, CanSelectDriveCommandExecute);
            SelectFolderCommand = new RelayCommand(OnSelectFolderCommandExecuted, CanSelectFolderCommandExecute);
            StopScanCommand = new RelayCommand(StopScanCommandExecuted, StopScanCommandExecute);          
        }

        private void OnSelectDriveCommandExecuted(object p)
        {
            _logger.LogInformation("SCAN DRIVE");
            EnableButtonStopScan = true;
            _treeListViewModel.Root = p.ToString();
            _treeListViewModel.UpdateTree();
        }

        private bool CanSelectDriveCommandExecute(object p) => !string.IsNullOrEmpty((string)p);

        private void OnSelectFolderCommandExecuted(object p)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            EnableButtonStopScan = true;
            _treeListViewModel.Root = dialog.SelectedPath;
            _treeListViewModel.UpdateTree();
        }

        private bool CanSelectFolderCommandExecute(object p) => true;

        private void StopScanCommandExecuted(object p)
        {
            _logger.LogInformation("STOP Scanning!");
            _treeListViewModel.CancelScan();
            EnableButtonStopScan = false;
        }

        private bool StopScanCommandExecute(object p) => true;

    }
}
