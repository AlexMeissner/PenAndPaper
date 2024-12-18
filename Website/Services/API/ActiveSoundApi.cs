using DataTransfer.Sound;
using System.Threading.Tasks;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface IActiveSoundApi
    {
        public Task<HttpResponse<ActiveAmbientSoundDto>> GetAmbientSoundAsync(int campaignId);
        public Task<HttpResponse<ActiveSoundEffectDto>> GetSoundEffectAsync(int campaignId);
        public Task<HttpResponse> PutAmbientSoundAsync(ActiveAmbientSoundDto payload);
        public Task<HttpResponse> PutSoundEffectAsync(ActiveSoundEffectDto payload);
    }

    [TransistentService]
    public class ActiveSoundApi(IEndPointProvider endPointProvider,  ITokenProvider tokenProvider) : IActiveSoundApi
    {
        private readonly HttpRequest _ambientRequest = new(endPointProvider.BaseURL + "ActiveAmbientSound", tokenProvider);
        private readonly HttpRequest _effectRequest = new(endPointProvider.BaseURL + "ActiveSoundEffect", tokenProvider);

        public Task<HttpResponse<ActiveAmbientSoundDto>> GetAmbientSoundAsync(int campaignId)
        {
            return _ambientRequest.GetAsync<ActiveAmbientSoundDto>($"campaignId={campaignId}");
        }

        public Task<HttpResponse<ActiveSoundEffectDto>> GetSoundEffectAsync(int campaignId)
        {
            return _effectRequest.GetAsync<ActiveSoundEffectDto>($"campaignId={campaignId}");
        }

        public Task<HttpResponse> PutAmbientSoundAsync(ActiveAmbientSoundDto payload)
        {
            return _ambientRequest.PutAsync(payload);
        }

        public Task<HttpResponse> PutSoundEffectAsync(ActiveSoundEffectDto payload)
        {
            return _effectRequest.PutAsync(payload);
        }
    }
}
