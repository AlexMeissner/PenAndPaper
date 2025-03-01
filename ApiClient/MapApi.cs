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

public class MapApi(IRequest request) : IMapApi
{
    public Task<Response<int>> CreateAsync(int campaignId, MapCreationDto payload)
    {
        return request.Path("campaigns", campaignId, "maps").PostAsync<int>(payload);
    }

    public Task<Response<ActiveMapDto>> GetActiveMapAsync(int campaignId)
    {
        return request.Path("campaigns", campaignId, "active-map").GetAsync<ActiveMapDto>();
    }

    public Task<Response<IEnumerable<MapsDto>>> GetAllAsync(int campaignId)
    {
        return request.Path("campaigns", campaignId, "maps").GetAsync<IEnumerable<MapsDto>>();
    }

    public Task<Response<MapDto>> GetAsync(int mapId)
    {
        return request.Path("maps", mapId).GetAsync<MapDto>();
    }

    public Task<Response> RemoveAsync(int mapId)
    {
        return request.Path("maps", mapId).DeleteAsync();
    }

    public Task<Response> SetActiveMapAsync(int campaignId, ActiveMapUpdateDto payload)
    {
        return request.Path("campaigns", campaignId, "active-map").PatchAsync(payload);
    }

    public Task<Response> UpdateGrid(int mapId, GridUpdateDto payload)
    {
        return request.Path("maps", mapId).PatchAsync(payload);
    }

    public Task<Response> UpdateNameAsync(int mapId, NameUpdateDto payload)
    {
        return request.Path("maps", mapId).PatchAsync(payload);
    }
}