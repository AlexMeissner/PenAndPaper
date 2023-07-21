using DataTransfer.CampaignSelection;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ICampaignOverviewApi
    {
        public Task<HttpResponse<CampaignOverviewDto>> GetAsync(int userId);
    }

    public class CampaignOverviewApi : ICampaignOverviewApi
    {
        private readonly HttpRequest _request;

        public CampaignOverviewApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "CampaignOverview");
        }

        public Task<HttpResponse<CampaignOverviewDto>> GetAsync(int userId)
        {
            return _request.GetAsync<CampaignOverviewDto>($"userId={userId}");
        }
    }
}