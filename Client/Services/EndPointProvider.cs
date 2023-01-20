namespace Client.Services
{
    public interface IEndPointProvider
    {
        public string BaseURL { get; }
    }

    public class EndPointProvider : IEndPointProvider
    {
        public string BaseURL => "https://localhost:7099/";
    }
}