using DataTransfer.Settings;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface ISettingsApi
    {
        public Task<HttpResponse<SettingsDto>> GetAsync();
        public Task<HttpResponse> PostAsync(SettingsDto settings);
        public Task<HttpResponse> PutAsync(SettingsDto settings);
    }

    [TransistentService]
    public class SettingsApi(IEndPointProvider endPointProvider) : ISettingsApi
    {
        private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Settings");

        public Task<HttpResponse<SettingsDto>> GetAsync()
        {
            return _request.GetAsync<SettingsDto>();
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
