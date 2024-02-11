using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IEndPointProvider
    {
        public string BaseURL { get; }
        public string WebSocketBaseURL { get; }
    }

    [TransistentService]
    public class EndPointProvider(ISettings settings) : IEndPointProvider
    {
        public string BaseURL => settings.APIHost;
        public string WebSocketBaseURL => settings.WebsocketHost;
    }
}
// ToDo: Do I need this class? Or should I just use ISettings?
