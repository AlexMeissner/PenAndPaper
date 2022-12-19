using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Client.Converter
{
    public class GridToRectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int size)
            {
                return new Rect(0, 0, size, size);
            }

            throw new NotImplementedException();
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}