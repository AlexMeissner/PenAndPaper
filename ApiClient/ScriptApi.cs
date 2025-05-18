using DataTransfer.Map;
using DataTransfer.Response;
using DataTransfer.Script;

namespace ApiClient;

public interface IScriptApi
{
    Task<Response<ScriptDto>> GetAsync(int mapId);
    Task<Response<IEnumerable<ScriptsDto>>> GetAllAsync(int campaignId);
    Task<Response> Update(int mapId, ScriptUpdateDto payload);
}

public class ScriptApi(IRequestBuilder requestBuilder) : IScriptApi
{
    public Task<Response<ScriptDto>> GetAsync(int mapId)
    {
        return requestBuilder.Path("maps", mapId, "script").GetAsync<ScriptDto>();
    }

    public Task<Response<IEnumerable<ScriptsDto>>> GetAllAsync(int campaignId)
    {
        return requestBuilder.Path("campaigns", campaignId, "scripts").GetAsync<IEnumerable<ScriptsDto>>();
    }

    public Task<Response> Update(int mapId, ScriptUpdateDto payload)
    {
        var mapUpdate = new MapUpdateDto(null, payload, null);
        return requestBuilder.Path("maps", mapId).PatchAsync(mapUpdate);
    }
}