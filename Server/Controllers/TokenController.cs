﻿using DataTransfer.Map;
using DataTransfer.Token;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Server.Hubs;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController(
        SQLDatabase dbContext,
        IUpdateNotifier updateNotifier,
        IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub) : ControllerBase
    {
        [HttpDelete]
        public async Task<IActionResult> Delete(int tokenId)
        {
            var token = await dbContext.Tokens.FindAsync(tokenId);

            if (token is null)
            {
                return NotFound(tokenId);
            }

            dbContext.Tokens.Remove(token);
            await dbContext.SaveChangesAsync();

            // ToDo: Fire an event such that all connected client remove the token from the map

            return Ok(tokenId);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int mapId)
        {
            var map = await dbContext.Maps
                .Include(m => m.Campaign)
                .ThenInclude(c => c.Gamemaster)
                .FirstOrDefaultAsync(m => m.Id == mapId);

            if (map is null)
            {
                return NotFound(mapId);
            }

            var monsterTokenItems = await dbContext.MonsterTokens
                .Include(t => t.Monster)
                .Where(t => t.MapId == mapId)
                .Select(t => new TokenItem(t.Id, map.Campaign.Gamemaster.Id, t.X, t.Y, t.Monster.Name, t.Monster.Image))
                .ToListAsync();

            var characterTokens = await dbContext.CharacterTokens
                .Include(t => t.Character)
                .ThenInclude(c => c.User)
                .Where(t => t.MapId == mapId)
                .Select(t => new TokenItem(t.Id, t.Character.User.Id, t.X, t.Y, t.Character.Name, t.Character.Image))
                .ToListAsync();

            var tokens = characterTokens.Concat(monsterTokenItems).ToList();

            var payload = new TokensDto(tokens);

            return Ok(payload);
        }

        [HttpPost]
        public async Task<IActionResult> Post(TokenCreationDto payload)
        {
            // ToDo: Refactor: One endpoint for monster token and one for character token

            var map = await dbContext.Maps.Include(m => m.Tokens).FirstOrDefaultAsync(m => m.Id == payload.MapId);

            if (map is null)
            {
                return NotFound(payload.MapId);
            }

            var character = await dbContext.Characters.Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == payload.CharacterId);

            int userId;

            if (character is null)
            {
                var campaign = await dbContext.Campaigns
                    .Include(c => c.Gamemaster)
                    .FirstAsync(c => c.Id == payload.CampaignId);

                userId = campaign.Gamemaster.Id;
            }
            else
            {
                userId = character.User.Id;
            }

            var alreadyContainsCharacter = await dbContext.CharacterTokens
                .Include(t => t.Character)
                .AnyAsync(t =>
                    t.MapId == payload.MapId && payload.CharacterId != null && t.Character.Id == payload.CharacterId);

            if (alreadyContainsCharacter)
            {
                return Conflict(payload.CharacterId);
            }

            var token = await CreateToken(payload);

            map.Tokens.Add(token);

            await dbContext.SaveChangesAsync();

            await updateNotifier.Send(payload.CampaignId, UpdateEntity.TokenAdded);

            var image = token switch
            {
                CharacterToken characterToken => characterToken.Character.Image,
                MonsterToken monsterToken => monsterToken.Monster.Image,
                _ => throw new Exception("Unknown token type")
            };

            // ToDo: Should only notify clients in campaign
            var eventArgs = new TokenAddedEventArgs(token.Id, userId, image, token.X, token.Y);
            await campaignUpdateHub.Clients.All.TokenAdded(eventArgs);

            return CreatedAtAction(nameof(Get), token.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(TokenUpdateDto payload)
        {
            var token = await dbContext.Tokens.FindAsync(payload.Id);

            if (token is null)
            {
                return NotFound(payload.Id);
            }

            token.X = payload.X;
            token.Y = payload.Y;

            await dbContext.SaveChangesAsync();

            var tokenUpdate = new TokenMovedDto(token.Id, token.X, token.Y);
            await updateNotifier.Send(payload.CampaignId, tokenUpdate);

            // ToDo: Should only notify clients in campaign
            var eventArgs = new TokenMovedEventArgs(token.Id, token.X, token.Y);
            await campaignUpdateHub.Clients.All.TokenMoved(eventArgs);

            return Ok(token);
        }

        private async Task<Token> CreateToken(TokenCreationDto creationInfo)
        {
            if (creationInfo.CharacterId is not null)
            {
                var character = await dbContext.Characters.FindAsync(creationInfo.CharacterId) ??
                                throw new NullReferenceException();

                return new CharacterToken()
                {
                    X = creationInfo.X,
                    Y = creationInfo.Y,
                    Character = character,
                };
            }
            else if (creationInfo.MonsterId is not null)
            {
                var monster = await dbContext.Monsters.FindAsync(creationInfo.MonsterId) ??
                              throw new NullReferenceException();

                return new MonsterToken()
                {
                    X = creationInfo.X,
                    Y = creationInfo.Y,
                    Monster = monster,
                };
            }

            throw new NotImplementedException("Unknown token type");
        }
    }
}