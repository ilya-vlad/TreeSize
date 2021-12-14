using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF.Service
{
    public class SizeConverter : IValueConverter
    {        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var size = (double)value;
            if (size < KB) return $"{size} Bytes";
            else if (size < MB) return $"{Round(size, KB)} KB";
            else if (size < GB) return $"{Round(size, MB)} MB";
            else if (size < TB) return $"{Round(size, GB)} GB";
            else return $"{Round(size, TB)} TB";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double Round(double size, double rank)
        {
            return Math.Round(size / rank, 2);
        }
        
        private static double KB = 1024;
        private static double MB = KB * 1024;
        private static double GB = MB * 1024;
        private static double TB = GB * 1024;
    }
}
