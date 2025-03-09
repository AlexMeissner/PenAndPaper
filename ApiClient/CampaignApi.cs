using DataTransfer.Campaign;
using DataTransfer.Response;

namespace ApiClient;

public interface ICampaignApi
{
    Task<Response<int>> CreateAsync(CampaignCreationDto payload);
    Task<Response<CampaignDto>> GetAsync(int campaignId);
    Task<Response<IEnumerable<CampaignsDto>>> GetAllAsync();
    Task<Response> ModifyAsync(int campaignId, CampaignUpdateDto payload);
}

public class CampaignApi(IRequestBuilder requestBuilder) : ICampaignApi
{
    public Task<Response<int>> CreateAsync(CampaignCreationDto payload)
    {
        return requestBuilder.Path("campaigns").PostAsync<int>(payload);
    }

    public Task<Response<CampaignDto>> GetAsync(int campaignId)
    {
        return requestBuilder.Path("campaigns", campaignId).GetAsync<CampaignDto>();
    }

    public Task<Response<IEnumerable<CampaignsDto>>> GetAllAsync()
    {
        return requestBuilder.Path("campaigns").GetAsync<IEnumerable<CampaignsDto>>();
    }

    public Task<Response> ModifyAsync(int campaignId, CampaignUpdateDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId).PutAsync(payload);
    }
}