using System;
using System.Collections.Specialized;
using WPF.Infrastructure;

namespace WPF.Common
{
    public class Node : BasePropertyChanged
    {
        public string FullName { get; }

        public string Name { get; set; }

        public TypeNode Type { get; }

        public HierarchicalObservableCollection<Node> Children;

        public Node NodeParent { get; }

        public int Id { get; }

        static private int _i;

        public double Size
        {
            get => _size;
            set
            {
                Set(ref _size, value);
            }
        }

        private double _size;

        public double Allocated
        {
            get => _allocated;
            set
            {
                Set(ref _allocated, value);
            }
        }

        private double _allocated;

        public int CountFolders
        {
            get => _countFolders;
            set => Set(ref _countFolders, value);
        }

        private int _countFolders;

        public int CountFiles
        {
            get => _countFiles;
            set => Set(ref _countFiles, value);
        }

        private int _countFiles;

        public double PercentOfParent
        {
            get
            {
                if (NodeParent != null)
                {
                    return _percentOfParent;
                }
                return 100;
            }
            set => Set(ref _percentOfParent, value);
        }

        private double _percentOfParent;

        public DateTime LastModified
        {
            get => _lastModified;
            set => Set(ref _lastModified, value);
        }

        private DateTime _lastModified;

        public Node(string name, string fullName, TypeNode type, double size, double allocated, DateTime lastModified, Node nodeParent)
        {
            Name = name;
            FullName = fullName;
            Type = type;
            Size = size;
            Allocated = allocated;
            Id = ++_i;
            Children = new HierarchicalObservableCollection<Node>();
            NodeParent = nodeParent;
            LastModified = lastModified;

            Children.CollectionChanged += Children_CollectionChanged;
        }

        public void CalculatePercentOfParent()
        {
            PercentOfParent = Math.Round(Size / NodeParent.Size * 100, 2);
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (NodeParent != null)
            {
                OnPropertyChanged(nameof(NodeParent));
            }
        }        
    }
}