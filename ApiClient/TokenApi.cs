using DataTransfer.Response;
using DataTransfer.Token;

namespace ApiClient;

public interface ITokenApi
{
    Task<Response<int>> CreateCharacter(int mapId, int characterId, TokenCreationDto payload);
    Task<Response<int>> CreateMonster(int mapId, int monsterId, TokenCreationDto payload);
    Task<Response<IEnumerable<TokensDto>>> GetAllAsync(int mapId);
    Task<Response> MoveAsync(int tokenId, TokenUpdateDto payload);
    Task<Response> RemoveAsync(int tokenId);
}

public class TokenApi(IRequest request) : ITokenApi
{
    public Task<Response<int>> CreateCharacter(int mapId, int characterId, TokenCreationDto payload)
    {
        return request.Path("maps", mapId, "character-tokens", characterId).PostAsync<int>(payload);
    }

    public Task<Response<int>> CreateMonster(int mapId, int monsterId, TokenCreationDto payload)
    {
        return request.Path("maps", mapId, "monster-tokens", monsterId).PostAsync<int>(payload);
    }

    public Task<Response<IEnumerable<TokensDto>>> GetAllAsync(int mapId)
    {
        return request.Path("maps", mapId, "tokens").GetAsync<IEnumerable<TokensDto>>();
    }

    public Task<Response> MoveAsync(int tokenId, TokenUpdateDto payload)
    {
        return request.Path("tokens", tokenId).PatchAsync(payload);
    }

    public Task<Response> RemoveAsync(int tokenId)
    {
        return request.Path("tokens", tokenId).DeleteAsync();
    }
}