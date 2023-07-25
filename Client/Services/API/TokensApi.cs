using DataTransfer.Map;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services.API
{
    public interface ITokenApi
    {
        public Task<HttpResponse<TokensDto>> GetAsync(int mapId);
        public Task<HttpResponse> PostAsync(TokenCreationDto payload);
        public Task<HttpResponse> PutAsync(TokenItem payload);
    }

    [TransistentService]
    public class TokenApi : ITokenApi
    {
        private readonly HttpRequest _request;

        public TokenApi(IEndPointProvider endPointProvider)
        {
            _request = new(endPointProvider.BaseURL + "Token");
        }

        public Task<HttpResponse<TokensDto>> GetAsync(int mapId)
        {
            return _request.GetAsync<TokensDto>($"mapId={mapId}");
        }

        public Task<HttpResponse> PostAsync(TokenCreationDto payload)
        {
            return _request.PostAsync(payload);
        }

        public Task<HttpResponse> PutAsync(TokenItem payload)
        {
            return _request.PutAsync(payload);
        }
    }
}
