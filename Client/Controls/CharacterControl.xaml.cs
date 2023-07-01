using Client.Services;
using Client.Services.API;
using DataTransfer.Character;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Client.Controls
{
    public partial class CharacterControl : UserControl
    {
        private readonly ICharacterApi _characterApi;
        private readonly ISessionData _sessionData;

        public CharacterOverviewDto CharacterOverview { get; set; } = new() { Items = new ObservableCollection<CharacterOverviewItem>() };

        public CharacterControl(ICharacterApi characterApi, ISessionData sessionData)
        {
            _characterApi = characterApi;
            _sessionData = sessionData;

            InitializeComponent();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_sessionData.CampaignId is int campaignId)
            {
                var response = await _characterApi.GetOverviewAsync(campaignId);

                if (response.Error is null)
                {
                    CharacterOverview.Items.Clear();

                    foreach (var item in response.Data.Items)
                    {
                        CharacterOverview.Items.Add(item);
                    }
                }
            }
        }
    }
}
