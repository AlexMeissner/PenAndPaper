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

public class CampaignApi(IRequest request) : ICampaignApi
{
    public Task<Response<int>> CreateAsync(CampaignCreationDto payload)
    {
        return request.Path("campaigns").PostAsync<int>(payload);
    }

    public Task<Response<CampaignDto>> GetAsync(int campaignId)
    {
        return request.Path("campaigns", campaignId).GetAsync<CampaignDto>();
    }

    public Task<Response<IEnumerable<CampaignsDto>>> GetAllAsync()
    {
        return request.Path("campaigns").GetAsync<IEnumerable<CampaignsDto>>();
    }

    public Task<Response> ModifyAsync(int campaignId, CampaignUpdateDto payload)
    {
        return request.Path("campaigns", campaignId).PutAsync(payload);
    }
}