using DataTransfer.Campaign;
using static Website.Services.ServiceExtension;

namespace Website.Services.API;

public interface ICampaignOverviewApi
{
    public Task<HttpResponse<CampaignOverviewDto>> GetAsync(int userId);
}

[TransistentService]
public class CampaignOverviewApi(IEndPointProvider endPointProvider, IIdentityProvider identityProvider) : ICampaignOverviewApi
{
    private readonly HttpRequest _request = new(endPointProvider.BaseURL + "CampaignOverview", identityProvider);

    public Task<HttpResponse<CampaignOverviewDto>> GetAsync(int userId)
    {
        return _request.GetAsync<CampaignOverviewDto>($"userId={userId}");
    }
}