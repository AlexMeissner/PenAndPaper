using Client.Helper;
using DataTransfer;
using DataTransfer.Map;
using System.Threading.Tasks;

namespace Client.Services.API
{
    public interface ITokenApi
    {
        public Task<ApiResponse<TokensDto>> GetAsync(int mapId);
        public Task<ApiResponse> PostAsync(TokenCreationDto payload);
        public Task<ApiResponse> PutAsync(TokenItem payload);
    }

    public class TokenApi : ITokenApi
    {
        private readonly IEndPointProvider _endPointProvider;

        public TokenApi(IEndPointProvider endPointProvider)
        {
            _endPointProvider = endPointProvider;
        }

        public Task<ApiResponse<TokensDto>> GetAsync(int mapId)
        {
            string url = _endPointProvider.BaseURL + $"Token?mapId={mapId}";
            return url.GetAsync<TokensDto>();
        }

        public Task<ApiResponse> PostAsync(TokenCreationDto payload)
        {
            string url = _endPointProvider.BaseURL + "Token";
            return url.PostAsync(payload);
        }

        public Task<ApiResponse> PutAsync(TokenItem payload)
        {
            string url = _endPointProvider.BaseURL + "Token";
            return url.PutAsync(payload);
        }
    }
}
