using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPF.Infrastructure.Command;
using System.Windows.Forms;
using System.Threading.Tasks;
using WPF.Infrastructure;

namespace WPF.ViewModel
{
    public class MainViewModel : BasePropertyChanged
    {
        public TreeListViewModel TreeListViewModel;
        
        public string[] Drives
        {
            get => DriveInfo.GetDrives()
                .Where(d => d.IsReady)
                .Select(x => x.Name)
                .ToArray();
            //get => new string[] {
            //    @"C:\Users\ilya\Desktop\Корень",
            //    @"C:\Users\ilya\Desktop\One file and folder",
            //    @"C:\Users\ilya\Desktop\"
            //};
        }

        #region Commands

        public ICommand SelectDriveCommand { get; }
        private void OnSelectDriveCommandExecuted(object p)
        {
            TreeListViewModel.Root = p.ToString();
            TreeListViewModel.UpdateTree();
        }

        private bool CanSelectDriveCommandExecute(object p) => !string.IsNullOrEmpty((string)p);

        public ICommand SelectFolderCommand { get; }
        private void OnSelectFolderCommandExecuted(object p)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            TreeListViewModel.Root = dialog.SelectedPath;
            TreeListViewModel.UpdateTree();

            //TreeListViewModel.testModel.MyVoid();
        }
        private bool CanSelectFolderCommandExecute(object p) => true;

        #endregion
        public MainViewModel(TreeListViewModel treeListViewModel)
        {
            TreeListViewModel = treeListViewModel;
            SelectDriveCommand = new RelayCommand(OnSelectDriveCommandExecuted, CanSelectDriveCommandExecute);
            SelectFolderCommand = new RelayCommand(OnSelectFolderCommandExecuted, CanSelectFolderCommandExecute);
        }
    }
}
