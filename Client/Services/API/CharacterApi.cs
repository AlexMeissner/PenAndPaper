using DataTransfer.Character;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface ICharacterApi
    {
        public Task<HttpResponse<CharacterOverviewDto>> GetOverviewAsync(int campaignId);
    }

    [TransistentService]
    public class CharacterApi : ICharacterApi
    {
        private readonly HttpRequest _request;

        public CharacterApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "CharacterOverview");
        }

        public Task<HttpResponse<CharacterOverviewDto>> GetOverviewAsync(int campaignId)
        {
            return _request.GetAsync<CharacterOverviewDto>($"campaignId={campaignId}");
        }
    }
}
