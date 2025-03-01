using Backend.Extensions;
using Backend.Services.Repositories;
using DataTransfer.Character;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}")]
public class CharactersController(ICharacterRepository characterRepository) : ControllerBase
{
    [HttpGet("characters")]
    public async Task<IActionResult> GetAll(int campaignId)
    {
        var response = await characterRepository.GetAllAsync(campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpGet("users/{userId:int}/characters")]
    public async Task<IActionResult> GetAll(int campaignId, int userId)
    {
        var response = await characterRepository.GetAllAsync(campaignId, userId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPost("users/{userId:int}/characters")]
    public async Task<IActionResult> Post(int campaignId, int userId, CharacterCreationDto payload)
    {
        var response = await characterRepository.CreateAsync(campaignId, userId, payload);

        return response.Match<IActionResult>(
            _ => CreatedAtAction(nameof(GetAll), new { campaignId, userId }),
            this.StatusCode);
    }
}