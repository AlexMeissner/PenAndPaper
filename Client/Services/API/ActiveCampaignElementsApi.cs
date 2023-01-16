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
        public Task<ApiResponse<ActiveMapDto>> GetAsync(int campaignId)
        {
            // TODO
            string url = $"https://localhost:7099/ActiveMap?campaignId={campaignId}";
            return url.GetAsync<ActiveMapDto>();
        }

        public Task<ApiResponse> PutAsync(ActiveMapDto payload)
        {
            // TODO
            string url = "https://localhost:7099/ActiveMap";
            return url.PutAsync(payload);
        }
    }
}
