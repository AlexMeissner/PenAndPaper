using Client.Helper;
using DataTransfer;
using DataTransfer.Dice;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface IRollApi
    {
        public Task<ApiResponse<DiceRollResultDto>> GetAsync(int campaignId);
        public Task<ApiResponse> PutAsync(RollDiceDto payload);
    }

    public class RollApi : IRollApi
    {
        Task<ApiResponse<DiceRollResultDto>> IRollApi.GetAsync(int campaignId)
        {
            // TODO
            string url = $"https://localhost:7099/Roll?campaignId={campaignId}";
            return url.GetAsync<DiceRollResultDto>();
        }

        public Task<ApiResponse> PutAsync(RollDiceDto payload)
        {
            // TODO
            string url = "https://localhost:7099/Roll";
            return url.PutAsync(payload);

        }
    }
}