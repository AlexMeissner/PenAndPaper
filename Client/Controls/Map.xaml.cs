﻿using Client.Services;
using Client.ViewModels;
using DataTransfer.Character;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class Map : UserControl
    {
        private readonly IControlProvider _controlProvider;

        public MapViewModel ViewModel => (MapViewModel)DataContext;

        public Map(IControlProvider controlProvider, IViewModelProvider viewModelProvider, ICampaignUpdates campaignUpdates)
        {
            DataContext = viewModelProvider.Get<MapViewModel>();

            _controlProvider = controlProvider;

            InitializeComponent();

            DiceRollerPresenter.Content = controlProvider.Get<DiceRoller>();

            campaignUpdates.DiceRolled += OnDiceRolled;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.UpdateMap();
            await ViewModel.UpdateTokens();
        }

        private async void OnDiceRolled(object? sender, EventArgs e)
        {
            await Dispatcher.Invoke(async () =>
            {
                var respose = await ViewModel.GetDiceRoll();

                respose.Match(
                    async success =>
                    {
                        var diceRoller = (DiceRoller)DiceRollerPresenter.Content;
                        await diceRoller.ViewModel.Show(success);
                        //await Dispatcher.InvokeAsync(new Action(async () => await ((DiceRoller)DiceRollerPresenter.Content).ViewModel.Show(success)));
                    });
            });
        }

        private void OnShowDice(object sender, MouseEventArgs e)
        {
            ViewModel.DiceVisibility = Visibility.Visible;
        }

        private async void OnHideDice(object sender, MouseEventArgs e)
        {
            const int timeToHide = 3000;
            await Task.Delay(timeToHide);
            ViewModel.DiceVisibility = Visibility.Collapsed;
        }

        private void OnOpenSettings(object sender, RoutedEventArgs e)
        {
            OpenPopupWindow("Einstellungen", _controlProvider.Get<SettingsControl>());
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
            if (e.Data.GetData(typeof(CharacterOverviewItem)) is CharacterOverviewItem character)
            {
                var position = e.GetPosition(this);
                await ViewModel.CreateToken(position, character.CharacterId);
            }
        }
    }
}