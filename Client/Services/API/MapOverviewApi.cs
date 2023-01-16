using Client.Helper;
using DataTransfer;
using DataTransfer.Map;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IMapOverviewApi
    {
        public Task<ApiResponse<MapOverviewDto>> GetAsync(int campaignId);
    }

    public class MapOverviewApi : IMapOverviewApi
    {
        public Task<ApiResponse<MapOverviewDto>> GetAsync(int campaignId)
        {
            // TODO
            string url = $"https://localhost:7099/MapOverview?campaignId={campaignId}";
            return url.GetAsync<MapOverviewDto>();
        }
    }
}