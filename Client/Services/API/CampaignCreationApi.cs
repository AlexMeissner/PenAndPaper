using Client.Helper;
using DataTransfer;
using DataTransfer.CampaignCreation;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ICampaignCreationApi
    {
        public Task<ApiResponse<CampaignCreationDto>> GetAsync(int CampaignId);
        public Task<ApiResponse> PostAsync(CampaignCreationDto payload);
    }

    public class CampaignCreationApi : ICampaignCreationApi
    {
        public Task<ApiResponse<CampaignCreationDto>> GetAsync(int CampaignId)
        {
            // TODO
            string url = $"https://localhost:7099/CampaignCreation?campaignId={CampaignId}";
            return url.GetAsync<CampaignCreationDto>();
        }

        public Task<ApiResponse> PostAsync(CampaignCreationDto payload)
        {
            // TODO
            string url = $"https://localhost:7099/CampaignCreation";
            return url.PostAsync(payload);
        }
    }
}