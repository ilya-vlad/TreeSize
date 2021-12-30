using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace WPF.Models
{
    public class HierarchicalObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        public HierarchicalObservableCollection()
        {           
            CollectionChanged += HierarchicalObservableCollection_CollectionChanged;
        }

        private void HierarchicalObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {           
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (INotifyPropertyChanged node in e.NewItems)
                {
                    node.PropertyChanged += Children_PropertyChanged;
                }
            }            
        }

        private void Children_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
            OnCollectionChanged(args);
        }
    }
}