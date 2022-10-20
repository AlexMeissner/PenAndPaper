using DataTransfer;
using DataTransfer.CampaignCreation;
using DataTransfer.User;
using Microsoft.EntityFrameworkCore;
using Server.Database;

namespace Server.Services
{
    public interface ICampaignCreation
    {
        public Task<ApiResponse<CampaignCreationDto>> GetAsync(int campaignId);
        public Task<ApiResponse> PostAsync(CampaignCreationDto payload);
    }

    public class CampaignCreation : ICampaignCreation
    {
        private readonly SQLDatabase _dbContext;

        public CampaignCreation(SQLDatabase dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiResponse<CampaignCreationDto>> GetAsync(int campaignId)
        {
            CampaignCreationDto payload;

            try
            {
                var users = _dbContext.Users.Select(x => new UsersDto() { Id = x.Id, Email = x.Email, Username = x.Username });

                if (campaignId == -1) // New campaign
                {
                    payload = new CampaignCreationDto()
                    {
                        CampaignId = campaignId,
                        CampaignName = string.Empty,
                        Gamemaster = null,
                        UsersNotInCampaign = await users.ToListAsync().ConfigureAwait(false),
                        UsersInCampaign = new List<UsersDto>()
                    };
                }
                else // Existing campaign
                {
                    var dbCampaign = await _dbContext.Campaigns.FirstAsync(x => x.Id == campaignId).ConfigureAwait(false);
                    var dbUserInCampaign = _dbContext.UsersInCampaign.Where(x => x.CampaignId == campaignId);

                    var gamemasterId = (await dbUserInCampaign.FirstAsync(x => x.IsGamemaster)).UserId;
                    var gamemaster = await _dbContext.Users.FirstAsync(x => x.Id == gamemasterId);

                    var usersInCampaign = _dbContext.Users.Where(x => dbUserInCampaign.Any(y => x.Id == y.UserId));
                    var usersNotInCampaign = _dbContext.Users.Where(x => !usersInCampaign.Any(y => x.Id == y.Id));

                    payload = new CampaignCreationDto()
                    {
                        CampaignId = campaignId,
                        CampaignName = dbCampaign.Name,
                        Gamemaster = new UsersDto() { Id = gamemaster.Id, Email = gamemaster.Email, Username = gamemaster.Username },
                        UsersNotInCampaign = await usersInCampaign.Select(x => new UsersDto() { Id = x.Id, Username = x.Username, Email = x.Email }).ToListAsync(),
                        UsersInCampaign = await usersNotInCampaign.Select(x => new UsersDto() { Id = x.Id, Username = x.Username, Email = x.Email }).ToListAsync()
                    };
                }
            }
            catch (Exception exception)
            {
                return ApiResponse<CampaignCreationDto>.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }

            return ApiResponse<CampaignCreationDto>.Success(payload);
        }

        public async Task<ApiResponse> PostAsync(CampaignCreationDto payload)
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

                return ApiResponse.Success;
            }
            catch (Exception exception)
            {
                return ApiResponse.Failure(new ErrorDetails(ErrorCode.Exception, exception.Message));
            }
        }
    }
}