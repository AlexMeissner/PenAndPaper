using Client.Helper;
using DataTransfer;
using DataTransfer.Map;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IMapApi
    {
        public Task<ApiResponse<MapDto>> GetAsync(int MapId);
        public Task<ApiResponse> PostAsync(MapDto payload);
        public Task<ApiResponse> PutAsync(MapDto payload);
        public Task<ApiResponse> DeleteAsync(int MapId);
    }

    public class MapApi : IMapApi
    {
        private readonly IEndPointProvider _endPointProvider;

        public MapApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<MapDto>> GetAsync(int MapId)
        {
            string url = _endPointProvider.BaseURL + $"Map?mapId={MapId}";
            return url.GetAsync<MapDto>();
        }

        public Task<ApiResponse> PostAsync(MapDto payload)
        {
            string url = _endPointProvider.BaseURL + "Map";
            return url.PostAsync(payload);
        }

        public Task<ApiResponse> PutAsync(MapDto payload)
        {
            string url = _endPointProvider.BaseURL + "Map";
            return url.PutAsync(payload);
        }

        public Task<ApiResponse> DeleteAsync(int MapId)
        {
            string url = _endPointProvider.BaseURL + $"Map?mapId={MapId}";
            return url.DeleteAsync();
        }
    }
}