using Backend.Extensions;
using Backend.Hubs;
using DataTransfer.Initiative;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Initiative;

[ApiController]
public class InitiativeController(IInitiativeRepository initiativeRepository,
    IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub) : ControllerBase
{
    [HttpPost("campaigns/{campaignId:int}/maps/{mapId:int}/combatants")]
    public async Task<IActionResult> Add(int campaignId, int mapId, AddCombatantDto payload)
    {
        var response = await initiativeRepository.AddCombatant(mapId, payload);

        var combatant = response.Match<CombatantDto?>(
            combatant => combatant,
            statusCode => null);

        if (combatant is not null)
        {
            int? characterId = combatant is CharacterCombatantDto characterCombatant ? characterCombatant.CharacterId : null;
            int? monsterId = combatant is MonsterCombatantDto monsterCombatant ? monsterCombatant.MonsterId : null;

            var eventArgs = new CombatantAddedEventArgs(combatant.TokenId, combatant.Initiative, combatant.Image, combatant.Color, characterId, monsterId);
            await campaignUpdateHub.Clients.AllInCampaign(campaignId).CombatantAdded(eventArgs);
        }

        return this.StatusCode(response.StatusCode);
    }

    [HttpGet("maps/{mapId:int}/combatants")]
    public async Task<IActionResult> Get(int mapId)
    {
        var response = await initiativeRepository.GetCombatants(mapId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpDelete("campaigns/{campaignId:int}/maps/{mapId:int}/combatants/{tokenId:int}")]
    public async Task<IActionResult> Remove(int campaignId, int mapId, int tokenId)
    {
        var response = await initiativeRepository.RemoveCombatants(mapId, tokenId);

        if (response.IsSuccess)
        {
            var eventArgs = new CombatantRemovedEventArgs(tokenId);
            await campaignUpdateHub.Clients.AllInCampaign(campaignId).CombatantRemoved(eventArgs);
        }

        return this.StatusCode(response.StatusCode);
    }

    [HttpPatch("campaigns/{campaignId:int}/maps/{mapId:int}/combatants/{tokenId:int}")]
    public async Task<IActionResult> UpdateInitiative(int campaignId, int mapId, int tokenId, CombatantUpdateDto payload)
    {
        var response = await initiativeRepository.UpdateCombatant(mapId, tokenId, payload);

        if (response.IsSuccess)
        {
            var eventArgs = new CombatantUpdatedEventArgs(tokenId, payload.Initiative);
            await campaignUpdateHub.Clients.AllInCampaign(campaignId).CombatantUpdated(eventArgs);
        }

        return this.StatusCode(response.StatusCode);
    }

    [HttpPatch("campaigns/{campaignId:int}/maps/{mapId:int}/combatants")]
    public async Task<IActionResult> UpdateTurn(int campaignId, int mapId, CombatantsUpdateDto payload)
    {
        var response = await initiativeRepository.UpdateTurn(mapId, payload);

        if (response.IsSuccess)
        {
            var eventArgs = new TurnChangedEventArgs(payload.TokenId);
            await campaignUpdateHub.Clients.AllInCampaign(campaignId).TurnChanged(eventArgs);
        }

        return this.StatusCode(response.StatusCode);
    }
}
