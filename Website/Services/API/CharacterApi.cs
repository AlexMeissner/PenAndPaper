using DataTransfer.Character;
using System.Threading.Tasks;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface ICharacterApi
    {
        public Task<HttpResponse<CharacterCreationDto>> GetAsync(int characterId);
        public Task<HttpResponse<CharacterOverviewDto>> GetOverviewAsync(int campaignId);
        public Task<HttpResponse> PostAsync(CharacterCreationDto payload);
    }

    [TransistentService]
    public class CharacterApi : ICharacterApi
    {
        private readonly HttpRequest _characterRequest;
        private readonly HttpRequest _overviewRequest;

        public CharacterApi(IEndPointProvider endPointProvider, IIdentityProvider identityProvider)
        {
            _characterRequest = new(endPointProvider.BaseURL + "Character", identityProvider);
            _overviewRequest = new(endPointProvider.BaseURL + "CharacterOverview", identityProvider);
        }

        public Task<HttpResponse<CharacterCreationDto>> GetAsync(int characterId)
        {
            return _characterRequest.GetAsync<CharacterCreationDto>($"characterId={characterId}");
        }

        public Task<HttpResponse<CharacterOverviewDto>> GetOverviewAsync(int campaignId)
        {
            return _overviewRequest.GetAsync<CharacterOverviewDto>($"campaignId={campaignId}");
        }

        public Task<HttpResponse> PostAsync(CharacterCreationDto payload)
        {
            return _characterRequest.PostAsync(payload);
        }
    }
}
