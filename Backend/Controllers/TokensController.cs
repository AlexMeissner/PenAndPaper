using Backend.Extensions;
using Backend.Services.Repositories;
using DataTransfer.Token;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class TokensController(ITokenRepository tokenRepository) : ControllerBase
{
    [HttpDelete("tokens/{tokenId:int}")]
    public async Task<IActionResult> Delete(int tokenId)
    {
        var response = await tokenRepository.DeleteAsync(tokenId);

        return this.StatusCode(response.StatusCode);
    }

    [HttpPatch("tokens/{tokenId:int}")]
    public async Task<IActionResult> Update(int tokenId, TokenUpdateDto payload)
    {
        var response = await tokenRepository.UpdateAsync(tokenId, payload);

        return this.StatusCode(response.StatusCode);
    }

    [HttpGet("maps/{mapId:int}/tokens")]
    public IActionResult GetAll(int mapId)
    {
        var response = tokenRepository.GetAllAsync(mapId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPost("maps/{mapId:int}/character-tokens/{characterId:int}")]
    public async Task<IActionResult> CreateCharacter(int mapId, int characterId, TokenCreationDto payload)
    {
        var response = await tokenRepository.CreateCharacterToken(mapId, characterId, payload);

        return response.Match<IActionResult>(
            _ => Created("", mapId), // "" because there is no explicit getter for a token (yet)
            this.StatusCode);
    }

    [HttpPost("maps/{mapId:int}/monster-tokens/{monsterId:int}")]
    public async Task<IActionResult> CreateMonster(int mapId, int monsterId, TokenCreationDto payload)
    {
        var response = await tokenRepository.CreateMonsterToken(mapId, monsterId, payload);

        return response.Match<IActionResult>(
            _ => Created("", mapId), // "" because there is no explicit getter for a token (yet)
            this.StatusCode);
    }
}