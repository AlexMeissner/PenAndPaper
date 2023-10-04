using DataTransfer.Campaign;
using DataTransfer.Dice;
using DataTransfer.Map;
using DataTransfer.User;
using DataTransfer.WebSocket;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Text.Json;

namespace Server.Services.BusinessLogic
{
    public interface ICampaign
    {
        Task<int> Create(CampaignCreationDto campaign);
        Task<CampaignCreationDto?> GetCreationDataAsync(int campaignId, int userId);
        Task<ActiveMapDto?> GetActiveCampaignElements(int campaignId);
        Task<bool?> UpdateActiveCampaignElements(ActiveMapDto activeMap);
    }

    public class Campaign : ICampaign
    {
        private readonly IRepository<DbActiveCampaignElements> _activeElementsRepository;
        private readonly IRepository<DbCampaign> _campaignRepository;
        private readonly IRepository<DbUser> _users;
        private readonly IRepository<DbUserInCampaign> _campaignUsersRepository;
        private readonly IRepository<DbDiceRoll> _rollsRepository;
        private readonly IUpdateNotifier _updateNotifier;

        public Campaign(
            IRepository<DbActiveCampaignElements> activeElementsRepository,
            IRepository<DbCampaign> campaignRepository,
            IRepository<DbUser> users,
            IRepository<DbUserInCampaign> userInCampaignRepository,
            IRepository<DbDiceRoll> rollsRepository,
            IUpdateNotifier updateNotifier)
        {
            _activeElementsRepository = activeElementsRepository;
            _campaignRepository = campaignRepository;
            _updateNotifier = updateNotifier;
            _rollsRepository = rollsRepository;
            _users = users;
            _campaignUsersRepository = userInCampaignRepository;
        }

        public async Task<int> Create(CampaignCreationDto campaign)
        {
            var dbCampaign = new DbCampaign() { Name = campaign.CampaignName };
            await _campaignRepository.AddAsync(dbCampaign);

            await _campaignUsersRepository.AddAsync(new()
            {
                UserId = campaign.Gamemaster.Id,
                CampaignId = dbCampaign.Id,
                IsGamemaster = true
            });

            await _activeElementsRepository.AddAsync(new()
            {
                CampaignId = dbCampaign.Id,
                MapId = -1,
                AmbientId = -1,
                EffectId = -1,
            });

            await _rollsRepository.AddAsync(new()
            {
                CampaignId = dbCampaign.Id,
                Roll = JsonSerializer.Serialize(new DiceRollResultDto(string.Empty, Array.Empty<bool>()))
            });

            foreach (var user in campaign.UsersInCampaign)
            {
                await _campaignUsersRepository.AddAsync(new()
                {
                    UserId = user.Id,
                    CampaignId = dbCampaign.Id,
                    IsGamemaster = false
                });
            }

            return dbCampaign.Id;
        }

        public async Task<ActiveMapDto?> GetActiveCampaignElements(int campaignId)
        {
            var activeCampaignElements = await _activeElementsRepository.FirstAsync(x => x.CampaignId == campaignId);

            if (activeCampaignElements is null)
            {
                return null;
            }

            return new ActiveMapDto(activeCampaignElements.CampaignId, activeCampaignElements.MapId);
        }

        public async Task<CampaignCreationDto?> GetCreationDataAsync(int campaignId, int userId)
        {
            var users = _users.Select(x => new UsersDto(x.Id, x.Username, x.Email));

            if (campaignId == -1) // New campaign
            {
                var gamemaster = await _users.FirstAsync(x => x.Id == userId);

                if (gamemaster is null)
                {
                    return null;
                }

                return new CampaignCreationDto(
                    CampaignId: campaignId,
                    CampaignName: string.Empty,
                    Gamemaster: new UsersDto(gamemaster.Id, gamemaster.Username, gamemaster.Email),
                    UsersNotInCampaign: await users.ToListAsync().ConfigureAwait(false),
                    UsersInCampaign: new List<UsersDto>());
            }
            else // Existing campaign
            {
                var dbCampaign = await _campaignRepository.FirstAsync(x => x.Id == campaignId).ConfigureAwait(false);

                if (dbCampaign is null)
                {
                    return null;
                }

                var dbUserInCampaign = _campaignUsersRepository.Where(x => x.CampaignId == campaignId);

                var gamemasterUser = await dbUserInCampaign.FirstAsync(x => x.IsGamemaster);

                if (gamemasterUser is null)
                {
                    return null;
                }

                var gamemaster = await _users.FirstAsync(x => x.Id == gamemasterUser.UserId);

                if (gamemaster is null)
                {
                    return null;
                }

                var usersInCampaign = _users.Where(x => dbUserInCampaign.Any(y => x.Id == y.UserId));
                var usersNotInCampaign = _users.Where(x => !usersInCampaign.Any(y => x.Id == y.Id));

                return new CampaignCreationDto(
                    CampaignId: campaignId,
                    CampaignName: dbCampaign.Name,
                    Gamemaster: new UsersDto(gamemaster.Id, gamemaster.Username, gamemaster.Email),
                    UsersNotInCampaign: await usersInCampaign.Select(x => new UsersDto(x.Id, x.Username, x.Email)).ToListAsync(),
                    UsersInCampaign: await usersNotInCampaign.Select(x => new UsersDto(x.Id, x.Username, x.Email)).ToListAsync()
                );
            }
        }

        public async Task<bool?> UpdateActiveCampaignElements(ActiveMapDto activeMap)
        {
            var activeCampaignElements = await _activeElementsRepository.FirstAsync(x => x.CampaignId == activeMap.CampaignId);

            if (activeCampaignElements is null)
            {
                return null;
            }

            if (activeCampaignElements.MapId == activeMap.MapId)
            {
                return false;
            }

            activeCampaignElements.MapId = activeMap.MapId;
            await _activeElementsRepository.UpdateAsync(activeCampaignElements);
            await _updateNotifier.Send(activeMap.CampaignId, UpdateEntity.Map);

            return true;
        }
    }
}
