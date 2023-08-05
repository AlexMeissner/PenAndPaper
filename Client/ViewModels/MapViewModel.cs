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
    [TransistentService]
    public class MapViewModel : BaseViewModel
    {
        private readonly ISessionData _sessionData;
        private readonly IMapApi _mapApi;
        private readonly ITokenApi _tokenApi;
        private readonly IRollApi _rollApi;
        private readonly IActiveMapApi _activeMapApi;

        public int Id { get; private set; }
        public ObservableCollection<IMapItem> Items { get; private set; } = new();

        public BackgroundMapItem? Background => (BackgroundMapItem?)Items.FirstOrDefault(x => x is BackgroundMapItem);
        public GridMapItem? Grid => (GridMapItem?)Items.FirstOrDefault(x => x is GridMapItem);

        public Visibility DiceVisibility { get; set; } = Visibility.Collapsed;

        public ICommand RollD4Command { get; set; }
        public ICommand RollD6Command { get; set; }
        public ICommand RollD8Command { get; set; }
        public ICommand RollD10Command { get; set; }
        public ICommand RollD12Command { get; set; }
        public ICommand RollD20Command { get; set; }

        public MapViewModel(ISessionData sessionData, IMapApi mapApi, ITokenApi tokenApi, IRollApi rollApi, IActiveMapApi activeMapApi, ICampaignUpdates campaignUpdates)
        {
            _sessionData = sessionData;
            _mapApi = mapApi;
            _tokenApi = tokenApi;
            _rollApi = rollApi;
            _activeMapApi = activeMapApi;

            campaignUpdates.MapChanged += OnMapChanged;
            campaignUpdates.TokenChanged += OnTokenChanged;

            RollD4Command = new AsyncCommand(OnRollD4);
            RollD6Command = new AsyncCommand(OnRollD6);
            RollD8Command = new AsyncCommand(OnRollD8);
            RollD10Command = new AsyncCommand(OnRollD10);
            RollD12Command = new AsyncCommand(OnRollD12);
            RollD20Command = new AsyncCommand(OnRollD20);
        }

        public Task CreateToken(Point position, int characterId)
        {
            if (Grid is not null)
            {
                position.X -= position.X % Grid.Size;
                position.Y -= position.Y % Grid.Size;
            }

            TokenCreationDto payload = new()
            {
                CampaignId = _sessionData.CampaignId,
                MapId = Id,
                CharacterId = characterId,
                X = (int)position.X,
                Y = (int)position.Y
            };

            return _tokenApi.PostAsync(payload);
        }

        public Task<HttpResponse<DiceRollResultDto>> GetDiceRoll()
        {
            return _rollApi.GetAsync(_sessionData.CampaignId);
        }

        private async void OnMapChanged(object? sender, EventArgs e)
        {
            await UpdateMap();
            await UpdateTokens();
        }

        private async void OnTokenChanged(object? sender, EventArgs e)
        {
            await UpdateTokens();
        }

        public async Task UpdateMap()
        {
            var activeMapResponse = await _activeMapApi.GetAsync(_sessionData.CampaignId);

            activeMapResponse.Match(
                async success =>
                {
                    var mapResponse = await _mapApi.GetAsync(success.MapId);

                    await Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        mapResponse.Match(
                        s =>
                        {
                            if (Background is not null)
                            {
                                Items.Remove(Background);
                            }
                            if (Grid is not null)
                            {
                                Items.Remove(Grid);
                            }

                            Id = success.MapId;

                            var background = new BackgroundMapItem(s.ImageData);

                            Items.Add(background);

                            if (s.Grid.IsActive)
                            {
                                const int lineThickness = 1; // ToDo
                                SolidColorBrush color = new(Colors.Red); // ToDo
                                var map = new GridMapItem(s.Grid.Size, background.Width, background.Height, lineThickness, color);
                                Items.Add(map);
                            }

                            //ZoomableCanvas.Reset();
                        });
                    });
                },
                failure =>
                {
                    MessageBoxUtility.Show(failure);
                });
        }


        public async Task UpdateTokens()
        {
            var response = await _tokenApi.GetAsync(Id);


            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                response.Match(success =>
                {
                    var tokens = Items.Where(x => x is TokenMapItem).ToList();
                    Items.RemoveAll(tokens);

                    foreach (var item in success.Items)
                    {
                        var token = new TokenMapItem(item.X, item.Y, item.Id, item.UserId, item.Name, item.Image);
                        Items.Add(token);
                    }
                });
            });
        }

        private async Task RollDice(Dice dice)
        {
            DiceVisibility = Visibility.Collapsed;

            var payload = new RollDiceDto()
            {
                CampaignId = _sessionData.CampaignId,
                PlayerId = _sessionData.UserId,
                Dice = dice
            };

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
