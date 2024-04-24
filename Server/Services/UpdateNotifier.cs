using DataTransfer.WebSocket;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Server.Services
{
    public interface IUpdateNotifier
    {
        bool Add(WebSocket webSocket);
        bool Remove(WebSocket webSocket);
        Task Send(int campaignId, object entity);
        void SetCampaignId(WebSocket webSocket, int campaignId);
    }

    public class UpdateNotifier(ILogger<UpdateNotifier> logger) : IUpdateNotifier
    {
        private readonly ConcurrentDictionary<WebSocket, int> _webSockets = new();

        public bool Add(WebSocket webSocket)
        {
            return _webSockets.TryAdd(webSocket, -1);
        }

        public bool Remove(WebSocket webSocket)
        {
            return _webSockets.TryRemove(webSocket, out int _);
        }

        public async Task Send(int campaignId, object entity)
        {
            logger.LogInformation("Sending update {entity} ...", entity);

            var update = new WebSocketUpdate(entity);
            var message = JsonSerializer.Serialize(update);
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            var clientsInSameCampaign = _webSockets.Where(x => x.Value == campaignId).Select(y => y.Key);

            foreach (var socket in clientsInSameCampaign)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public void SetCampaignId(WebSocket webSocket, int campaignId)
        {
            _webSockets[webSocket] = campaignId;
        }
    }
}
