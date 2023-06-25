using Client.Services;
using Client.Services.API;
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
        private readonly IRollApi _rollApi;
        private readonly IActiveMapApi _activeMapApi;

        public MapDto Map { get; set; } = new();

        public MapControl(IServiceProvider serviceProvider, ISessionData sessionData, IMapApi mapApi, IRollApi rollApi, IActiveMapApi activeMapApi, ICampaignUpdates campaignUpdates)
        {
            _sessionData = sessionData;
            _mapApi = mapApi;
            _rollApi = rollApi;
            _activeMapApi = activeMapApi;

            campaignUpdates.MapChanged += OnMapChanged;
            campaignUpdates.DiceRolled += OnDiceRolled;
            InitializeComponent();

            DiceRollerPresenter.Content = serviceProvider.GetRequiredService<DiceRollerControl>();
        }

        private async void OnMapChanged(object? sender, EventArgs e)
        {
            await Update();
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
            await Update();
        }

        private async Task Update()
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

                    Map.ImageData = mapResponse.Data.ImageData;
                    Map.Grid.IsActive = mapResponse.Data.Grid.IsActive;
                    Map.Grid.Size = mapResponse.Data.Grid.Size;
                }

                ZoomableCanvas.Reset();
            }
        }
    }
}