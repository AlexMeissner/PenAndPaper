using Client.Services;
using Client.Services.API;
using Client.View;
using DataTransfer.Character;
using DataTransfer.Dice;
using DataTransfer.Map;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class MapControl : UserControl
    {
        private readonly ISessionData _sessionData;
        private readonly IMapApi _mapApi;
        private readonly ITokenApi _tokenApi;
        private readonly IRollApi _rollApi;
        private readonly IActiveMapApi _activeMapApi;
        private readonly IServiceProvider _serviceProvider;

        public MapDto Map { get; set; } = new();
        public TokensDto Tokens { get; set; } = new();

        public MapControl(IServiceProvider serviceProvider, ISessionData sessionData, IMapApi mapApi, ITokenApi tokenApi, IRollApi rollApi, IActiveMapApi activeMapApi, ICampaignUpdates campaignUpdates)
        {
            _sessionData = sessionData;
            _mapApi = mapApi;
            _tokenApi = tokenApi;
            _rollApi = rollApi;
            _activeMapApi = activeMapApi;
            _serviceProvider = serviceProvider;

            campaignUpdates.MapChanged += OnMapChanged;
            campaignUpdates.DiceRolled += OnDiceRolled;
            campaignUpdates.TokenChanged += OnTokenChanged;
            InitializeComponent();

            DiceRollerPresenter.Content = serviceProvider.GetRequiredService<DiceRoller>();
        }

        private async void OnMapChanged(object? sender, EventArgs e)
        {
            await UpdateMap();
            await Dispatcher.InvokeAsync(new Action(async () => await UpdateTokens()));
        }

        private async void OnDiceRolled(object? sender, EventArgs e)
        {
            var result = await _rollApi.GetAsync(_sessionData.CampaignId);

            result.Match(
                async success =>
                {
                    await Dispatcher.InvokeAsync(new Action(async () => await ((DiceRoller)DiceRollerPresenter.Content).ViewModel.Show(success)));
                },
                failure =>
                {
                    MessageBoxUtility.Show(failure);
                });
        }

        private async void OnTokenChanged(object? sender, EventArgs e)
        {
            await Dispatcher.InvokeAsync(new Action(async () => await UpdateTokens()));
        }

        private void OnShowDice(object sender, MouseEventArgs e)
        {
            DicePanel.Visibility = Visibility.Visible;
        }

        private async void OnHideDice(object sender, MouseEventArgs e)
        {
            const int timeToHide = 3000;
            await Task.Delay(timeToHide);
            DicePanel.Visibility = Visibility.Collapsed;
        }

        private async Task RollDice(Dice dice)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var payload = new RollDiceDto()
            {
                CampaignId = _sessionData.CampaignId,
                PlayerId = _sessionData.UserId,
                Dice = dice
            };
            
            await _rollApi.PutAsync(payload);
        }

        private async void OnRollD4(object sender, RoutedEventArgs e)
        {
            await RollDice(Dice.D4);
        }

        private async void OnRollD6(object sender, RoutedEventArgs e)
        {
            await RollDice(Dice.D6);
        }

        private async void OnRollD8(object sender, RoutedEventArgs e)
        {
            await RollDice(Dice.D8);
        }

        private async void OnRollD10(object sender, RoutedEventArgs e)
        {
            await RollDice(Dice.D10);
        }

        private async void OnRollD12(object sender, RoutedEventArgs e)
        {
            await RollDice(Dice.D12);
        }

        private async void OnRollD20(object sender, RoutedEventArgs e)
        {
            await RollDice(Dice.D20);
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await UpdateMap();
            await Dispatcher.InvokeAsync(new Action(async () => await UpdateTokens()));
        }

        private async Task UpdateMap()
        {
            if (_sessionData.CampaignId is int campaignId)
            {
                var activeMapResponse = await _activeMapApi.GetAsync(campaignId);

                activeMapResponse.Match(
                    async success =>
                    {
                        var mapResponse = await _mapApi.GetAsync(success.MapId);

                        mapResponse.Match(
                            s =>
                            {
                                Map.Id = success.MapId;
                                Map.ImageData = s.ImageData;
                                Map.Grid.IsActive = s.Grid.IsActive;
                                Map.Grid.Size = s.Grid.Size;

                                //ZoomableCanvas.Reset();
                            },
                            f =>
                            {
                                MessageBoxUtility.Show(f);
                            });
                    },
                    failure =>
                    {
                        MessageBoxUtility.Show(failure);
                    });
            }
        }

        private async Task UpdateTokens()
        {
            var response = await _tokenApi.GetAsync(Map.Id);

            response.Match(
                success =>
                {
                    Tokens.Items.Clear();

                    foreach (var item in success.Items)
                    {
                        Tokens.Items.Add(item);
                    }
                });
        }

        private void OnOpenSettings(object sender, RoutedEventArgs e)
        {
            OpenPopupWindow("Einstellungen", _serviceProvider.GetRequiredService<SettingsControl>());
        }

        private void OnClosePopupWindow(object sender, RoutedEventArgs e)
        {
            PopupWindow.Visibility = Visibility.Collapsed;
        }

        private void OpenPopupWindow(string title, UserControl control)
        {
            PopupWindowTitle.Text = title;
            PopupWindowContentPresenter.Content = control;
            PopupWindow.Visibility = Visibility.Visible;
        }

        private async void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(CharacterOverviewItem)) is CharacterOverviewItem character &&
                _sessionData.CampaignId is int campaignId)
            {
                var position = e.GetPosition(this);

                if (Map.Grid.IsActive)
                {
                    position.X -= position.X % Map.Grid.Size;
                    position.Y -= position.Y % Map.Grid.Size;
                }

                TokenCreationDto payload = new()
                {
                    CampaignId = campaignId,
                    MapId = Map.Id,
                    CharacterId = character.CharacterId,
                    X = (int)position.X,
                    Y = (int)position.Y
                };

                await _tokenApi.PostAsync(payload);
            }
        }
    }
}