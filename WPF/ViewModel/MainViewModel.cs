using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPF.Infrastructure.Command;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WPF.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public TreeListViewModel TreeListVM;
        
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

        public ICommand SelectDriveCommand { get; }
        private void OnSelectDriveCommandExecuted(object p)
        {
            TreeListVM.Root = p.ToString();
            TreeListVM.UpdateTree();
        }

        private bool CanSelectDriveCommandExecute(object p) => !string.IsNullOrEmpty((string)p);

        public ICommand SelectFolderCommand { get; }
        private void OnSelectFolderCommandExecuted(object p)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK)
                return;




            TreeListVM.Root = dialog.SelectedPath;
            TreeListVM.UpdateTree();

            //TreeListVM.testMODEL.MyVoid();
        }
        private bool CanSelectFolderCommandExecute(object p) => true;

        public MainViewModel(TreeListViewModel treeListVM)
        {
            TreeListVM = treeListVM;
            SelectDriveCommand = new RelayCommand(OnSelectDriveCommandExecuted, CanSelectDriveCommandExecute);
            SelectFolderCommand = new RelayCommand(OnSelectFolderCommandExecuted, CanSelectFolderCommandExecute);
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
            return true;
        }
    }
}
