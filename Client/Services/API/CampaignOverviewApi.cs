using Client.Helper;
using DataTransfer;
using DataTransfer.CampaignSelection;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ICampaignOverviewApi
    {
        public Task<ApiResponse<CampaignOverviewDto>> GetAsync(int userId);
    }

    public class CampaignOverviewApi : ICampaignOverviewApi
    {
        private readonly IEndPointProvider _endPointProvider;

        public CampaignOverviewApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<CampaignOverviewDto>> GetAsync(int userId)
        {
            string url = _endPointProvider.BaseURL + $"CampaignOverview?userId={userId}";
            return url.GetAsync<CampaignOverviewDto>();
        }
    }
}