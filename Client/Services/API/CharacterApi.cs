using Client.Helper;
using DataTransfer;
using DataTransfer.Character;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ICharacterApi
    {
        public Task<ApiResponse<CharacterOverviewDto>> GetOverviewAsync(int campaignId);
    }

    public class CharacterApi : ICharacterApi
    {
        private readonly IEndPointProvider _endPointProvider;

        public CharacterApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<CharacterOverviewDto>> GetOverviewAsync(int campaignId)
        {
            string url = _endPointProvider.BaseURL + $"CharacterOverview?campaignId={campaignId}";
            return url.GetAsync<CharacterOverviewDto>();
        }
    }
}
