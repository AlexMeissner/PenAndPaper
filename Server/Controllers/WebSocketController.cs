using Microsoft.AspNetCore.Mvc;
using Server.Services;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebSocketController(IUpdateNotifier updateNotifier, ILogger<WebSocketController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task Get()
        {
            WebSocket? webSocket = null;

            try
            {
                if (HttpContext.WebSockets.IsWebSocketRequest)
                {
                    webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                    updateNotifier.Add(webSocket);

                    logger.LogInformation("Websocket created ({ip})", HttpContext.Connection.RemoteIpAddress);

                    await SocketLifeCircle(webSocket);
                }
            }
            finally
            {
                if (webSocket is not null)
                {
                    updateNotifier.Remove(webSocket);
                    logger.LogInformation("Websocket closed ({ip})", HttpContext.Connection.RemoteIpAddress);
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
                        updateNotifier.SetCampaignId(webSocket, campaignId);
                    }
                }

            } while (!result.CloseStatus.HasValue);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
