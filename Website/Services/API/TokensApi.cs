using DataTransfer.Map;
using static Website.Services.ServiceExtension;

namespace Website.Services.API
{
    public interface ITokenApi
    {
        public Task<HttpResponse<TokensDto>> GetAsync(int mapId);
        public Task<HttpResponse> PostAsync(TokenCreationDto payload);
        public Task<HttpResponse> PutAsync(TokenUpdateDto payload);
    }

    [TransistentService]
    public class TokenApi(IEndPointProvider endPointProvider, ITokenProvider tokenProvider) : ITokenApi
    {
        private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Token", tokenProvider);

        public Task<HttpResponse<TokensDto>> GetAsync(int mapId)
        {
            return _request.GetAsync<TokensDto>($"mapId={mapId}");
        }

        public Task<HttpResponse> PostAsync(TokenCreationDto payload)
        {
            return _request.PostAsync(payload);
        }

        public Task<HttpResponse> PutAsync(TokenUpdateDto payload)
        {
            return _request.PutAsync(payload);
        }
    }
}
