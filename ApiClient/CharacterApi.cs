using DataTransfer.Character;
using DataTransfer.Response;

namespace ApiClient;

public interface ICharacterApi
{
    Task<Response<int>> CreateAsync(int campaignId, CharacterCreationDto payload);
    Task<Response<CharacterDto>> GetAsync(int characterId);
    Task<Response<IEnumerable<CharactersDto>>> GetAllAsync(int campaignId);
    Task<Response<IEnumerable<CharactersDto>>> GetAllFromUserAsync(int campaignId);
    Task<Response> UpdateAsync(int characterId, CharacterUpdateDto payload);
    Task<Response> UpdateHealthAsync(int characterId, CharacterHealthUpdate payload);
    Task<Response> UpdateSpellSlotsAsync(int characterId, CharacterSpellSlotUpdate payload);
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

    public Task<Response> UpdateAsync(int characterId, CharacterUpdateDto payload)
    {
        return requestBuilder.Path("characters", characterId).PutAsync(payload);
    }

    public Task<Response> UpdateHealthAsync(int characterId, CharacterHealthUpdate payload)
    {
        var update = new CharacterPatchDto(payload, null);
        return requestBuilder.Path("characters", characterId).PatchAsync(update);
    }

    public Task<Response> UpdateSpellSlotsAsync(int characterId, CharacterSpellSlotUpdate payload)
    {
        var update = new CharacterPatchDto(null, payload);
        return requestBuilder.Path("characters", characterId).PatchAsync(update);
    }
}