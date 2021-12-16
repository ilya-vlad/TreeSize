using System.Threading;
using System.Windows;
using WPF.ViewModel;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var treeListViewModel = new TreeListViewModel(tree);            
            var mainViewModel = new MainViewModel(treeListViewModel);
            DataContext = mainViewModel;
            tree.Model = treeListViewModel;            
        }
    }
}
