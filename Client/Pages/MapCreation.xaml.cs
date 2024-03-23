using Client.Converter;
using Client.Services;
using Client.ViewModels;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using static Client.Services.ServiceExtension;

namespace Client.Pages
{
    [TransistentService]
    public partial class MapCreation : Page
    {
        public IPopupPage PopupPage { get; }

        public MapCreationViewModel ViewModel { get; set; }

        public MapCreation(IViewModelProvider viewModelProvider, IPopupPage popupPage)
        {
            PopupPage = popupPage;
            ViewModel = viewModelProvider.Get<MapCreationViewModel>();

            InitializeComponent();
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mousePostion = e.GetPosition(this);
            ViewModel.Zoom(mousePostion, e.Delta);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(this);
            ViewModel.SetInitialMousePosition(mousePosition);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(this);

            if (e.RightButton == MouseButtonState.Pressed)
            {
                ViewModel.MoveMap(mousePosition);
            }
        }

        private void OnOpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new()
            {
                Filter = "Bilder (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg"
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
