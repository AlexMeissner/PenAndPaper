using Client.Services;
using Client.Services.API;
using DataTransfer.Character;
using DataTransfer.Dice;
using DataTransfer.Map;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Client.Controls
{
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

            DiceRollerPresenter.Content = serviceProvider.GetRequiredService<DiceRollerControl>();
        }

        private async void OnMapChanged(object? sender, EventArgs e)
        {
            await UpdateMap();
            await Dispatcher.InvokeAsync(new Action(async () => await UpdateTokens()));
        }

        private async void OnDiceRolled(object? sender, EventArgs e)
        {
            if (_sessionData.CampaignId is int campaignId)
            {
                var rollDiceResult = await _rollApi.GetAsync(campaignId);

                if (rollDiceResult.Error is null)
                {
                    await Dispatcher.InvokeAsync(new Action(async () => await ((DiceRollerControl)DiceRollerPresenter.Content).Show(rollDiceResult.Data)));
                }
                else
                {
                    MessageBox.Show(rollDiceResult.Error.Message);
                }
            }
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
            if (_sessionData.CampaignId is int campaignId && _sessionData.UserId is int playerId)
            {
                DicePanel.Visibility = Visibility.Collapsed;

                var payload = new RollDiceDto()
                {
                    CampaignId = campaignId,
                    PlayerId = playerId,
                    Dice = dice
                };

                await _rollApi.PutAsync(payload);
            }
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

                if (activeMapResponse.Error is not null)
                {
                    MessageBox.Show(activeMapResponse.Error.Message);
                    return;
                }

                if (activeMapResponse.Data.MapId > 0)
                {
                    var mapResponse = await _mapApi.GetAsync(activeMapResponse.Data.MapId);

                    if (mapResponse.Error is not null)
                    {
                        MessageBox.Show(mapResponse.Error.Message);
                        return;
                    }

                    Map.Id = activeMapResponse.Data.MapId;
                    Map.ImageData = mapResponse.Data.ImageData;
                    Map.Grid.IsActive = mapResponse.Data.Grid.IsActive;
                    Map.Grid.Size = mapResponse.Data.Grid.Size;
                }

                //ZoomableCanvas.Reset();
            }
        }

        private async Task UpdateTokens()
        {
            var response = await _tokenApi.GetAsync(Map.Id);

            if (response.Error is null)
            {
                Tokens.Items.Clear();

                foreach (var item in response.Data.Items)
                {
                    Tokens.Items.Add(item);
                }
            }
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