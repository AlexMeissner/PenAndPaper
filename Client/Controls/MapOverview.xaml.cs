using Client.Services;
using Client.ViewModels;
using Client.Windows;
using DataTransfer.Map;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class MapOverview : UserControl
    {
        public MapOverviewViewModel ViewModel => (MapOverviewViewModel)DataContext;

        public MapOverview(IViewModelProvider viewModelProvider)
        {
            DataContext = viewModelProvider.Get<MapOverviewViewModel>();

            InitializeComponent();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.Update();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(OverviewItemsControl.ItemsSource).Refresh();
        }

        private async void OnCreateMap(object sender, RoutedEventArgs e)
        {
            MapCreationWindow mapCreationWindow = new();

            if (mapCreationWindow.ShowDialog() == true)
            {
                var payload = new MapDto(
                    Id: mapCreationWindow.ViewModel.Id,
                    CampaignId: mapCreationWindow.ViewModel.CampaignId,
                    Name: mapCreationWindow.ViewModel.Name,
                    ImageData: mapCreationWindow.ViewModel.ImageData,
                    Grid: new GridDto(mapCreationWindow.ViewModel.GridSize, mapCreationWindow.ViewModel.GridIsActive));

                await ViewModel.CreateMap(payload);
            }

        }

        private async void OnEdit(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var item = (MapOverviewItemDto)button.DataContext;

            var response = await ViewModel.GetMap(item);

            response.Match(
                async success =>
                {
                    var mapCreationWindow = new MapCreationWindow(success);

                    if (mapCreationWindow.ShowDialog() == true)
                    {
                        var payload = new MapDto(
                            Id: mapCreationWindow.ViewModel.Id,
                            CampaignId: mapCreationWindow.ViewModel.CampaignId,
                            Name: mapCreationWindow.ViewModel.Name,
                            ImageData: mapCreationWindow.ViewModel.ImageData,
                            Grid: new GridDto(mapCreationWindow.ViewModel.GridSize, mapCreationWindow.ViewModel.GridIsActive));

                        await ViewModel.UpdateMap(payload);
                    }
                },
                failure =>
                {
                    MessageBoxUtility.Show(failure);
                });
        }
    }
}