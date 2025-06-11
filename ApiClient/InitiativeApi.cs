using DataTransfer.Initiative;
using DataTransfer.Response;

namespace ApiClient;

public interface IInitiativeApi
{
    Task<Response> AddCombatant(int campaignId, int mapId, AddCombatantDto payload);
    Task<Response<IEnumerable<CombatantDto>>> GetCombatants(int mapId);
    Task<Response> RemoveCombatant(int campaignId, int mapId, int tokenId);
    Task<Response> UpdateCombatant(int campaignId, int mapId, int tokenId, CombatantUpdateDto payload);
    Task<Response> UpdateTurn(int campaignId, int mapId, CombatantsUpdateDto payload);
}

public class InitiativeApi(IRequestBuilder requestBuilder) : IInitiativeApi
{
    public Task<Response> AddCombatant(int campaignId, int mapId, AddCombatantDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "maps", mapId, "combatants").PostAsync(payload);
    }

    public Task<Response<IEnumerable<CombatantDto>>> GetCombatants(int mapId)
    {
        return requestBuilder.Path("maps", mapId, "combatants").GetAsync<IEnumerable<CombatantDto>>();
    }

    public Task<Response> RemoveCombatant(int campaignId, int mapId, int tokenId)
    {
        return requestBuilder.Path("campaigns", campaignId, "maps", mapId, "combatants", tokenId).DeleteAsync();
    }

    public Task<Response> UpdateCombatant(int campaignId, int mapId, int tokenId, CombatantUpdateDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "maps", mapId, "combatants", tokenId).PatchAsync(payload);
    }

    public Task<Response> UpdateTurn(int campaignId, int mapId, CombatantsUpdateDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "maps", mapId, "combatants").PatchAsync(payload);
    }
}
