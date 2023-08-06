using DataTransfer.Campaign;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface ICampaignCreationApi
    {
        public Task<HttpResponse<CampaignCreationDto>> GetAsync(int CampaignId);
        public Task<HttpResponse> PostAsync(CampaignCreationDto payload);
    }

    [TransistentService]
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