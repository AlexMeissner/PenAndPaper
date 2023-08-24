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
        private readonly IControlProvider _controlProvider;

        private readonly MatrixTransform Transformation = new();
        private readonly float ZoomFactor = 1.1f;
        private Point InitialMousePosition;
        private IMapItem? _selectedMapItem = null;

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

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scaleFactor = (e.Delta < 0) ? 1.0f / ZoomFactor : ZoomFactor;

            var mousePostion = e.GetPosition(this);

            var scaleMatrix = Transformation.Matrix;
            scaleMatrix.ScaleAt(scaleFactor, scaleFactor, mousePostion.X, mousePostion.Y);
            Transformation.Matrix = scaleMatrix;

            foreach (var child in ViewModel.Items)
            {
                double sx = child.X * scaleFactor;
                double sy = child.Y * scaleFactor;

                child.X = (int)sx;
                child.Y = (int)sy;

                child.Transformation = Transformation;
            }
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(this);

            var hitTestResult = VisualTreeHelper.HitTest(this, mousePosition);

            if (hitTestResult.VisualHit is FrameworkElement element &&
                element.DataContext is TokenMapItem mapItem)
            {
                _selectedMapItem = mapItem;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                InitialMousePosition = Transformation.Inverse.Transform(e.GetPosition(this));
            }
        }

        private async void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_selectedMapItem is not null)
            {
                var mousePosition = e.GetPosition(this);

                if (ViewModel.Grid is not null)
                {
                    mousePosition.X -= mousePosition.X % ViewModel.Grid.Size;
                    mousePosition.Y -= mousePosition.Y % ViewModel.Grid.Size;
                }

                await ViewModel.MoveMapItem(_selectedMapItem, (int)mousePosition.X, (int)mousePosition.Y);
                _selectedMapItem = null;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedMapItem is TokenMapItem token)
            {
                var mousePosition = e.GetPosition(this);

                if (ViewModel.Grid is not null)
                {
                    mousePosition.X -= mousePosition.X % ViewModel.Grid.Size;
                    mousePosition.Y -= mousePosition.Y % ViewModel.Grid.Size;
                }

                token.X = (int)mousePosition.X;
                token.Y = (int)mousePosition.Y;
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                var mousePosition = Transformation.Inverse.Transform(e.GetPosition(this));
                var delta = Point.Subtract(mousePosition, InitialMousePosition);
                var translate = new TranslateTransform(delta.X, delta.Y);
                Transformation.Matrix = translate.Value * Transformation.Matrix;

                foreach (var child in ViewModel.Items)
                {
                    child.Transformation = Transformation;
                }
            }
        }
    }
}
