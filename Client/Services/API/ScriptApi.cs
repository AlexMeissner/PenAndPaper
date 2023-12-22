using DataTransfer.Script;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface IScriptApi
    {
        Task<HttpResponse<ScriptDto>> Get(int mapId);
        Task<HttpResponse> Update(ScriptDto payload);
    }

    [TransistentService]
    public class ScriptApi : IScriptApi
    {
        private readonly HttpRequest _request;

        public ScriptApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "Script");
        }

        public Task<HttpResponse<ScriptDto>> Get(int mapId)
        {
            return _request.GetAsync<ScriptDto>($"mapId={mapId}");
        }

        public Task<HttpResponse> Update(ScriptDto payload)
        {
            return _request.PutAsync(payload);
        }
    }
}
