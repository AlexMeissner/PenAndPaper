using Client.Services;
using Client.Services.API;
using DataTransfer.Character;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

                response.Match(success =>
                    {
                        CharacterOverview.Items.Clear();

                        foreach (var item in success.Items)
                        {
                            CharacterOverview.Items.Add(item);
                        }
                    });
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                sender is ListView listView &&
                listView.SelectedItem is CharacterOverviewItem character)
            {
                DragDrop.DoDragDrop(this, character, DragDropEffects.Copy);
            }
        }
    }
}
