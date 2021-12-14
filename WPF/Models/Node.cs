﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using WPF.Infrastructure;

namespace WPF.Models
{
    public class Node : BasePropertyChanged
    {
        private double _size;
        public double Size
        {
            get => _size;
            set 
            {
                Set(ref _size, value);
                if (NodeParent != null)
                {
                    CalculatePercentOfParent();                    
                }
                if (Children != null)
                {
                    foreach (var child in Children)
                        child.CalculatePercentOfParent();
                }
            } 
        }

        public string FullName { get; }

        public string Name { get; set; }

        public TypeNode Type { get; }

        public HierarchicalObservableCollection<Node> Children;

        private int _countFolders;
        public int CountFolders
        {
            get => _countFolders;
            set => Set(ref _countFolders, value);
        }

        private int _countFiles;
        public int CountFiles
        {
            get => _countFiles;
            set => Set(ref _countFiles, value);
        }


        static private int _i;
        public int Id { get; }

        public Node(string name, string fullName, TypeNode type, double size, Node nodeParent)
        {
            Name = name;
            FullName = fullName;
            Type = type;
            Size = size;
            Id = ++_i;
            Children = new HierarchicalObservableCollection<Node>();
            NodeParent = nodeParent;

            Children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (NodeParent != null)
            {
                OnPropertyChanged(nameof(NodeParent));
                //Debug.WriteLine("asdasd");
            }
        }



        private double _percentOfParent;
        public double PercentOfParent
        {
            get 
            {
                if (NodeParent != null)
                {
                    //CalculatePercentOfParent();
                    return _percentOfParent;
                }
                return 100;                 
            } 
            set => Set(ref _percentOfParent, value);
        }

        public Node NodeParent { get; }        

        private void CalculatePercentOfParent()
        {
            PercentOfParent = Math.Round(Size / NodeParent.Size * 100, 2);
        }
    }
}