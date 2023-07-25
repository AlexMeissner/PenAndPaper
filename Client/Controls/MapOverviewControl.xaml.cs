using Client.Services;
using Client.Services.API;
using Client.View;
using DataTransfer.Map;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class MapOverviewControl : UserControl
    {
        public MapOverviewDto MapOverview { get; set; } = new();

        private readonly IMapApi _mapApi;
        private readonly IMapOverviewApi _mapOverviewApi;
        private readonly IActiveMapApi _activeMapApi;
        private readonly ISessionData _sessionData;

        public MapOverviewControl(IMapApi mapApi, IMapOverviewApi mapOverviewApi, IActiveMapApi activeMapApi, ISessionData sessionData, ICampaignUpdates campaignUpdates)
        {
            _mapApi = mapApi;
            _mapOverviewApi = mapOverviewApi;
            _activeMapApi = activeMapApi;
            _sessionData = sessionData;

            campaignUpdates.MapCollectionChanged += OnMapCollectionChanged;

            InitializeComponent();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await Update();
        }

        private async void OnMapCollectionChanged(object? sender, EventArgs e)
        {
            await Update();
        }

        private async Task Update()
        {
            if (_sessionData.CampaignId is int campaignId)
            {
                var response = await _mapOverviewApi.GetAsync(campaignId);

                response.Match(
                    success =>
                    {
                        MapOverview.Items = success.Items;
                    },
                    failure =>
                    {
                        MessageBoxUtility.Show(failure);
                    });
            }
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

        private async void OnCreateMap(object sender, RoutedEventArgs e)
        {
            MapCreationWindow mapCreationWindow = new();

            if (mapCreationWindow.ShowDialog() == true && _sessionData.CampaignId is int campaignId)
            {
                mapCreationWindow.MapCreation.CampaignId = campaignId;
                await _mapApi.PostAsync(mapCreationWindow.MapCreation);
            }
        }

        private async void OnPlay(object sender, RoutedEventArgs e)
        {
            if (_sessionData.CampaignId is int campaignId &&
                sender is Button button &&
                button.DataContext is MapOverviewItemDto mapOverviewItemDto)
            {
                var payload = new ActiveMapDto()
                {
                    CampaignId = campaignId,
                    MapId = mapOverviewItemDto.MapId,
                };

                var response = await _activeMapApi.PutAsync(payload);

                if (response.Failed)
                {
                    MessageBoxUtility.Show(response.StatusCode);
                }
            }
        }

        private async void OnEdit(object sender, RoutedEventArgs e)
        {
            if (_sessionData.CampaignId is int campaignId &&
                sender is Button button &&
                button.DataContext is MapOverviewItemDto mapOverviewItemDto)
            {
                var response = await _mapApi.GetAsync(mapOverviewItemDto.MapId);

                response.Match(
                    async success =>
                    {
                        MapCreationWindow mapCreationWindow = new(success);

                        if (mapCreationWindow.ShowDialog() == true)
                        {
                            mapCreationWindow.MapCreation.CampaignId = campaignId;
                            await _mapApi.PutAsync(mapCreationWindow.MapCreation);
                        }
                    },
                    failure =>
                    {
                        MessageBoxUtility.Show(failure);
                    });
            }
        }

        private async void OnDelete(object sender, RoutedEventArgs e)
        {
            if (sender is Button button &&
                button.DataContext is MapOverviewItemDto mapOverviewItemDto)
            {
                var response = await _mapApi.DeleteAsync(mapOverviewItemDto.MapId);

                if (response.Failed)
                {
                    MessageBoxUtility.Show(response.StatusCode);
                }
            }
        }
    }
}