using DataTransfer.Map;
using DataTransfer.Response;

namespace ApiClient;

public interface IMapApi
{
    Task<Response<int>> CreateAsync(int campaignId, MapCreationDto payload);
    Task<Response<ActiveMapDto>> GetActiveMapAsync(int campaignId);
    Task<Response<IEnumerable<MapsDto>>> GetAllAsync(int campaignId);
    Task<Response<MapDto>> GetAsync(int mapId);
    Task<Response> RemoveAsync(int mapId);
    Task<Response> SetActiveMapAsync(int campaignId, ActiveMapUpdateDto payload);
    Task<Response> UpdateGrid(int mapId, GridUpdateDto payload);
    Task<Response> UpdateNameAsync(int mapId, NameUpdateDto payload);
}

public class MapApi(IRequestBuilder requestBuilder) : IMapApi
{
    public Task<Response<int>> CreateAsync(int campaignId, MapCreationDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "maps").PostAsync<int>(payload);
    }

    public Task<Response<ActiveMapDto>> GetActiveMapAsync(int campaignId)
    {
        return requestBuilder.Path("campaigns", campaignId, "active-map").GetAsync<ActiveMapDto>();
    }

    public Task<Response<IEnumerable<MapsDto>>> GetAllAsync(int campaignId)
    {
        return requestBuilder.Path("campaigns", campaignId, "maps").GetAsync<IEnumerable<MapsDto>>();
    }

    public Task<Response<MapDto>> GetAsync(int mapId)
    {
        return requestBuilder.Path("maps", mapId).GetAsync<MapDto>();
    }

    public Task<Response> RemoveAsync(int mapId)
    {
        return requestBuilder.Path("maps", mapId).DeleteAsync();
    }

    public Task<Response> SetActiveMapAsync(int campaignId, ActiveMapUpdateDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "active-map").PatchAsync(payload);
    }

    public Task<Response> UpdateGrid(int mapId, GridUpdateDto payload)
    {
        return requestBuilder.Path("maps", mapId).PatchAsync(payload);
    }

    public Task<Response> UpdateNameAsync(int mapId, NameUpdateDto payload)
    {
        return requestBuilder.Path("maps", mapId).PatchAsync(payload);
    }
}