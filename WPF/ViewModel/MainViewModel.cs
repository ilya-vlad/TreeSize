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

        private readonly ILogger _logger;

        private TreeListViewModel _treeListViewModel;

        public MainViewModel(ILogger logger, TreeListViewModel treeListViewModel)
        {
            _logger = logger;
            _treeListViewModel = treeListViewModel;
            SelectDriveCommand = new RelayCommand(OnSelectDriveCommandExecuted, CanSelectDriveCommandExecute);
            SelectFolderCommand = new RelayCommand(OnSelectFolderCommandExecuted, CanSelectFolderCommandExecute);
        }

        private void OnSelectDriveCommandExecuted(object p)
        {
            _treeListViewModel.Root = p.ToString();
            _treeListViewModel.UpdateTree();
        }

        private bool CanSelectDriveCommandExecute(object p) => !string.IsNullOrEmpty((string)p);

        private void OnSelectFolderCommandExecuted(object p)
        {
            _logger.LogInformation("Call SelectFolderCommand");

            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            _treeListViewModel.Root = dialog.SelectedPath;
            _treeListViewModel.UpdateTree();
            //TreeListViewModel.testModel.MyVoid();
        }

        private bool CanSelectFolderCommandExecute(object p) => true;
    }
}
