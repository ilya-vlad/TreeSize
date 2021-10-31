using System.Collections.ObjectModel;
using WPF.Infrastructure;

namespace WPF.Models
{
    public class Node : BasePropertyChanged
    {
        private double size;
        
        public double Size
        {
            get => size;
            set => Set(ref size, value);
        }

        private ObservableCollection<Node> children;

        public ObservableCollection<Node> Children
        {
            get => children;
            set => Set(ref children, value);
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public Node()
        {
            Children = new ObservableCollection<Node>();
            
        }
    }
}
