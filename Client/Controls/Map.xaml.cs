using Client.Services;
using Client.ViewModels;
using DataTransfer.Character;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Client.Services.ServiceExtension;

namespace Client.Controls
{
    [TransistentService]
    public partial class Map : UserControl
    {
        public MapViewModel ViewModel => (MapViewModel)DataContext;

        public Map(IControlProvider controlProvider, IViewModelProvider viewModelProvider, IUpdateNotifier campaignUpdates)
        {
            DataContext = viewModelProvider.Get<MapViewModel>();

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
                    },
                    failure => { });
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

        private async void OnDrop(object sender, DragEventArgs e)
        {
            var position = e.GetPosition(this);

            if (e.Data.GetData(typeof(CharacterOverviewItem)) is CharacterOverviewItem character)
            {
                await ViewModel.CreateToken(TokenType.Character, position, character.CharacterId);
            }
            else if (e.Data.GetData(typeof(BaseMonsterData)) is BaseMonsterData monster)
            {
                await ViewModel.CreateToken(TokenType.Monster, position, monster.Id);
            }
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mousePostion = e.GetPosition(this);
            ViewModel.Zoom(mousePostion, e.Delta);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(this);
            ViewModel.SetInitialMousePosition(mousePosition);

            var hitTestResult = VisualTreeHelper.HitTest(this, mousePosition);

            if (hitTestResult.VisualHit is FrameworkElement element &&
                element.DataContext is TokenMapItem mapItem)
            {
                ViewModel.SetSelectedItem(mapItem);
            }
        }

        private async void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            await ViewModel.UpdateSelectedItemPosition();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(this);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ViewModel.MoveMapItem(mousePosition);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                ViewModel.MoveMap(mousePosition);
            }
        }
    }
}
