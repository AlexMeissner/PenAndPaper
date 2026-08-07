using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MonsterManager.Converter
{
    internal class ByteArrayToBitmapImageConverter : IValueConverter
    {
        private static readonly CultureInfo CultureInfo = new("en-US");

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] data && data.Length > 0)
            {
                using MemoryStream stream = new(data);

                BitmapImage image = new();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();

                return image;
            }

            return null;
        }

        public BitmapImage Convert(byte[] data)
        {
            return (BitmapImage)Convert(data, typeof(BitmapImage), data, CultureInfo)!;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BitmapImage image)
            {
                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(image));

                using MemoryStream stream = new();
                encoder.Save(stream);
                return stream.ToArray();
            }

            return null;
        }

        public byte[] ConvertBack(BitmapImage image)
        {
            return (byte[])ConvertBack(image, typeof(byte[]), image, CultureInfo)!;
        }
    }
}
