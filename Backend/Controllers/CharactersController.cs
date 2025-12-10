using Backend.Extensions;
using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.Character;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class CharactersController(IIdentity identity, ICharacterRepository characterRepository) : ControllerBase
{
    [HttpGet("characters/{characterId:int}")]
    public async Task<IActionResult> Get(int characterId)
    {
        var response = await characterRepository.GetAsync(characterId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpGet("campaigns/{campaignId:int}/characters")]
    public async Task<IActionResult> GetAll(int campaignId)
    {
        var response = await characterRepository.GetAllAsync(campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpGet("campaigns/{campaignId:int}/user-characters")]
    public async Task<IActionResult> GetAllForUser(int campaignId)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await characterRepository.GetAllAsync(campaignId, identityClaims.User.Id);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPost("campaigns/{campaignId:int}/characters")]
    public async Task<IActionResult> Post(int campaignId, CharacterCreationDto payload)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await characterRepository.CreateAsync(campaignId, identityClaims.User.Id, payload);

        return response.Match<IActionResult>(
            id => CreatedAtAction(nameof(Get), new { id }, id),
            this.StatusCode);
    }
}