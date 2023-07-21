using DataTransfer.CampaignCreation;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ICampaignCreationApi
    {
        public Task<HttpResponse<CampaignCreationDto>> GetAsync(int CampaignId);
        public Task<HttpResponse> PostAsync(CampaignCreationDto payload);
    }

    public class CampaignCreationApi : ICampaignCreationApi
    {
        private readonly HttpRequest _request;

        public CampaignCreationApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "CampaignCreation");
        }

        public Task<HttpResponse<CampaignCreationDto>> GetAsync(int CampaignId)
        {
            return _request.GetAsync<CampaignCreationDto>($"campaignId={CampaignId}");
        }

        public Task<HttpResponse> PostAsync(CampaignCreationDto payload)
        {
            return _request.PostAsync(payload);
        }
    }
}