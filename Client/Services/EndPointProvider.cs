using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface IEndPointProvider
    {
        public string BaseURL { get; }
    }

    [TransistentService]
    public class EndPointProvider : IEndPointProvider
    {
        public string BaseURL => "https://localhost:7099/";
    }
}