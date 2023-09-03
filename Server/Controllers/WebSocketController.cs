using Microsoft.AspNetCore.Mvc;
using Server.Services;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController : ControllerBase
    {
        private readonly IUpdateNotifier _updateNotifier;

        public WebSocketController(IUpdateNotifier updateNotifier)
        {
            _updateNotifier = updateNotifier;
        }

        [HttpGet]
        public async Task Get()
        {
            WebSocket? webSocket = null;

            try
            {
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    _updateNotifier.Add(webSocket);
                    await SocketLifeCircle(webSocket);
                }
            }
            finally
            {
                if (webSocket is not null)
                {
                    _updateNotifier.Remove(webSocket);
                }
            }
        }

        private async Task SocketLifeCircle(WebSocket webSocket)
        {
            var buffer = new byte[sizeof(int)];
            WebSocketReceiveResult result;

            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    if (JsonSerializer.Deserialize<int>(message) is int campaignId)
                    {
                        _updateNotifier.SetCampaignId(webSocket, campaignId);
                    }
                }

            } while (!result.CloseStatus.HasValue);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
