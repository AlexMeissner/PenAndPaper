using Client.Services;
using Client.Services.API;
using DataTransfer.Character;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class CharacterListViewModel : BaseViewModel
    {
        private readonly ICharacterApi _characterApi;
        private readonly ISessionData _sessionData;

        public ObservableCollection<CharacterOverviewItem> Items { get; init; } = new();

        public CharacterListViewModel(ICharacterApi characterApi, ISessionData sessionData)
        {
            _characterApi = characterApi;
            _sessionData = sessionData;
        }

        public async Task Load()
        {
            var response = await _characterApi.GetOverviewAsync(_sessionData.CampaignId);
            response.Match(characterList => Items.ReplaceWith(characterList.Items));
        }
    }
}
