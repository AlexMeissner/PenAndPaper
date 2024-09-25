using DataTransfer.Settings;
using System.Threading.Tasks;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface ISettingsApi
    {
        public Task<HttpResponse<SettingsDto>> GetAsync();
        public Task<HttpResponse> PostAsync(SettingsDto settings);
        public Task<HttpResponse> PutAsync(SettingsDto settings);
    }

    [TransistentService]
    public class SettingsApi(IEndPointProvider endPointProvider, ITokenProvider tokenProvider) : ISettingsApi
    {
        private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Settings", tokenProvider);

        public Task<HttpResponse<SettingsDto>> GetAsync()
        {
            return _request.GetAsync<SettingsDto>("id=1");
        }

        public Task<HttpResponse> PostAsync(SettingsDto settings)
        {
            return _request.PostAsync(settings);
        }

        public Task<HttpResponse> PutAsync(SettingsDto settings)
        {
            return _request.PutAsync(settings);
        }
    }
}
