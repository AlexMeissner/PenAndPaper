using Client.Commands;
using Client.Pages;
using Client.Services;
using Client.Services.API;
using DataTransfer.Character;
using System;
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
        private readonly IUpdateNotifier _updateNotifier;

        public string Filter { get; set; } = string.Empty;
        public ICommand OpenCharacterCreationCommand { get; }
        public ObservableCollection<CharacterOverviewItem> Items { get; init; } = [];

        public CharacterListViewModel(ICharacterApi characterApi, ISessionData sessionData, IPopupPage popupPage, IUpdateNotifier updateNotifier)
        {
            _characterApi = characterApi;
            _sessionData = sessionData;
            _popupPage = popupPage;
            _updateNotifier = updateNotifier;

            OpenCharacterCreationCommand = new RelayCommand(OpenCharacterCreation);
        }

        public async Task Load()
        {
            var response = await _characterApi.GetOverviewAsync(_sessionData.CampaignId);
            response.Match(characterList => Items.ReplaceWith(characterList.Items));

            _updateNotifier.CharacterChanged += OnCharacterChanged;
        }

        public bool OnFilter(object item)
        {
            if (string.IsNullOrEmpty(Filter))
            {
                return true;
            }

            if (item is CharacterOverviewItem character)
            {
                return character.CharacterName.Contains(Filter, StringComparison.CurrentCultureIgnoreCase);
            }

            return false;
        }

        public void UnsubscribeEventHandlers()
        {
            _updateNotifier.CharacterChanged -= OnCharacterChanged;
        }

        private async void OnCharacterChanged(object? sender, EventArgs e)
        {
            await Load();
        }

        private void OpenCharacterCreation()
        {
            _popupPage.Open<CharacterCreation>("Charaktererstellung");
        }
    }
}
