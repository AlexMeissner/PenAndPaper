using MarkdownSharp;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Client.Converter
{
    public class TextToMarkdownConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Markdown().Transform((string)value);
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}