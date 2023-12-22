using DataTransfer.Map;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface IMapApi
    {
        public Task<HttpResponse<MapDto>> GetAsync(int MapId);
        public Task<HttpResponse> PostAsync(MapDto payload);
        public Task<HttpResponse> PutAsync(MapDto payload);
        public Task<HttpResponse> DeleteAsync(int MapId);
    }

    [TransistentService]
    public class MapApi : IMapApi
    {
        private readonly HttpRequest _request;

        public MapApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "Map");
        }

        public Task<HttpResponse<MapDto>> GetAsync(int mapId)
        {
            return _request.GetAsync<MapDto>($"mapId={mapId}");
        }

        public Task<HttpResponse> PostAsync(MapDto payload)
        {
            return _request.PostAsync(payload);
        }

        public Task<HttpResponse> PutAsync(MapDto payload)
        {
            return _request.PutAsync(payload);
        }

        public Task<HttpResponse> DeleteAsync(int mapId)
        {
            return _request.DeleteAsync($"mapId={mapId}");
        }
    }
}