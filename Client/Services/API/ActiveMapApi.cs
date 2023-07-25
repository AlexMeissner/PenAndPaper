using DataTransfer.Map;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface IActiveMapApi
    {
        public Task<HttpResponse<ActiveMapDto>> GetAsync(int campaignId);
        public Task<HttpResponse> PutAsync(ActiveMapDto payload);
    }

    [TransistentService]
    public class ActiveMapApi : IActiveMapApi
    {
        private readonly HttpRequest _request;

        public ActiveMapApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "ActiveMap");
        }

        public Task<HttpResponse<ActiveMapDto>> GetAsync(int campaignId)
        {
            return _request.GetAsync<ActiveMapDto>($"campaignId={campaignId}");
        }

        public Task<HttpResponse> PutAsync(ActiveMapDto payload)
        {
            return _request.PutAsync(payload);
        }
    }
}
