using DataTransfer.Sound;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IActiveSoundApi
    {
        public Task<HttpResponse<ActiveSoundDto>> GetAsync(int campaignId);
        public Task<HttpResponse> PutAsync(ActiveSoundDto payload);
    }

    public class ActiveSoundApi : IActiveSoundApi
    {
        private readonly HttpRequest _request;

        public ActiveSoundApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "ActiveSound");
        }

        public Task<HttpResponse<ActiveSoundDto>> GetAsync(int campaignId)
        {
            return _request.GetAsync<ActiveSoundDto>($"campaignId={campaignId}");
        }

        public Task<HttpResponse> PutAsync(ActiveSoundDto payload)
        {
            return _request.PutAsync(payload);
        }
    }
}
