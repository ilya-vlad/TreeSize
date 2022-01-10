using System;
using System.Globalization;
using System.Windows.Data;
using WPF.Common;

namespace WPF.Service.Converters
{
    public class ModeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ModeDisplay)value == (ModeDisplay)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
