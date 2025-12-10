using DataTransfer.Character;
using DataTransfer.Response;

namespace ApiClient;

public interface ICharacterApi
{
    Task<Response<int>> CreateAsync(int campaignId, CharacterCreationDto payload);
    Task<Response<CharacterDto>> GetAsync(int characterId);
    Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId);
    Task<Response<IEnumerable<CharactersDto>>> GetAllFromUserAsync(int campaignId);
}

public class CharacterApi(IRequestBuilder requestBuilder) : ICharacterApi
{
    public Task<Response<int>> CreateAsync(int campaignId, CharacterCreationDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "characters").PostAsync<int>(payload);
    }

    public Task<Response<CharacterDto>> GetAsync(int characterId)
    {
        return requestBuilder.Path("characters", characterId).GetAsync<CharacterDto>();
    }

    public Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId)
    {
        return requestBuilder.Path("campaigns", campaignId, "characters").GetAsync<IEnumerable<CharactersDto>>();
    }

    public Task<Response<IEnumerable<CharactersDto>>> GetAllFromUserAsync(int campaignId)
    {
        return requestBuilder.Path("campaigns", campaignId, "user-characters").GetAsync<IEnumerable<CharactersDto>>();
    }
}