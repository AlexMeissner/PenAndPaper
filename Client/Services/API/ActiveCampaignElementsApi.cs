using Client.Helper;
using DataTransfer;
using DataTransfer.Map;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IActiveMapApi
    {
        public Task<ApiResponse<ActiveMapDto>> GetAsync(int campaignId);
        public Task<ApiResponse> PutAsync(ActiveMapDto payload);
    }

    public class ActiveMapApi : IActiveMapApi
    {
        private readonly IEndPointProvider _endPointProvider;

        public ActiveMapApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<ActiveMapDto>> GetAsync(int campaignId)
        {
            string url = _endPointProvider.BaseURL + $"ActiveMap?campaignId={campaignId}";
            return url.GetAsync<ActiveMapDto>();
        }

        public Task<ApiResponse> PutAsync(ActiveMapDto payload)
        {
            string url = _endPointProvider.BaseURL + "ActiveMap";
            return url.PutAsync(payload);
        }
    }
}
