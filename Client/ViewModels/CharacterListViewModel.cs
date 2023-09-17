using Client.Commands;
using Client.Pages;
using Client.Services;
using Client.Services.API;
using DataTransfer.Character;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    [TransistentService]
    public class CharacterListViewModel : BaseViewModel
    {
        private readonly ICharacterApi _characterApi;
        private readonly ISessionData _sessionData;
        private readonly IPopupPage _popupPage;

        public ICommand OpenCharacterCreationCommand { get; }
        public ObservableCollection<CharacterOverviewItem> Items { get; init; } = new();

        public CharacterListViewModel(ICharacterApi characterApi, ISessionData sessionData, IPopupPage popupPage)
        {
            _characterApi = characterApi;
            _sessionData = sessionData;
            _popupPage = popupPage;

            OpenCharacterCreationCommand = new RelayCommand(OpenCharacterCreation);
        }

        public async Task Load()
        {
            var response = await _characterApi.GetOverviewAsync(_sessionData.CampaignId);
            response.Match(characterList => Items.ReplaceWith(characterList.Items));
        }

        private void OpenCharacterCreation()
        {
            _popupPage.Open<CharacterCreation>("Charaktererstellung");
        }
    }
}
