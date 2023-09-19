using DataTransfer.Map;
using DataTransfer.WebSocket;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;
        private readonly IUpdateNotifier _updateNotifier;

        public TokenController(SQLDatabase dbContext, IUpdateNotifier updateNotifier)
        {
            _dbContext = dbContext;
            _updateNotifier = updateNotifier;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int mapId)
        {
            try
            {
                var tokenIdsOnMap = await _dbContext.TokensOnMap.Where(x => x.MapId == mapId).Select(y => y.TokenId).ToListAsync();

                var items = new List<TokenItem>();

                foreach (var tokenId in tokenIdsOnMap)
                {
                    var token = await _dbContext.Tokens.FirstAsync(x => x.Id == tokenId);

                    int userId;
                    string name;
                    byte[] image;

                    if (token.CharacterId is int characterId)
                    {
                        var character = await _dbContext.Characters.FirstAsync(x => x.Id == characterId);
                        var user = await _dbContext.Users.FirstAsync(x => x.Id == characterId);
                        userId = user.Id;
                        name = character.Name;
                        image = character.Image;
                    }
                    else // token is controlled by game master
                    {
                        var map = await _dbContext.Maps.FirstAsync(x => x.Id == mapId);
                        var campaign = await _dbContext.Campaigns.FirstAsync(x => x.Id == map.CampaignId);
                        var gamemasterInCampaign = await _dbContext.UsersInCampaign.FirstAsync(x => x.CampaignId == campaign.Id && x.IsGamemaster);
                        var gamemaster = await _dbContext.Users.FirstAsync(x => x.Id == gamemasterInCampaign.UserId);

                        userId = gamemaster.Id;
                        name = "ToDo";
                        image = Array.Empty<byte>();
                    }

                    items.Add(new(
                        Id: token.Id,
                        UserId: userId,
                        X: token.X,
                        Y: token.Y,
                        Name: name,
                        Image: image));
                }

                var payload = new TokensDto(items);

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(TokenCreationDto payload)
        {
            try
            {
                var tokensOnMap = await _dbContext.TokensOnMap.Where(x => x.MapId == payload.MapId).Select(y => y.TokenId).ToListAsync();

                if (await _dbContext.Tokens.AnyAsync(x => tokensOnMap.Contains(x.Id) && x.CharacterId != null && x.CharacterId == payload.CharacterId))
                {
                    return Conflict();
                }

                var token = new DbToken()
                {
                    X = payload.X,
                    Y = payload.Y,
                    CharacterId = payload.CharacterId,
                    MonsterId = payload.MonsterId
                };

                await _dbContext.Tokens.AddAsync(token);
                await _dbContext.SaveChangesAsync();


                await _dbContext.TokensOnMap.AddAsync(new()
                {
                    MapId = payload.MapId,
                    TokenId = token.Id
                });

                await _dbContext.SaveChangesAsync();

                await _updateNotifier.Send(payload.CampaignId, UpdateEntity.Token);

                return CreatedAtAction(nameof(Get), token.Id);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(TokenUpdateDto payload)
        {
            try
            {
                var token = await _dbContext.Tokens.FirstOrDefaultAsync(x => x.Id == payload.Id);

                if (token is null)
                {
                    return NotFound(payload.Id);
                }

                token.X = payload.X;
                token.Y = payload.Y;

                await _dbContext.SaveChangesAsync();

                await _updateNotifier.Send(payload.CampaignId, UpdateEntity.Token);

                return Ok(token);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}