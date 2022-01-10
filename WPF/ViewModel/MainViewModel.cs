using System.IO;
using System.Linq;
using System.Windows.Input;
using WPF.Infrastructure.Command;
using System.Windows.Forms;
using WPF.Infrastructure;
using Microsoft.Extensions.Logging;
using WPF.Common;

namespace WPF.ViewModel
{
    public class MainViewModel : BasePropertyChanged
    {
        public DriveInfo[] Drives
        {
            get => DriveInfo.GetDrives().Where(d => d.IsReady).ToArray();
        }
        
        public ModeDisplay ModeDisplay
        { 
            get => _modeDisplay; 
            set => Set(ref _modeDisplay, value);
        }

        private ModeDisplay _modeDisplay;

        public SizeUnit SizeUnit
        {
            get => _sizeUnit;
            set => Set(ref _sizeUnit, value);
        }

        private SizeUnit _sizeUnit;

        public int WidthPrefix
        {
            get => _widthPrefix;
            set => Set(ref _widthPrefix, value);
        }

        private int _widthPrefix;

        public bool EnableButtonStopScan
        {
            get => _enableButtonStopScan;
            set => Set(ref _enableButtonStopScan, value);
        }

        private bool _enableButtonStopScan;

        #region COMMANDS

        public ICommand SelectDriveCommand { get; }

        public ICommand SelectFolderCommand { get; }

        public ICommand StopScanCommand { get; }

        public ICommand RefreshScanCommand { get; }

        public ICommand SetModeCommand { get; }

        public ICommand SetSizeUnitCommand { get; }

        public ICommand CloseApplicationCommand { get; }

        #endregion

        private readonly ILogger _logger;

        private TreeListViewModel _treeListViewModel;

        public MainViewModel(ILogger logger, TreeListViewModel treeListViewModel)
        {
            _logger = logger;
            _treeListViewModel = treeListViewModel;

            ModeDisplay = ModeDisplay.Size;
            SizeUnit = SizeUnit.Auto;

            SelectDriveCommand = new RelayCommand(OnSelectDriveCommandExecuted, CanSelectDriveCommandExecute);
            SelectFolderCommand = new RelayCommand(OnSelectFolderCommandExecuted, CanSelectFolderCommandExecute);
            StopScanCommand = new RelayCommand(StopScanCommandExecuted, StopScanCommandExecute);
            RefreshScanCommand = new RelayCommand(RefreshScanCommandExecuted, RefreshScanCommandExecute);
            SetModeCommand = new RelayCommand(SetModeCommandExecuted, SetModeCommandExecute);
            SetSizeUnitCommand = new RelayCommand(SetSizeUnitCommandExecuted, SetSizeUnitCommandExecute);
            CloseApplicationCommand = new RelayCommand(CloseApplicationCommandExecuted, CloseApplicationCommandExecute);
        }

        private void OnSelectDriveCommandExecuted(object p)
        {
            EnableButtonStopScan = true;
            _treeListViewModel.Root = p.ToString();
            _treeListViewModel.UpdateTree();

            _logger.LogInformation($"Scan drive: {p}");
        }

        private bool CanSelectDriveCommandExecute(object p) => !string.IsNullOrEmpty((string)p);

        private void OnSelectFolderCommandExecuted(object p)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                _logger.LogInformation($"Deselected directory");
                return;
            }

            EnableButtonStopScan = true;
            _treeListViewModel.Root = dialog.SelectedPath;
            _logger.LogInformation($"Selected directory: {dialog.SelectedPath}");

            _treeListViewModel.UpdateTree();
        }
        private bool CanSelectFolderCommandExecute(object p) => true;

        private void StopScanCommandExecuted(object p)
        {
            _logger.LogInformation("Scanning Stopped");
            _treeListViewModel.CancelScan();
            EnableButtonStopScan = false;
        }

        private bool StopScanCommandExecute(object p) => true;

        private void RefreshScanCommandExecuted(object p)
        {
            _logger.LogInformation("Scanning Refresh");
            _treeListViewModel.UpdateTree();
        }

        private bool RefreshScanCommandExecute(object p) => true;

        private void SetModeCommandExecuted(object p)
        {
            var mode = (ModeDisplay)p;
            ModeDisplay = mode;

            _logger.LogInformation($"Mode {mode} is set");
        }

        private bool SetModeCommandExecute(object p) => true;

        private void SetSizeUnitCommandExecuted(object p)
        {
            var sizeUnit = (SizeUnit)p;
            SizeUnit = sizeUnit;
        }

        private bool SetSizeUnitCommandExecute(object p) => true;

        private void CloseApplicationCommandExecuted(object p)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private bool CloseApplicationCommandExecute(object p) => true;
    }
}
