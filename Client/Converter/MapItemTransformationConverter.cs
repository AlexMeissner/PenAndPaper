using System;
using System.Globalization;
using System.Windows.Data;

namespace Client.Converter
{
    public class MapItemTransformationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double position = 0;

            foreach (var item in values)
            {
                if (item is int offset)
                {
                    position += offset;
                }
            }

            return position;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
