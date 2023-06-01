using System.IO;
using System.Linq;
using System.Windows.Input;
using WPF.Infrastructure.Command;
using System.Windows.Forms;
using WPF.Infrastructure;
using Microsoft.Extensions.Logging;
using WPF.Common;
using System.Threading.Tasks;

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

        public bool IsScanning
        {
            get => _isScanning; 
            set => Set(ref _isScanning, value);
        }

        public bool _isScanning;

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

            SelectDriveCommand = new RelayCommandAsync(CanSelectDriveCommandExecute, OnSelectDriveCommandExecuted);
            SelectFolderCommand = new RelayCommandAsync(CanSelectFolderCommandExecute, OnSelectFolderCommandExecutedAsync);
            StopScanCommand = new RelayCommand(StopScanCommandExecuted, StopScanCommandExecute);
            RefreshScanCommand = new RelayCommandAsync(CanRefreshScanCommandExecute, RefreshScanCommandExecuted);
            SetModeCommand = new RelayCommand(SetModeCommandExecuted, SetModeCommandExecute);
            SetSizeUnitCommand = new RelayCommand(SetSizeUnitCommandExecuted, SetSizeUnitCommandExecute);
            CloseApplicationCommand = new RelayCommand(CloseApplicationCommandExecuted, CloseApplicationCommandExecute);
        }

        private async Task<bool> CanSelectFolderCommandExecute(object p) => !IsScanning;

        public async Task OnSelectFolderCommandExecutedAsync(object parameter)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                _logger.LogInformation($"Deselected directory");
                return;
            }
            IsScanning = true;
            _treeListViewModel.Root = dialog.SelectedPath;
            _logger.LogInformation($"Selected directory: {dialog.SelectedPath}");

            _ = await Task.Run(() => _treeListViewModel.UpdateTree().ConfigureAwait(false));

            _logger.LogInformation($"Finish scan folder {dialog.SelectedPath}");
            IsScanning = false;
        }

        private async Task OnSelectDriveCommandExecuted(object p)
        {
            IsScanning = true;
            _treeListViewModel.Root = p.ToString();
            _logger.LogInformation($"Selected drive: {p}");

            _ = await Task.Run(() => _treeListViewModel.UpdateTree().ConfigureAwait(false));

            _logger.LogInformation($"Finish scan drive: {p}");
            IsScanning = false;
        }

        private async Task<bool> CanSelectDriveCommandExecute(object p) => !string.IsNullOrEmpty((string)p);
        
        private async Task RefreshScanCommandExecuted(object p)
        {
            _logger.LogInformation("Scanning Refresh");
            IsScanning = true;
            _ = await Task.Run(() => _treeListViewModel.UpdateTree().ConfigureAwait(false));
            IsScanning = false;
        }

        private async Task<bool> CanRefreshScanCommandExecute(object p) => !IsScanning;

        private void StopScanCommandExecuted(object p)
        {
            _treeListViewModel.CancelScan();
        }

        private bool StopScanCommandExecute(object p) => true;

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
