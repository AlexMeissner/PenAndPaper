using DataTransfer.Campaign;
using DataTransfer.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignCreationController : ControllerBase
    {
        private readonly SQLDatabase _dbContext;

        public CampaignCreationController(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int campaignId)
        {
            try
            {
                CampaignCreationDto payload;

                var users = _dbContext.Users.Select(x => new UsersDto(x.Id, x.Username, x.Email));

                if (campaignId == -1) // New campaign
                {
                    payload = new CampaignCreationDto(
                        CampaignId: campaignId,
                        CampaignName: string.Empty,
                        Gamemaster: null,
                        UsersNotInCampaign: await users.ToListAsync().ConfigureAwait(false),
                        UsersInCampaign: new List<UsersDto>());
                }
                else // Existing campaign
                {
                    var dbCampaign = await _dbContext.Campaigns.FirstAsync(x => x.Id == campaignId).ConfigureAwait(false);
                    var dbUserInCampaign = _dbContext.UsersInCampaign.Where(x => x.CampaignId == campaignId);

                    var gamemasterId = (await dbUserInCampaign.FirstAsync(x => x.IsGamemaster)).UserId;
                    var gamemaster = await _dbContext.Users.FirstAsync(x => x.Id == gamemasterId);

                    var usersInCampaign = _dbContext.Users.Where(x => dbUserInCampaign.Any(y => x.Id == y.UserId));
                    var usersNotInCampaign = _dbContext.Users.Where(x => !usersInCampaign.Any(y => x.Id == y.Id));

                    payload = new CampaignCreationDto(
                        CampaignId: campaignId,
                        CampaignName: dbCampaign.Name,
                        Gamemaster: new UsersDto(gamemaster.Id, gamemaster.Username, gamemaster.Email),
                        UsersNotInCampaign: await usersInCampaign.Select(x => new UsersDto(x.Id, x.Username, x.Email)).ToListAsync(),
                        UsersInCampaign: await usersNotInCampaign.Select(x => new UsersDto(x.Id, x.Username, x.Email)).ToListAsync()
                    );
                }

                return Ok(payload);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(CampaignCreationDto payload)
        {
            try
            {
                var dbCampaign = new DbCampaign() { Name = payload.CampaignName };
                await _dbContext.Campaigns.AddAsync(dbCampaign);
                await _dbContext.SaveChangesAsync();

                await _dbContext.UsersInCampaign.AddAsync(new()
                {
                    UserId = payload.Gamemaster!.Id,
                    CampaignId = dbCampaign.Id,
                    IsGamemaster = true
                });

                _dbContext.ActiveCampaignElements.Add(new()
                {
                    CampaignId = dbCampaign.Id,
                    MapId = -1,
                    AmbientId = -1,
                    EffectId = -1,
                });

                foreach (var user in payload.UsersInCampaign)
                {
                    await _dbContext.UsersInCampaign.AddAsync(new()
                    {
                        UserId = user.Id,
                        CampaignId = dbCampaign.Id,
                        IsGamemaster = false
                    });
                }

                await _dbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(Get), dbCampaign.Id);
            }
            catch (Exception exception)
            {
                return this.InternalServerError(exception);
            }
        }
    }
}