using Client.Helper;
using DataTransfer;
using DataTransfer.Sound;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IActiveSoundApi
    {
        public Task<ApiResponse<ActiveSoundDto>> GetAsync(int campaignId);
        public Task<ApiResponse> PutAsync(ActiveSoundDto payload);
    }

    public class ActiveSoundApi : IActiveSoundApi
    {
        private readonly IEndPointProvider _endPointProvider;

        public ActiveSoundApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<ActiveSoundDto>> GetAsync(int campaignId)
        {
            string url = _endPointProvider.BaseURL + $"ActiveSound?campaignId={campaignId}";
            return url.GetAsync<ActiveSoundDto>();
        }

        public Task<ApiResponse> PutAsync(ActiveSoundDto payload)
        {
            string url = _endPointProvider.BaseURL + "ActiveSound";
            return url.PutAsync(payload);
        }
    }
}
