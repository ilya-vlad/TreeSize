using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WPF.Infrastructure;
using WPF.ViewModel;

namespace WPF.Models
{
    public class Node : BasePropertyChanged
    {
        private double _size;

        public double Size
        {
            get => _size;
            set => Set(ref _size, value);
        }

        public string FullName { get; set; }

        public string Name { get; set; }

        public TypeNode Type { get; set; }
        static private int _i;
        public int Id { get; }
        public List<Node> Children;

        public Node(string name, string fullName, TypeNode type, double size)
        {
            Name = name;
            FullName = fullName;
            Type = type;
            Size = size;
            Id = ++_i;
            Children = new List<Node>();
        }

    }
}
