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
        public Task<ApiResponse<MapDto>> GetAsync(int MapId)
        {
            // TODO
            string url = $"https://localhost:7099/Map?mapId={MapId}";
            return url.GetAsync<MapDto>();
        }

        public Task<ApiResponse> PostAsync(MapDto payload)
        {
            // TODO
            string url = $"https://localhost:7099/Map";
            return url.PostAsync(payload);
        }

        public Task<ApiResponse> PutAsync(MapDto payload)
        {
            // TODO
            string url = $"https://localhost:7099/Map";
            return url.PutAsync(payload);
        }

        public Task<ApiResponse> DeleteAsync(int MapId)
        {
            // TODO
            string url = $"https://localhost:7099/Map?mapId={MapId}";
            return url.DeleteAsync();
        }
    }
}