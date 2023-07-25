using DataTransfer.Dice;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface IRollApi
    {
        public Task<HttpResponse<DiceRollResultDto>> GetAsync(int campaignId);
        public Task<HttpResponse> PutAsync(RollDiceDto payload);
    }

    [TransistentService]
    public class RollApi : IRollApi
    {
        private readonly HttpRequest _request;

        public RollApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "Roll");
        }

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