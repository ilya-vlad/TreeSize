using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF.Infrastructure
{
    public class BasePropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));            
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            App.Current.Dispatcher.Invoke(() => OnPropertyChanged(PropertyName));
            return true;
        }
    }
}