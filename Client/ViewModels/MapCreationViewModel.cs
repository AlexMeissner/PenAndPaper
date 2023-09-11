using Client.Commands;
using Client.Services;
using Client.Services.API;
using DataTransfer.Map;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class MapCreationViewModel : BaseViewModel
    {
        private bool _isEdit = false;

        private readonly IMapApi _mapApi;
        private readonly ISessionData _sessionData;
        private readonly IPopupPage _popupPage;

        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public int GridSize { get; set; } = 100;
        public bool GridIsActive { get; set; } = false;

        public ICommand CreateCommand { get; set; }

        public MapCreationViewModel(IMapApi mapApi, ISessionData sessionData, IPopupPage popupPage)
        {
            _mapApi = mapApi;
            _sessionData = sessionData;
            _popupPage = popupPage;
            CreateCommand = new AsyncCommand(Create);
        }

        public async Task Create()
        {
            var payload = new MapDto(
                Id: Id,
                CampaignId: CampaignId,
                Name: Name,
                ImageData: ImageData,
                Grid: new GridDto(GridSize, GridIsActive));

            if (_isEdit)
            {
                await _mapApi.PutAsync(payload);
            }
            else
            {
                // ToDo: is _sessionData.CampaignId required?
                await _mapApi.PostAsync(payload with { CampaignId = _sessionData.CampaignId });
            }

            _popupPage.CloseCommand.Execute(null);
        }

        public void Load(MapDto map)
        {
            Id = map.Id;
            CampaignId = map.CampaignId;
            Name = map.Name;
            ImageData = map.ImageData;
            GridSize = map.Grid.Size;
            GridIsActive = map.Grid.IsActive;

            _isEdit = true;
        }
    }
}
