using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Infrastructure
{
    public class BasePropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        static DateTime? lastUpdate;
        static TimeSpan? delay;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));            
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (delay == null)
                delay = TimeSpan.FromSeconds(3);

            if (Equals(field, value)) return false;
            field = value;
            //if (lastUpdate == null || DateTime.Now - lastUpdate > delay)
            //{
            //    lastUpdate = DateTime.Now;
            //    Debug.WriteLine($"OnPropertyChanged {PropertyName}");
            //}
            OnPropertyChanged(PropertyName);

            return true;
        }
    }
}
