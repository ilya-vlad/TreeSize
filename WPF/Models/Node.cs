using System.Collections.ObjectModel;
using System.Linq;
using WPF.Infrastructure;
using WPF.ViewModel;

namespace WPF.Models
{
    public class Node : BasePropertyChanged
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
        public Node Parent { get; }
        private double _size;
        
        public double Size
        {
            get => _size;
            set => Set(ref _size, value);
        }

        private ObservableCollection<Node> _children;

        public ObservableCollection<Node> Children
        {
            get => _children;
            set => Set(ref _children, value);
        }

        public string FullName { get; set; }

        public string Name { get; set; }        
        public int Level { get; set; }
        public bool CanExpand => Children.Any();
        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => _isExpanded = value;
        }
        public Node(MainWindowViewModel mainWindowViewModel, Node parent )
        {
            Children = new ObservableCollection<Node>();
            _mainWindowViewModel = mainWindowViewModel;
            Parent = parent;
        }
    }
}
