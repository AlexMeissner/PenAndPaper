using Client.Commands;
using Client.Services;
using Client.Services.API;
using DataTransfer.Map;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class MapCreationViewModel : BaseViewModel
    {
        private bool isEdit = false;
        private Point initialMapOffset;
        private Point initialMousePosition;

        private readonly IMapApi _mapApi;
        private readonly ISessionData _sessionData;
        private readonly IPopupPage _popupPage;

        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] ImageData { get; set; } = [];
        public int GridSize { get; set; } = 100;
        public bool GridIsActive { get; set; } = false;
        public MapTransformation MapTransformation { get; set; } = new();

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

            if (isEdit)
            {
                await _mapApi.PutAsync(payload);
            }
            else
            {
                // ToDo: is _sessionData.CampaignId required?
                await _mapApi.PostAsync(payload with { CampaignId = _sessionData.CampaignId });
            }

            _popupPage.Close();
        }

        public void Load(MapDto map)
        {
            Id = map.Id;
            CampaignId = map.CampaignId;
            Name = map.Name;
            ImageData = map.ImageData;
            GridSize = map.Grid.Size;
            GridIsActive = map.Grid.IsActive;

            isEdit = true;
        }

        public void Zoom(Point position, int delta)
        {
            MapTransformation.Zoom(position, delta);
        }

        public void SetInitialMousePosition(Point position)
        {
            initialMousePosition = position;
            initialMapOffset.X = MapTransformation.X;
            initialMapOffset.Y = MapTransformation.Y;
        }

        public void MoveMap(Point position)
        {
            var delta = Point.Subtract(position, initialMousePosition) / MapTransformation.Scaling.Matrix.M11;
            var offsetPosition = initialMapOffset + delta;
            MapTransformation.Move(offsetPosition);
        }
    }
}
