using System.Windows;
using WPF.ViewModel;
using Microsoft.Extensions.Logging;
using System.Windows.Controls.Ribbon;
using System.Collections.Generic;
using System.Collections.Specialized;
using Aga.Controls.Tree;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using WPF.Service;
using System.Windows.Documents;
using WPF.Common;
using System.Threading.Tasks;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private readonly ILogger _logger;
        private GridViewColumnHeader _lastHeaderClicked;
        private ListSortDirection _lastDirection;
        private SortAdorner _listViewSortAdorner;

        public MainWindow(ILogger<MainWindow> logger, DirectoryService directoryService)
        {
            _logger = logger;

            InitializeComponent();            

            var treeListViewModel = new TreeListViewModel(_logger, _tree, directoryService);                  
            var mainViewModel = new MainViewModel(_logger, treeListViewModel);

            
            DataContext = mainViewModel;
            _tree.Model = treeListViewModel;

        }
        
        private void ExpandItems_Click(object sender, RoutedEventArgs e)
        {
            var level = (LevelExpension)((RibbonMenuItem)sender).Tag;
            var nodes = _tree.Nodes;

            if (level != LevelExpension.FullExpansion)
            {
                ExpandNodesToLevel(nodes, (int)level);                
            }
            else
            {
                ExpandAllNodes(nodes);
            }
        }

        private void ExpandNodesToLevel(IEnumerable<TreeNode> children, int level)
        {
            if (level != 0)
            {
                foreach(var child in children)
                {
                    child.IsExpanded = true;
                    ExpandNodesToLevel(child.Nodes, level - 1);
                }
            }
        }

        private void ExpandAllNodes(IEnumerable<TreeNode> children)
        {            
            foreach (var child in children)
            {
                child.IsExpanded = true;
                ExpandAllNodes(child.Nodes);
            }
        }

        private void CollapseAllNode_Click(object sender, RoutedEventArgs e)
        {
            CollapseAllNodes(_tree.Nodes);
        }

        private void CollapseAllNodes(IEnumerable<TreeNode> children)
        {
            foreach (var child in children)
            {
                child.IsExpanded = false;
                CollapseAllNodes(child.Nodes);
            }
        }

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var header = (ColumnSort)headerClicked.Tag;

                    Sort(header, direction);

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }

            if(_listViewSortAdorner != null)
            {
                AdornerLayer.GetAdornerLayer(_lastHeaderClicked).Remove(_listViewSortAdorner);
            }

            _listViewSortAdorner = new SortAdorner(_lastHeaderClicked, _lastDirection);            
            AdornerLayer.GetAdornerLayer(_lastHeaderClicked).Add(_listViewSortAdorner);
        }

        private void Sort(ColumnSort sortBy, ListSortDirection direction)
        {
            var dataView = (ListCollectionView)CollectionViewSource.GetDefaultView(_tree.ItemsSource);
            dataView.CustomSort = new TreeNodeSorter(sortBy, direction);
        }
    }
}