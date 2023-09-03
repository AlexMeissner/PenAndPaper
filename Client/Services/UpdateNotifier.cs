using DataTransfer.WebSocket;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IUpdateNotifier
    {
        event EventHandler MapChanged;
        event EventHandler MapCollectionChanged;
        event EventHandler TokenChanged;
        event EventHandler DiceRolled;
        event EventHandler AmbientSoundChanged;
        event EventHandler SoundEffectChanged;

        Task SetCampaignAsync(int campaignId);
    }

    [SingletonService]
    public class UpdateNotifier : IUpdateNotifier
    {
        public event EventHandler? MapChanged;
        public event EventHandler? MapCollectionChanged;
        public event EventHandler? TokenChanged;
        public event EventHandler? DiceRolled;
        public event EventHandler? AmbientSoundChanged;
        public event EventHandler? SoundEffectChanged;

        private readonly ClientWebSocket _webSocket = new();
        private readonly IEndPointProvider _endPointProvider;

        public UpdateNotifier(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public async Task Close()
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "WebSocket connection closed.", CancellationToken.None);
            _webSocket.Dispose();
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

        public async Task Connect()
        {
            try
            {
                var uri = new Uri(_endPointProvider.WebSocketBaseURL + "WebSocket");
                await _webSocket.ConnectAsync(uri, CancellationToken.None);

                await ReceiveMessageAsync(_webSocket);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.ToString());
            }
        }

        private async Task ReceiveMessageAsync(ClientWebSocket webSocket)
        {
            while (_webSocket.State == WebSocketState.Open)
            {
                byte[] buffer = new byte[sizeof(int)];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if (JsonSerializer.Deserialize<UpdateEntity>(message) is UpdateEntity entity)
                    {
                        InvokeEvent(entity);
                    }
                }
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
                case UpdateEntity.Token:
                    TokenChanged?.Invoke(this, EventArgs.Empty);
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
            }
        }
    }
}
