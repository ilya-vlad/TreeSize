using System.Threading;
using System.Windows;
using WPF.ViewModel;
using Microsoft.Extensions.Logging;
using System.Linq;
using WPF.Models;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ILogger _logger;
        
        public MainWindow(ILogger<MainWindow> logger)
        {
            _logger = logger;

            InitializeComponent();            

            var treeListViewModel = new TreeListViewModel(_logger, _tree);                  
            var mainViewModel = new MainViewModel(_logger, treeListViewModel);

            DataContext = mainViewModel;
            _tree.Model = treeListViewModel;            
        }
    }
}