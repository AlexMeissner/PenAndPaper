using DataTransfer.Script;
using System.Threading.Tasks;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
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

        public ScriptApi(IEndPointProvider endPointProvider, IIdentityProvider identityProvider)
        {
            _request = new(endPointProvider.BaseURL + "Script", identityProvider);
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
