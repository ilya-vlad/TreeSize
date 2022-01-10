using System;
using System.Globalization;
using System.Windows.Data;
using WPF.Common;

namespace WPF.Service.Converters
{
    public class SizeUnitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (SizeUnit)value == (SizeUnit)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
