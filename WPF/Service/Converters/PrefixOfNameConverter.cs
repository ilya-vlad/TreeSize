using System;
using System.Globalization;
using System.Windows.Data;
using WPF.Common;
using WPF.ViewModel;

namespace WPF.Service.Converters
{

    public class PrefixOfNameConverter : IMultiValueConverter
    {
        private static SizeConverter _sizeConverter = new();
        private static SizeUnit _lastSizeUnit;
        private static ModeDisplay _lastMode;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var node = (Node)values[0];
            var mode = (ModeDisplay)values[1];
            var sizeUnit = (SizeUnit)values[2];
            var viewModel = (MainViewModel)values[3];

            if (_lastSizeUnit != sizeUnit)
            {
                viewModel.WidthPrefix = 0;
                _lastSizeUnit = sizeUnit;
            }

            if(_lastMode != mode)
            {
                viewModel.WidthPrefix = 0;
                _lastMode = mode;
            }

            string prefix = mode switch
            {
                ModeDisplay.Size => GetConvertedSizeLine(sizeUnit, node.Size),
                ModeDisplay.Allocated => GetConvertedSizeLine(sizeUnit, node.Allocated),
                ModeDisplay.File_Count => GetConvertedCountFilesLine(node.CountFiles),
                ModeDisplay.Percent => GetConvertedPercentOfParent(node.PercentOfParent),
                _ => string.Empty
            };

            var widthInPixels = prefix.Length * 7;
            viewModel.WidthPrefix = widthInPixels > viewModel.WidthPrefix ? widthInPixels : viewModel.WidthPrefix;

            return $"{prefix}   ";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string GetConvertedSizeLine(SizeUnit sizeUnit, double size)
        {
            var values = new object[] { sizeUnit, size };
            return _sizeConverter.Convert(values, null, null, null).ToString();
        }

        private string GetConvertedCountFilesLine(int countFiles)
        {
            return $"{countFiles} Files";
        }

        private string GetConvertedPercentOfParent(double percent)
        {
            return $"{percent} %";
        }
    }
}
