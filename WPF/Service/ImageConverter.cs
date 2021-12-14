using System;
using System.Globalization;
using System.Windows.Data;
using WPF.Models;

namespace WPF.Service
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var node = value as Node;
            if (node.Type == TypeNode.File)
                return "Image/file.png";
            else
                return "Image/folder.png";            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
