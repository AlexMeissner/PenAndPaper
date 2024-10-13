using Client.Commands;
using Client.Services;
using Client.Services.API;
using Client.Windows;
using DataTransfer.Dice;
using DataTransfer.Map;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static Client.Services.ServiceExtension;

namespace Client.ViewModels
{
    public enum TokenType
    {
        Character,
        Monster
    }

    [TransistentService]
    public class MapViewModel : BaseViewModel
    {
        private readonly ISessionData _sessionData;
        private readonly IMapApi _mapApi;
        private readonly ITokenApi _tokenApi;
        private readonly IRollApi _rollApi;
        private readonly IActiveMapApi _activeMapApi;
        private readonly IUpdateNotifier _updateNotifier;

        private Point _initialMousePosition;
        private Point _initialMapOffset;
        private IMapItem? _selectedMapItem = null;

        public int? Id { get; private set; }
        public MapTransformation MapTransformation { get; set; } = new();
        public ObservableCollection<IMapItem> Items { get; private set; } = [];

        public BackgroundMapItem? Background => (BackgroundMapItem?)Items.FirstOrDefault(x => x is BackgroundMapItem);
        public GridMapItem? Grid => (GridMapItem?)Items.FirstOrDefault(x => x is GridMapItem);

        public Visibility DiceVisibility { get; set; } = Visibility.Collapsed;

        public ICommand RollD4Command { get; set; }
        public ICommand RollD6Command { get; set; }
        public ICommand RollD8Command { get; set; }
        public ICommand RollD10Command { get; set; }
        public ICommand RollD12Command { get; set; }
        public ICommand RollD20Command { get; set; }

        public MapViewModel(ISessionData sessionData, IMapApi mapApi, ITokenApi tokenApi, IRollApi rollApi, IActiveMapApi activeMapApi, IUpdateNotifier updateNotifier)
        {
            _sessionData = sessionData;
            _mapApi = mapApi;
            _tokenApi = tokenApi;
            _rollApi = rollApi;
            _activeMapApi = activeMapApi;
            _updateNotifier = updateNotifier;

            _updateNotifier.MapChanged += OnMapChanged;
            _updateNotifier.TokenAdded += OnTokenAdded;
            _updateNotifier.TokenMoved += OnTokenMoved;

            RollD4Command = new AsyncCommand(OnRollD4);
            RollD6Command = new AsyncCommand(OnRollD6);
            RollD8Command = new AsyncCommand(OnRollD8);
            RollD10Command = new AsyncCommand(OnRollD10);
            RollD12Command = new AsyncCommand(OnRollD12);
            RollD20Command = new AsyncCommand(OnRollD20);
        }

        public void UnsubscribeEventHandlers()
        {
            _updateNotifier.MapChanged -= OnMapChanged;
            _updateNotifier.TokenAdded -= OnTokenAdded;
        }

        public Task CreateToken(TokenType type, Point position, int id)
        {
            if (Id is int mapId)
            {
                if (Grid is not null)
                {
                    position.X -= position.X % Grid.Size;
                    position.Y -= position.Y % Grid.Size;
                }

                int? characterId = (type == TokenType.Character) ? id : null;
                int? monsterId = (type == TokenType.Monster) ? id : null;

                var payload = new TokenCreationDto(
                    CampaignId: _sessionData.CampaignId,
                    MapId: mapId,
                    CharacterId: characterId,
                    MonsterId: monsterId,
                    X: (int)position.X,
                    Y: (int)position.Y
                );

                return _tokenApi.PostAsync(payload);
            }

            return Task.CompletedTask;
        }

        public Task<HttpResponse<DiceRollResultDto>> GetDiceRoll()
        {
            return _rollApi.GetAsync(_sessionData.CampaignId);
        }

        private async void OnMapChanged(object? sender, EventArgs e)
        {
            MapTransformation.Reset();

            await UpdateMap();
            await UpdateTokens();
        }

        private async void OnTokenAdded(object? sender, EventArgs e)
        {
            await UpdateTokens();
        }

        private void OnTokenMoved(object? sender, TokenMovedEventArgs e)
        {
            var token = Items.FirstOrDefault(t => t is TokenMapItem item && item.Id == e.Id);

            if (token is null)
            {
                // ToDo: Log error
                return;
            }

            token.X = e.X;
            token.Y = e.Y;
        }

        public async Task UpdateMap()
        {
            if (Background is not null)
            {
                Items.Remove(Background);
            }
            if (Grid is not null)
            {
                Items.Remove(Grid);
            }

            var activeMapResponse = await _activeMapApi.GetAsync(_sessionData.CampaignId);

            Id = activeMapResponse.Match(
                success =>
                {
                    return success.MapId;
                },
                failure =>
                {
                    MessageBoxUtility.Show(failure);
                    return null;
                });

            if (Id is int mapId)
            {
                var mapResponse = await _mapApi.GetAsync(mapId);

                mapResponse.Match(
                    s =>
                    {
                        var background = new BackgroundMapItem(s.ImageData);

                        Items.Add(background);

                        if (s.Grid.IsActive)
                        {
                            const int lineThickness = 1; // ToDo
                            SolidColorBrush color = new(Colors.LightGray); // ToDo
                            var map = new GridMapItem(s.Grid.Size, background.Width, background.Height, lineThickness, color);
                            Items.Add(map);
                        }
                    },
                    f =>
                    {
                        MessageBoxUtility.Show(f);
                    });
            }
        }

        public async Task UpdateTokens()
        {
            var tokens = Items.Where(x => x is TokenMapItem).ToList();
            Items.RemoveAll(tokens);

            if (Id is int mapId)
            {
                var response = await _tokenApi.GetAsync(mapId);

                response.Match(
                    success =>
                    {
                        foreach (var item in success.Items)
                        {
                            var token = new TokenMapItem(item.X, item.Y, item.Id, item.UserId, item.Name, item.Image);
                            Items.Add(token);
                        }
                    },
                    failure =>
                    {
                        MessageBoxUtility.Show(failure);
                    });
            }
        }

        public async Task UpdateSelectedItemPosition()
        {
            if (_selectedMapItem is TokenMapItem token)
            {
                var payload = new TokenUpdateDto(_sessionData.CampaignId, token.Id, token.X, token.Y);

                await _tokenApi.PutAsync(payload);

                _selectedMapItem = null;
            }
        }

        public void Zoom(Point position, int delta)
        {
            MapTransformation.Zoom(position, delta);
        }

        public void SetSelectedItem(IMapItem mapItem)
        {
            _selectedMapItem = mapItem;
        }

        public void SetInitialMousePosition(Point position)
        {
            _initialMousePosition = position;
            _initialMapOffset.X = MapTransformation.X;
            _initialMapOffset.Y = MapTransformation.Y;
        }

        public void MoveMap(Point position)
        {
            var delta = Point.Subtract(position, _initialMousePosition) / MapTransformation.Scaling.Matrix.M11;
            var position2 = _initialMapOffset + delta;
            MapTransformation.Move(position2);
        }

        public void MoveMapItem(Point position)
        {
            if (_selectedMapItem is TokenMapItem token)
            {
                position = MapTransformation.Scaling.Inverse.Transform(position);

                position.X -= MapTransformation.X;
                position.Y -= MapTransformation.Y;

                if (Grid is not null)
                {
                    position.X -= position.X % Grid.Size;
                    position.Y -= position.Y % Grid.Size;
                }

                token.X = (int)position.X;
                token.Y = (int)position.Y;
            }
        }

        private async Task RollDice(Dice dice)
        {
            DiceVisibility = Visibility.Collapsed;

            var payload = new RollDiceDto(
                CampaignId: _sessionData.CampaignId,
                PlayerId: _sessionData.UserId,
                Dice: dice
            );

            await _rollApi.PutAsync(payload);
        }

        private Task OnRollD4()
        {
            return RollDice(Dice.D4);
        }

        private Task OnRollD6()
        {
            return RollDice(Dice.D6);
        }

        private Task OnRollD8()
        {
            return RollDice(Dice.D8);
        }

        private Task OnRollD10()
        {
            return RollDice(Dice.D10);
        }

        private Task OnRollD12()
        {
            return RollDice(Dice.D12);
        }

        private Task OnRollD20()
        {
            return RollDice(Dice.D20);
        }
    }
}
