using Client.Converter;
using Client.View;
using DataTransfer.Map;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Client.Controls
{
    public partial class MapOverviewControl : UserControl
    {
        public MapOverviewDto MapOverview { get; set; }

        public MapOverviewControl()
        {
            ByteArrayToBitmapImageConverter converter = new();
            BitmapImage image = new(new Uri(@"W:\PenAndPaper\LoTR\WhosThatCharacter.png"));
            var data = converter.ConvertBack(image, typeof(byte[]), image, CultureInfo.CurrentCulture) as byte[];

            MapOverview = new()
            {
                Items = new[]
                {
                    new MapOverviewItemDto() { Name="Peter", ImageData = data },
                    new MapOverviewItemDto() { Name="Hans", ImageData = data },
                    new MapOverviewItemDto() { Name="Gustav", ImageData = data },
                    new MapOverviewItemDto() { Name="Olaf", ImageData = data },
                    new MapOverviewItemDto() { Name="Günther", ImageData = data },
                }
            }; // TODO: Get from Rest API

            InitializeComponent();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(OverviewItemsControl.ItemsSource);
            view.Filter = OnFilter;
        }

        private bool OnFilter(object item)
        {
            if (string.IsNullOrEmpty(FilterTextBox.Text))
            {
                return true;
            }

            if (item is MapOverviewItemDto mapOverviewItem)
            {
                return mapOverviewItem.Name.ToUpper().Contains(FilterTextBox.Text.ToUpper());
            }

            return false;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(OverviewItemsControl.ItemsSource).Refresh();
        }

        private void OnCreateMap(object sender, RoutedEventArgs e)
        {
            MapCreationWindow mapCreationWindow = new();

            if (mapCreationWindow.ShowDialog() == true)
            {
                // TODO: Add new item to view and upload via Rest API
                // mapCreationWindow.MapCreation;
            }
        }
    }
}