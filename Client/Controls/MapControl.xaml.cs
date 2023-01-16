using Client.Services;
using Client.Services.API;
using DataTransfer.Dice;
using DataTransfer.Map;
using System;
using System.Collections.Generic;
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
        private readonly IActiveMapApi _activeMapApi;

        public MapDto Map { get; set; } = new();

        public MapControl(ISessionData sessionData, IMapApi mapApi, IActiveMapApi activeMapApi, ICampaignUpdates campaignUpdates)
        {
            _sessionData = sessionData;
            _mapApi = mapApi;
            _activeMapApi = activeMapApi;

            campaignUpdates.MapChanged += OnMapChanged;

            InitializeComponent();
        }

        private async void OnMapChanged(object? sender, EventArgs e)
        {
            await Update();
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

        private void OnRollD4(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D4",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD6(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D6",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD8(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D8",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD10(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D10",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD12(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D12",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
        }

        private void OnRollD20(object sender, RoutedEventArgs e)
        {
            DicePanel.Visibility = Visibility.Collapsed;

            var random = new Random();

            var diceRollDto = new DiceRollDto()
            {
                Name = "Roll D20",
                Succeeded = new List<bool>
                {
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                    random.Next(2) == 1,
                }
            };

            DiceRoller.Show(diceRollDto);
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
            }
        }
    }
}