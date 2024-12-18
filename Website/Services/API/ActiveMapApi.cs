using DataTransfer.Map;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface IActiveMapApi
    {
        public Task<HttpResponse<ActiveMapDto>> GetAsync(int campaignId);
        public Task<HttpResponse> PutAsync(ActiveMapDto payload);
    }

    [TransistentService]
    public class ActiveMapApi(IEndPointProvider endPointProvider, IIdentityProvider identityProvider) : IActiveMapApi
    {
        private readonly HttpRequest _request = new(endPointProvider.BaseURL + "ActiveMap", identityProvider);

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
