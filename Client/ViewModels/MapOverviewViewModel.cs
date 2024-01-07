using Client.Commands;
using Client.Services;
using Client.Services.API;
using DataTransfer.Map;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class MapOverviewViewModel : BaseViewModel
    {
        private readonly IMapApi _mapApi;
        private readonly IMapOverviewApi _mapOverviewApi;
        private readonly IActiveMapApi _activeMapApi;
        private readonly ISessionData _sessionData;
        private readonly IUpdateNotifier _updateNotifier;

        public string Filter { get; set; } = string.Empty;

        public ObservableCollection<MapOverviewItemDto> Items { get; set; } = new();

        public ICommand DeleteCommand { get; set; }
        public ICommand PlayCommand { get; set; }


        public MapOverviewViewModel(IMapApi mapApi, IMapOverviewApi mapOverviewApi, IActiveMapApi activeMapApi, ISessionData sessionData, IUpdateNotifier updateNotifier)
        {
            _mapApi = mapApi;
            _mapOverviewApi = mapOverviewApi;
            _activeMapApi = activeMapApi;
            _sessionData = sessionData;
            _updateNotifier = updateNotifier;

            _updateNotifier.MapCollectionChanged += OnMapCollectionChanged;

            DeleteCommand = new AsyncCommand<MapOverviewItemDto>(OnDelete);
            PlayCommand = new AsyncCommand<MapOverviewItemDto>(OnPlay);
        }

        public void UnsubscribeEventHandlers()
        {
            _updateNotifier.MapCollectionChanged -= OnMapCollectionChanged;
        }

        public async Task<HttpResponse<MapDto>> GetMap(MapOverviewItemDto item)
        {
            return await _mapApi.GetAsync(item.MapId);
        }

        public async Task<HttpResponse> UpdateMap(MapDto payload)
        {
            return await _mapApi.PutAsync(payload with { CampaignId = _sessionData.CampaignId });
        }

        public async Task Update()
        {
            var response = await _mapOverviewApi.GetAsync(_sessionData.CampaignId);
            response.Match(success => { Items.ReplaceWith(success.Items); });
        }

        public bool OnFilter(object item)
        {
            if (string.IsNullOrEmpty(Filter))
            {
                return true;
            }

            if (item is MapOverviewItemDto mapOverviewItem)
            {
                return mapOverviewItem.Name.ToUpper().Contains(Filter.ToUpper());
            }

            return false;
        }

        public async Task OnDelete(MapOverviewItemDto item)
        {
            await _mapApi.DeleteAsync(item.MapId);
        }

        public async Task OnPlay(MapOverviewItemDto item)
        {
            var payload = new ActiveMapDto(_sessionData.CampaignId, item.MapId);

            await _activeMapApi.PutAsync(payload);
        }

        private async void OnMapCollectionChanged(object? sender, EventArgs e)
        {
            await Update();
        }
    }
}
