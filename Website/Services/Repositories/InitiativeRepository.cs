using Microsoft.EntityFrameworkCore;
using Website.Components.Controls;
using Website.Components.Pages;
using Website.Database;
using Website.Database.Models;
using Website.Events;

namespace Website.Services.Repositories;

public interface IInitiativeRepository
{
    Task<int?> AddCombatant(int campaignId, int mapId, int tokenId);
    Task<List<Combatant>> GetCombatants(int mapId);
    Task UpdateCombatant(int campaignId, int mapId, int tokenId, uint initiative);
    Task RemoveCombatant(int campaignId, int mapId, int tokenId);
    Task UpdateTurn(int campaignId, int mapId, int tokenId);
}

public class InitiativeRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory, ICampaignEventHub campaignEventHub) : IInitiativeRepository
{
    const uint DefaultInitiative = 1;

    public async Task<int?> AddCombatant(int campaignId, int mapId, int tokenId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return null;

        var token = await dbContext.Tokens
            .Include(t => (t as CharacterToken)!.Character.User)
            .Include(t => (t as MonsterToken)!.Monster)
            .FirstOrDefaultAsync(t => t.Id == tokenId);

        if (token is null) return null;

        token.Initiative = 1;
        map.ActingToken ??= token;

        await dbContext.SaveChangesAsync();

        switch (token)
        {
            case CharacterToken characterToken:
                campaignEventHub
                    .ForCampaign(campaignId)
                    .Publish(new CombatantAddedEvent(token.Id, DefaultInitiative, Convert.ToBase64String(characterToken.Character.Image), characterToken.Character.User.Color, characterToken.CharacterId, null));
                break;

            case MonsterToken monsterToken:
                campaignEventHub
                    .ForCampaign(campaignId)
                    .Publish(new CombatantAddedEvent(token.Id, DefaultInitiative, Convert.ToBase64String(monsterToken.Monster.Image), "#d00000", null, monsterToken.MonsterId));
                break;

            default:
                throw new Exception("Unknown token type");
        }

        return token.Id;
    }

    public async Task<List<Combatant>> GetCombatants(int mapId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var tokens = await dbContext.Maps
            .AsNoTracking()
            .Where(m => m.Id == mapId)
            .SelectMany(m => m.Tokens)
            .Where(t => t.Initiative != null)
            .Include(t => (t as CharacterToken)!.Character.User)
            .Include(t => (t as MonsterToken)!.Monster)
            .ToListAsync();

        if (tokens.Count == 0) return [];

        return [.. tokens.Select<Token, Combatant>(token =>
        {
            return token switch
            {
                CharacterToken characterToken => new CharacterCombatant()
                {
                    TokenId = characterToken.Id,
                    Initiative = characterToken.Initiative ?? DefaultInitiative,
                    Image = Convert.ToBase64String(characterToken.Character.Image),
                    Color = characterToken.Character.User.Color,
                    CharacterId = characterToken.CharacterId
                },

                MonsterToken monsterToken => new MonsterCombatant()
                {
                    TokenId = monsterToken.Id,
                    Initiative = monsterToken.Initiative ?? DefaultInitiative,
                    Image = Convert.ToBase64String(monsterToken.Monster.Image),
                    Color = "#d00000",
                    MonsterId = monsterToken.MonsterId
                },

                _ => throw new Exception("Unknown token type"),
            };
        }).OrderByDescending(c => c.Initiative)];
    }

    public async Task RemoveCombatant(int campaignId, int mapId, int tokenId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return;

        var token = await dbContext.Tokens.FindAsync(tokenId);

        if (token is null) return;

        token.Initiative = null;

        if (map.ActingTokenId == tokenId)
        {
            map.ActingToken = null;
        }

        await dbContext.SaveChangesAsync();

        campaignEventHub
            .ForCampaign(campaignId)
            .Publish(new CombatantRemovedEvent(tokenId));
    }

    public async Task UpdateCombatant(int campaignId, int mapId, int tokenId, uint initiative)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.Include(m => m.Tokens).FirstOrDefaultAsync(m => m.Id == mapId);

        if (map is null) return;

        var token = map.Tokens.FirstOrDefault(t => t.Id == tokenId);

        if (token is null) return;

        token.Initiative = initiative;

        await dbContext.SaveChangesAsync();

        campaignEventHub
           .ForCampaign(campaignId)
           .Publish(new CombatantUpdatedEvent(tokenId, initiative));
    }

    public async Task UpdateTurn(int campaignId, int mapId, int tokenId)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null) return;

        var token = await dbContext.Tokens.FindAsync(tokenId);

        if (token is null) return;

        map.ActingToken = token;

        await dbContext.SaveChangesAsync();

        campaignEventHub
           .ForCampaign(campaignId)
           .Publish(new TurnChangedEvent(tokenId));
    }
}
