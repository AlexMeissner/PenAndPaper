using static Website.Services.ServiceExtension;

namespace Website.Services
{
    public interface IEndPointProvider
    {
        public string BaseURL { get; }
        public string WebSocketBaseURL { get; }
    }

    [TransistentService]
    public class EndPointProvider(IConfiguration configuration) : IEndPointProvider
    {
        public string BaseURL => configuration["ApiHost"]!;
        public string WebSocketBaseURL => configuration["WebSocketHost"]!;
    }
}
