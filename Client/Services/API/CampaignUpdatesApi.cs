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
        private readonly IEndPointProvider _endPointProvider;

        public CampaignUpdatesApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<CampaignUpdateDto>> GetAsync(int campaignId)
        {
            string url = _endPointProvider.BaseURL + $"CampaignUpdates?campaignId={campaignId}";
            return url.GetAsync<CampaignUpdateDto>();
        }
    }
}