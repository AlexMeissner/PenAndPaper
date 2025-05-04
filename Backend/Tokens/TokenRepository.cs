using System.Net;
using Backend.Database;
using Backend.Database.Models;
using DataTransfer.Response;
using DataTransfer.Token;
using Microsoft.EntityFrameworkCore;

namespace Backend.Tokens;

public interface ITokenRepository
{
    Task<Response<TokenCreationResult>> CreateMonsterToken(int mapId, int monsterId, TokenCreationDto tokenCreationDto);
    Task<Response<TokenCreationResult>> CreateCharacterToken(int mapId, int characterId, TokenCreationDto tokenCreationDto);
    Task<Response> DeleteAsync(int tokenId);
    Response<IEnumerable<TokensDto>> GetAllAsync(int mapId);
    Task<int?> GetCampaignId(int tokenId);
    Task<Response> UpdateAsync(int tokenId, TokenUpdateDto payload);
}

public class TokenRepository(PenAndPaperDatabase dbContext) : ITokenRepository
{
    public async Task<Response<TokenCreationResult>> CreateMonsterToken(int mapId, int monsterId, TokenCreationDto tokenCreationDto)
    {
        var monster = await dbContext.Monsters.FindAsync(monsterId);

        if (monster is null)
        {
            return new Response<TokenCreationResult>(HttpStatusCode.NotFound);
        }

        var campaign = await dbContext.Campaigns
            .Include(c => c.Maps)
            .FirstOrDefaultAsync(c => c.Maps.Any(m => m.Id == mapId));

        if (campaign is null)
        {
            return new Response<TokenCreationResult>(HttpStatusCode.NotFound);
        }

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null)
        {
            return new Response<TokenCreationResult>(HttpStatusCode.NotFound);
        }

        var token = new MonsterToken()
        {
            X = tokenCreationDto.X,
            Y = tokenCreationDto.Y,
            Map = map,
            OwnerId = campaign.GameMasterId,
            Monster = monster
        };

        await dbContext.AddAsync(token);
        await dbContext.SaveChangesAsync();

        var result = new TokenCreationResult(token.Id, token.OwnerId, token.X, token.Y, monster.Image);

        return new Response<TokenCreationResult>(HttpStatusCode.Created, result);
    }

    public async Task<Response<TokenCreationResult>> CreateCharacterToken(int mapId, int characterId, TokenCreationDto tokenCreationDto)
    {
        var character = await dbContext.Characters.FindAsync(characterId);

        if (character is null)
        {
            return new Response<TokenCreationResult>(HttpStatusCode.NotFound);
        }

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null)
        {
            return new Response<TokenCreationResult>(HttpStatusCode.NotFound);
        }

        var token = new CharacterToken()
        {
            X = tokenCreationDto.X,
            Y = tokenCreationDto.Y,
            Map = map,
            OwnerId = character.UserId,
            Character = character
        };

        await dbContext.AddAsync(token);
        await dbContext.SaveChangesAsync();

        var result = new TokenCreationResult(token.Id, token.OwnerId, token.X, token.Y, character.Image);

        return new Response<TokenCreationResult>(HttpStatusCode.Created, result);
    }

    public async Task<Response> DeleteAsync(int tokenId)
    {
        var token = await dbContext.Tokens.FindAsync(tokenId);

        if (token is null)
        {
            return new Response(HttpStatusCode.NotFound);
        }

        dbContext.Remove(token);

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }

    public Response<IEnumerable<TokensDto>> GetAllAsync(int mapId)
    {
        var characterTokens = dbContext.CharacterTokens
            .Where(t => t.MapId == mapId)
            .Include(t => t.Character)
            .Select(t => new { t.Id, t.OwnerId, t.X, t.Y, t.Character.Name, t.Character.Image });

        var monsterTokens = dbContext.MonsterTokens
            .Where(t => t.MapId == mapId)
            .Include(t => t.Monster)
            .Select(t => new { t.Id, t.OwnerId, t.X, t.Y, t.Monster.Name, t.Monster.Image });

        var tokens = characterTokens
            .Concat(monsterTokens)
            .Select(t => new TokensDto(t.Id, t.OwnerId, t.X, t.Y, t.Name, t.Image));

        return new Response<IEnumerable<TokensDto>>(HttpStatusCode.OK, tokens);
    }

    public async Task<int?> GetCampaignId(int tokenId)
    {
        var token = await dbContext.Tokens
            .Include(t => t.Map)
            .FirstOrDefaultAsync(t => t.Id == tokenId);

        return token?.Map.CampaignId;
    }

    public async Task<Response> UpdateAsync(int tokenId, TokenUpdateDto payload)
    {
        var token = await dbContext.Tokens.FindAsync(tokenId);

        if (token is null)
        {
            return new Response(HttpStatusCode.NotFound);
        }

        token.X = payload.X;
        token.Y = payload.Y;

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }
}