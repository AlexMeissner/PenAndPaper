using DataTransfer.Dice;
using System.Threading.Tasks;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface IRollApi
    {
        public Task<HttpResponse<DiceRollResultDto>> GetAsync(int campaignId);
        public Task<HttpResponse> PutAsync(RollDiceDto payload);
    }

    [TransistentService]
    public class RollApi(IEndPointProvider endPointProvider,  ITokenProvider tokenProvider) : IRollApi
    {
        private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Roll", tokenProvider);

        public Task<HttpResponse<DiceRollResultDto>> GetAsync(int campaignId)
        {
            return _request.GetAsync<DiceRollResultDto>($"campaignId={campaignId}");
        }

        public Task<HttpResponse> PutAsync(RollDiceDto payload)
        {
            return _request.PutAsync(payload);
        }
    }
}
