using Client.Helper;
using DataTransfer;
using DataTransfer.Sound;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ISoundApi
    {
        public Task<ApiResponse<SoundOverviewDto>> GetOverviewAsync();
        public Task<ApiResponse<SoundDto>> GetAsync(int id);
        public Task<ApiResponse<SoundDataDto>> GetDataAsync(int id);
        public Task<ApiResponse> PostAsync(SoundCreationDto payload);
    }

    public class SoundApi : ISoundApi
    {
        private readonly ICache _Cache;
        private readonly IEndPointProvider EndPointProvider;

        public SoundApi(ICache cache, IEndPointProvider endPointProvider)
        {
            _Cache = cache;
            EndPointProvider = endPointProvider;
        }

        public Task<ApiResponse<SoundDto>> GetAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<SoundDataDto>> GetDataAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApiResponse<SoundOverviewDto>> GetOverviewAsync()
        {
            string url = EndPointProvider.BaseURL + $"SoundOverview";
            return url.GetAsync<SoundOverviewDto>();
        }

        public Task<ApiResponse> PostAsync(SoundCreationDto payload)
        {
            string url = EndPointProvider.BaseURL + $"Sound";
            return url.PostAsync(payload);
        }
    }
}