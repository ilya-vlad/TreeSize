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
            var treeListVM = new TreeListViewModel(tree);
            var mainVM = new MainViewModel(treeListVM);
            DataContext = mainVM;
            tree.Model = treeListVM;            
        }
    }
}
