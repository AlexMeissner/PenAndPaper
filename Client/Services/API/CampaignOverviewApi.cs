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
        public Task<ApiResponse<CampaignOverviewDto>> GetAsync(int userId)
        {
            // TODO
            string url = $"https://localhost:7099/CampaignOverview?userId={userId}";
            return url.GetAsync<CampaignOverviewDto>();
        }
    }
}