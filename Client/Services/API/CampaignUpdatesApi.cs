using DataTransfer.Campaign;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface ICampaignUpdatesApi
    {
        public Task<HttpResponse<CampaignUpdateDto>> GetAsync(int campaignId);
    }

    [TransistentService]
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