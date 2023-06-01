using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using WPF.Infrastructure;

namespace WPF.Common
{
    public class Node : BasePropertyChanged
    {
        public string FullName { get; }

        public string Name { get; set; }

        public TypeNode Type { get; }

        public List<Node> Children;

        public Node NodeParent { get; }

        public int Id { get; }

        private static int _i;

        public double Size { get; set; }

        public double Allocated { get; set; }

        public int CountFolders { get; set; }

        public int CountFiles { get; set; }

        public double PercentOfParent
        {
            get
            {
                if (NodeParent == null) return 100;
                return Size != 0 ? Math.Round(Size / NodeParent.Size * 100, 2) : 0;
            }
        }

        public DateTime LastModified { get; set; }

        public Node(string name, string fullName, TypeNode type, double size, double allocated, DateTime lastModified, Node nodeParent)
        {
            Name = name;
            FullName = fullName;
            Type = type;
            Size = size;
            Allocated = allocated;
            Id = ++_i;
            Children = new List<Node>();
            NodeParent = nodeParent;
            LastModified = lastModified;
        }
    }
}