using Client.Helper;
using DataTransfer;
using DataTransfer.Campaign;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ICampaignUpdatesApi
    {
        public Task<ApiResponse<CampaignUpdateDto>> GetAsync(int campaignId);
    }

    public class CampaignUpdatesApi : ICampaignUpdatesApi
    {
        public Task<ApiResponse<CampaignUpdateDto>> GetAsync(int campaignId)
        {
            // TODO
            string url = $"https://localhost:7099/CampaignUpdates?campaignId={campaignId}";
            return url.GetAsync<CampaignUpdateDto>();
        }
    }
}