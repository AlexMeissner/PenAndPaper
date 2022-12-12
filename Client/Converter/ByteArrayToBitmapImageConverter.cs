﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Client.Converter
{
    public class ByteArrayToBitmapImageConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] data)
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
    }
}