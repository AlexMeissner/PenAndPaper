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
        private readonly IEndPointProvider _endPointProvider;

        public CampaignCreationApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<CampaignCreationDto>> GetAsync(int CampaignId)
        {
            string url = _endPointProvider.BaseURL + $"CampaignCreation?campaignId={CampaignId}";
            return url.GetAsync<CampaignCreationDto>();
        }

        public Task<ApiResponse> PostAsync(CampaignCreationDto payload)
        {
            string url = _endPointProvider.BaseURL + "CampaignCreation";
            return url.PostAsync(payload);
        }
    }
}