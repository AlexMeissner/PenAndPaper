using DataTransfer.Character;
using DataTransfer.Response;

namespace ApiClient;

public interface ICharacterApi
{
    Task<Response<int>> CreateAsync(int campaignId, CharacterCreationDto payload);
    Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId);
}

public class CharacterApi(IRequestBuilder requestBuilder) : ICharacterApi
{
    public Task<Response<int>> CreateAsync(int campaignId, CharacterCreationDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "characters").PostAsync<int>(payload);
    }

    public Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId)
    {
        return requestBuilder.Path("campaigns", campaignId, "characters").GetAsync<IEnumerable<CharactersDto>>();
    }
}