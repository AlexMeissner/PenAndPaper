using ApiClient;

namespace Website.Services
{
    public class EndPointProvider(IConfiguration configuration) : IEndPointProvider
    {
        public string BaseUrl => configuration["ApiHost"]!;
    }
}