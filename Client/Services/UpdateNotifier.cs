using DataTransfer.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public class TokenMovedEventArgs(int id, int x, int y) : EventArgs
    {
        public int Id => id;
        public int X => x;
        public int Y => y;
    }

    public interface IUpdateNotifier
    {
        event EventHandler MapChanged;
        event EventHandler MapCollectionChanged;
        event EventHandler TokenAdded;
        event EventHandler<TokenMovedEventArgs> TokenMoved;
        event EventHandler DiceRolled;
        event EventHandler AmbientSoundChanged;
        event EventHandler SoundEffectChanged;
        event EventHandler CharacterChanged;

        List<Delegate> GetSubscribers();
        Task SetCampaignAsync(int campaignId);
    }

    [SingletonService]
    public class UpdateNotifier(IEndPointProvider endPointProvider) : IUpdateNotifier
    {
        public event EventHandler? MapChanged;
        public event EventHandler? MapCollectionChanged;
        public event EventHandler? TokenAdded;
        public event EventHandler<TokenMovedEventArgs>? TokenMoved;
        public event EventHandler? DiceRolled;
        public event EventHandler? AmbientSoundChanged;
        public event EventHandler? SoundEffectChanged;
        public event EventHandler? CharacterChanged;

        private readonly ClientWebSocket _webSocket = new();

        public async Task Close()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket connection closed.", CancellationToken.None);
            _webSocket.Dispose();
        }

        public async Task Connect()
        {
            try
            {
                var uri = new Uri(endPointProvider.WebSocketBaseURL + "WebSocket");
                await _webSocket.ConnectAsync(uri, CancellationToken.None);

                await ReceiveMessageAsync(_webSocket);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }

        public List<Delegate> GetSubscribers()
        {
            var eventHandlers = new List<Delegate?>([
                MapChanged,
                MapCollectionChanged,
                TokenAdded,
                TokenMoved,
                DiceRolled,
                AmbientSoundChanged,
                SoundEffectChanged,
                CharacterChanged]);

            var subscriber = new List<Delegate>();

            foreach (var eventHandler in eventHandlers)
            {
                if (eventHandler is not null)
                {
                    foreach (Delegate handler in eventHandler.GetInvocationList())
                    {
                        subscriber.Add(handler);
                    }
                }
            }

            return subscriber;
        }

        public async Task SetCampaignAsync(int campaignId)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                var message = JsonSerializer.Serialize(campaignId);
                var buffer = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private void InvokeEvent(UpdateEntity entity)
        {
            switch (entity)
            {
                case UpdateEntity.Map:
                    MapChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case UpdateEntity.MapCollection:
                    MapCollectionChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case UpdateEntity.TokenAdded:
                    TokenAdded?.Invoke(this, EventArgs.Empty);
                    break;
                case UpdateEntity.Dice:
                    DiceRolled?.Invoke(this, EventArgs.Empty);
                    break;
                case UpdateEntity.AmbientSound:
                    AmbientSoundChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case UpdateEntity.SoundEffect:
                    SoundEffectChanged?.Invoke(this, EventArgs.Empty);
                    break;
                case UpdateEntity.Character:
                    CharacterChanged?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private async Task ReceiveMessageAsync(ClientWebSocket webSocket)
        {
            while (_webSocket.State == WebSocketState.Open)
            {
                const uint bufferSize = 64;
                byte[] buffer = new byte[bufferSize];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    var update = JsonSerializer.Deserialize<WebSocketUpdate>(message);

                    if (update is null)
                    {
                        continue;
                    }

                    if (update.Entity is JsonElement jsonElement)
                    {
                        switch (update.Type)
                        {
                            case nameof(TokenMovedDto):
                                var token = JsonSerializer.Deserialize<TokenMovedDto>(jsonElement)!;
                                TokenMoved?.Invoke(this, new TokenMovedEventArgs(token.Id, token.X, token.Y));
                                break;

                            case nameof(UpdateEntity):
                                var updateEntity = JsonSerializer.Deserialize<UpdateEntity>(jsonElement);
                                InvokeEvent(updateEntity);
                                break;

                            default:
                                // ToDo: Log error
                                break;
                        }
                    }
                }
            }
        }
    }
}
