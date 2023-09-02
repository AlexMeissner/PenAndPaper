using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IEndPointProvider
    {
        public string BaseURL { get; }
        public string WebSocketBaseURL { get; }
    }

    [TransistentService]
    public class EndPointProvider : IEndPointProvider
    {
        public string BaseURL => "https://localhost:7099/";
        public string WebSocketBaseURL => "wss://localhost:7099/";
    }
}