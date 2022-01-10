using System;
using System.Globalization;
using System.Windows.Data;
using WPF.Common;

namespace WPF.Service.Converters
{
    public class SizeConverter : IMultiValueConverter
    {
        private static double KB = (double)SizeUnit.KB;
        private static double MB = (double)SizeUnit.MB;
        private static double GB = (double)SizeUnit.GB;
        
        public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
        {
            var sizeUnit = (SizeUnit)value[0];
            var size = (double)value[1];

            if (sizeUnit == SizeUnit.Auto)
            {
                if (size < KB) return $"{size} Bytes";
                else if (size < MB) return $"{Round(size, KB)} KB";
                else if (size < GB) return $"{Round(size, MB)} MB";
                else return $"{Round(size, GB)} GB";
            }
            else return $"{Round(size, (double)sizeUnit)} {sizeUnit}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double Round(double size, double rank)
        {
            return Math.Round(size / rank, 2);
        }
    }
}