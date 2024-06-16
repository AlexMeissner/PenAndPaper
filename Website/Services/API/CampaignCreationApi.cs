using DataTransfer.Campaign;
using System.Threading.Tasks;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface ICampaignCreationApi
    {
        public Task<HttpResponse<CampaignCreationDto>> GetAsync(int campaignId, int userId);
        public Task<HttpResponse> PostAsync(CampaignCreationDto payload);
        public Task<HttpResponse> PutAsync(CampaignCreationDto payload);
    }

    [TransistentService]
    public class CampaignCreationApi(IEndPointProvider endPointProvider) : ICampaignCreationApi
    {
        private readonly HttpRequest _request = new(endPointProvider.BaseURL + "CampaignCreation");

        public Task<HttpResponse<CampaignCreationDto>> GetAsync(int campaignId, int userId)
        {
            return _request.GetAsync<CampaignCreationDto>($"campaignId={campaignId}&userId={userId}");
        }

        public Task<HttpResponse> PostAsync(CampaignCreationDto payload)
        {
            return _request.PostAsync(payload);
        }

        public Task<HttpResponse> PutAsync(CampaignCreationDto payload)
        {
            return _request.PutAsync(payload);
        }
    }
}