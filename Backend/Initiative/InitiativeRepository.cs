using Backend.Database;
using Backend.Tokens;
using DataTransfer.Initiative;
using DataTransfer.Response;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Backend.Initiative;

public interface IInitiativeRepository
{
    Task<Response<CombatantDto>> AddCombatant(int mapId, AddCombatantDto payload);
    Task<Response<IEnumerable<CombatantDto>>> GetCombatants(int mapId);
    Task<Response> RemoveCombatants(int mapId, int tokenId);
    Task<Response> UpdateCombatant(int mapId, int tokenId, CombatantUpdateDto payload);
    Task<Response> UpdateTurn(int mapId, CombatantsUpdateDto payload);
}

public class InitiativeRepository(PenAndPaperDatabase dbContext) : IInitiativeRepository
{
    const uint DefaultInitiative = 1;

    public async Task<Response<CombatantDto>> AddCombatant(int mapId, AddCombatantDto payload)
    {
        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return new Response<CombatantDto>(HttpStatusCode.NotFound);

        var token = await dbContext.Tokens
            .Include(t => (t as CharacterToken)!.Character.User)
            .Include(t => (t as MonsterToken)!.Monster)
            .FirstOrDefaultAsync(t => t.Id == payload.TokenId);

        if (token is null) return new Response<CombatantDto>(HttpStatusCode.NotFound);

        token.Initiative = 1;
        map.ActingToken ??= token;

        await dbContext.SaveChangesAsync();

        var combatant = token switch
        {
            CharacterToken characterToken => new CombatantDto(
                characterToken.Id,
                DefaultInitiative,
                Convert.ToBase64String(characterToken.Character.Image),
                characterToken.Character.User.Color),

            MonsterToken monsterToken => new CombatantDto(
                monsterToken.Id,
                DefaultInitiative,
                Convert.ToBase64String(monsterToken.Monster.Image),
                "#d00000"),

            _ => throw new Exception("Unknown token type")
        };

        return new Response<CombatantDto>(HttpStatusCode.OK, combatant);
    }

    public async Task<Response<IEnumerable<CombatantDto>>> GetCombatants(int mapId)
    {
        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return new Response<IEnumerable<CombatantDto>>(HttpStatusCode.NotFound);

        var tokens = await dbContext.Maps
            .Where(m => m.Id == mapId)
            .SelectMany(m => m.Tokens)
            .Where(t => t.Initiative != null)
            .Include(t => (t as CharacterToken)!.Character.User)
            .Include(t => (t as MonsterToken)!.Monster)
            .ToListAsync();

        var combatants = tokens.Select(token =>
        {
            var image = token switch
            {
                CharacterToken characterToken => characterToken.Character.Image,
                MonsterToken monsterToken => monsterToken.Monster.Image,
                _ => []
            };

            var color = token switch
            {
                CharacterToken characterToken => characterToken.Character.User.Color,
                MonsterToken monsterToken => "#d00000",
                _ => ""
            };

            return new CombatantDto(token.Id, token.Initiative ?? DefaultInitiative, Convert.ToBase64String(image), color);
        });

        return new Response<IEnumerable<CombatantDto>>(HttpStatusCode.OK, combatants);
    }

    public async Task<Response> RemoveCombatants(int mapId, int tokenId)
    {
        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return new Response(HttpStatusCode.NotFound);

        var token = await dbContext.Tokens.FindAsync(tokenId);

        if (token is null) return new Response(HttpStatusCode.NotFound);

        token.Initiative = null;

        if (map.ActingTokenId == tokenId)
        {
            map.ActingToken = null;
        }

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }

    public async Task<Response> UpdateCombatant(int mapId, int tokenId, CombatantUpdateDto payload)
    {
        var map = await dbContext.Maps.Include(m => m.Tokens).FirstOrDefaultAsync(m => m.Id == mapId);

        if (map is null) return new Response(HttpStatusCode.NotFound);

        var token = map.Tokens.FirstOrDefault(t => t.Id == tokenId);

        if (token is null) return new Response(HttpStatusCode.NotFound);

        token.Initiative = payload.Initiative;

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }

    public async Task<Response> UpdateTurn(int mapId, CombatantsUpdateDto payload)
    {
        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return new Response(HttpStatusCode.NotFound);

        var token = await dbContext.Tokens.FindAsync(payload.TokenId);

        if (token is null) return new Response(HttpStatusCode.NotFound);

        map.ActingToken = token;

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }
}
