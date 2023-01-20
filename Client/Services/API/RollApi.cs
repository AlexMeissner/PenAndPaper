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
        private readonly IEndPointProvider _endPointProvider;

        public RollApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        Task<ApiResponse<DiceRollResultDto>> IRollApi.GetAsync(int campaignId)
        {
            string url = _endPointProvider.BaseURL + $"Roll?campaignId={campaignId}";
            return url.GetAsync<DiceRollResultDto>();
        }

        public Task<ApiResponse> PutAsync(RollDiceDto payload)
        {
            string url = _endPointProvider.BaseURL + "Roll";
            return url.PutAsync(payload);
        }
    }
}