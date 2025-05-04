using Backend.Extensions;
using Backend.Services;
using Backend.Services.Repositories;
using DataTransfer.Character;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}")]
public class CharactersController(IIdentity identity, ICharacterRepository characterRepository) : ControllerBase
{
    [HttpGet("characters")]
    public async Task<IActionResult> GetAll(int campaignId)
    {
        var response = await characterRepository.GetAllAsync(campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpGet("user-characters")]
    public async Task<IActionResult> GetAllForUser(int campaignId)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await characterRepository.GetAllAsync(campaignId, identityClaims.User.Id);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPost("characters")]
    public async Task<IActionResult> Post(int campaignId, CharacterCreationDto payload)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await characterRepository.CreateAsync(campaignId, identityClaims.User.Id, payload);

        return response.Match<IActionResult>(
            id => CreatedAtAction(nameof(GetAll), new { campaignId }, id),
            this.StatusCode);
    }
}