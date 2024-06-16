using DataTransfer.Sound;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface ISoundApi
    {
        public Task<HttpResponse<SoundOverviewDto>> GetOverviewAsync();
        public Task<HttpResponse<SoundDto>> GetAsync(int id);
        public Task<HttpResponse<SoundDataDto>> GetDataAsync(int id);
        public Task<HttpResponse> PostAsync(SoundCreationDto payload);
    }

    [TransistentService]
    public class SoundApi(IEndPointProvider endPointProvider) : ISoundApi
    {
        private readonly HttpRequest _soundRequest = new(endPointProvider.BaseURL + "Sound");
        private readonly HttpRequest _soundDataRequest = new(endPointProvider.BaseURL + "SoundData");
        private readonly HttpRequest _soundOverviewRequest = new(endPointProvider.BaseURL + "SoundOverview");

        public Task<HttpResponse<SoundDto>> GetAsync(int id)
        {
            return _soundRequest.GetAsync<SoundDto>($"id={id}");
        }

        public Task<HttpResponse<SoundDataDto>> GetDataAsync(int id)
        {
            return _soundDataRequest.GetAsync<SoundDataDto>($"id={id}");
        }

        public Task<HttpResponse<SoundOverviewDto>> GetOverviewAsync()
        {
            return _soundOverviewRequest.GetAsync<SoundOverviewDto>();
        }

        public Task<HttpResponse> PostAsync(SoundCreationDto payload)
        {
            return _soundRequest.PostAsync(payload);
        }
    }
}