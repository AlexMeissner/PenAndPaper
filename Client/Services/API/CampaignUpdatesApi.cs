using DataTransfer.Campaign;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ICampaignUpdatesApi
    {
        public Task<HttpResponse<CampaignUpdateDto>> GetAsync(int campaignId);
    }

    public class CampaignUpdatesApi : ICampaignUpdatesApi
    {
        private readonly HttpRequest _request;

        public CampaignUpdatesApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "CampaignUpdates");
        }

        public Task<HttpResponse<CampaignUpdateDto>> GetAsync(int campaignId)
        {
            return _request.GetAsync<CampaignUpdateDto>($"campaignId={campaignId}");
        }
    }
}