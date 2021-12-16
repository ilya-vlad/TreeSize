using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace WPF.Models
{
    public class HierarchicalObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        static DateTime? lastUpdate;
        static TimeSpan? delay;
        public HierarchicalObservableCollection()
        {
            //if(delay == null)   
            //    delay = TimeSpan.FromSeconds(3);    
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
            //if (lastUpdate == null || DateTime.Now - lastUpdate > delay)
            //{
            //    lastUpdate = DateTime.Now;
            //    Debug.WriteLine($"Children_PropertyChanged\t{lastUpdate.Value.TimeOfDay}");   
            //}
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
            OnCollectionChanged(args);            
        }
    }
}