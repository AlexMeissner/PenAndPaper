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

public class ScriptApi(IRequest request) : IScriptApi
{
    public Task<Response<ScriptDto>> GetAsync(int mapId)
    {
        return request.Path("maps", mapId, "scripts").GetAsync<ScriptDto>();
    }

    public Task<Response<IEnumerable<ScriptsDto>>> GetAllAsync(int campaignId)
    {
        return request.Path("campaigns", campaignId, "scripts").GetAsync<IEnumerable<ScriptsDto>>();
    }
    
    public Task<Response> Update(int mapId, ScriptUpdateDto payload)
    {
        return request.Path("maps", mapId).PatchAsync(payload);
    }
}