using System;
using System.Globalization;
using System.Windows.Data;
using WPF.Common;

namespace WPF.Service.Converters
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            if ((TypeNode)value == TypeNode.File)
                return "image/file.png";
            else
                return "image/folder.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
