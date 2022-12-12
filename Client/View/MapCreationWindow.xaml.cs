using Client.Converter;
using DataTransfer.Map;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Client.View
{
    public partial class MapCreationWindow : Window
    {
        public MapCreationDto MapCreation { get; set; }

        public MapCreationWindow()
        {
            InitializeComponent();
            MapCreation = new();
        }

        public MapCreationWindow(MapCreationDto mapCreation)
        {
            InitializeComponent();
            MapCreation = mapCreation;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnCreate(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnFromFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new()
            {
                Filter = "Bilder (*.png;*.jpeg)|*.png;*.jpeg"
            };

            if (fileDialog.ShowDialog() == true)
            {
                ByteArrayToBitmapImageConverter converter = new();
                BitmapImage image = new(new Uri(fileDialog.FileName));
                MapCreation.ImageData = converter.ConvertBack(image, typeof(byte[]), image, CultureInfo.CurrentCulture) as byte[];
            }
        }
    }
}