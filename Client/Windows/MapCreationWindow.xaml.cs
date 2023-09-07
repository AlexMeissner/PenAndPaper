using Client.Converter;
using Client.ViewModels;
using DataTransfer.Map;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Client.Windows
{
    public partial class MapCreationWindow : Window
    {
        public MapCreationViewModel ViewModel { get; init; }

        public MapCreationWindow()
        {
            ViewModel = new MapCreationViewModel();
            InitializeComponent();
        }

        public MapCreationWindow(MapDto mapCreation)
        {
            ViewModel = new MapCreationViewModel()
            {
                Id = mapCreation.Id,
                CampaignId = mapCreation.CampaignId,
                Name = mapCreation.Name,
                ImageData = mapCreation.ImageData,
                GridIsActive = mapCreation.Grid.IsActive,
                GridSize = mapCreation.Grid.Size,
            };

            InitializeComponent();
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
                ViewModel.ImageData = converter.ConvertBack(image);
            }
        }
    }
}