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
        private readonly IEndPointProvider _endPointProvider;

        public MapOverviewApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<MapOverviewDto>> GetAsync(int campaignId)
        {
            string url = _endPointProvider.BaseURL + $"MapOverview?campaignId={campaignId}";
            return url.GetAsync<MapOverviewDto>();
        }
    }
}