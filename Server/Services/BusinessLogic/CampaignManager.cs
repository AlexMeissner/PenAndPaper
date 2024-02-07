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
    public interface ICampaignManager
    {
        Task<int> Create(CampaignCreationDto campaign);
        Task<CampaignCreationDto?> GetCreationDataAsync(int campaignId, int userId);
        Task<ActiveMapDto?> GetActiveCampaignElements(int campaignId);
        Task<bool?> UpdateActiveCampaignElements(ActiveMapDto activeMap);
    }

    public class CampaignManager(IDatabaseContext dbContext, IRepository<Campaign> campaignRepository, IRepository<User> userRepository, IUpdateNotifier updateNotifier) : ICampaignManager
    {
        public async Task<int> Create(CampaignCreationDto campaign)
        {
            var emptyRoll = JsonSerializer.Serialize(new DiceRollResultDto(string.Empty, Array.Empty<bool>()));

            var gamemaster = await userRepository.FindAsync(campaign.Gamemaster.Id);

            if (gamemaster is null)
            {
                return -1;
            }

            List<User> players = [];

            foreach (var user in campaign.UsersInCampaign)
            {
                var player = await userRepository.FindAsync(user.Id);

                if (player is null)
                {
                    return -1;
                }

                players.Add(player);
            }

            var dbCampaign = new Campaign()
            {
                Name = campaign.CampaignName,
                Roll = emptyRoll,
                Gamemaster = gamemaster,
                Players = players,
                Maps = []
            };

            await dbContext.AddAsync(dbCampaign);

            return dbCampaign.Id;
        }

        public async Task<ActiveMapDto?> GetActiveCampaignElements(int campaignId)
        {
            var campaign = await campaignRepository.Include(c => c.ActiveMap).FirstAsync(c => c.Id == campaignId);

            if (campaign is null)
            {
                return null;
            }

            return new ActiveMapDto(campaign.Id, campaign.ActiveMap?.Id);
        }

        public async Task<CampaignCreationDto?> GetCreationDataAsync(int campaignId, int userId)
        {
            if (campaignId == -1) // New campaign
            {
                var gamemaster = await userRepository.FindAsync(userId);

                if (gamemaster is null)
                {
                    return null;
                }

                var usersNotInCampaign = await userRepository
                    .Where(user => user != gamemaster)
                    .Select(x => new UsersDto(x.Id, x.Username, x.Email))
                    .ToListAsync()
                    .ConfigureAwait(false);

                return new CampaignCreationDto(
                    CampaignId: campaignId,
                    CampaignName: string.Empty,
                    Gamemaster: new UsersDto(gamemaster.Id, gamemaster.Username, gamemaster.Email),
                    UsersNotInCampaign: usersNotInCampaign,
                    UsersInCampaign: []);
            }
            else // Existing campaign
            {
                var campaign = await campaignRepository.Include(c => c.Players).Include(c => c.Gamemaster).FirstAsync(c => c.Id == campaignId).ConfigureAwait(false);

                if (campaign is null)
                {
                    return null;
                }

                var playersInCampaign = campaign.Players.Select(u => new UsersDto(u.Id, u.Username, u.Email)).ToList();
                var usersNotInCampaign = await userRepository
                    .Where(x => !playersInCampaign.Any(y => x.Id == y.Id) && x != campaign.Gamemaster)
                    .Select(x => new UsersDto(x.Id, x.Username, x.Email))
                    .ToListAsync();

                return new CampaignCreationDto(
                    CampaignId: campaignId,
                    CampaignName: campaign.Name,
                    Gamemaster: new UsersDto(campaign.Gamemaster.Id, campaign.Gamemaster.Username, campaign.Gamemaster.Email),
                    UsersNotInCampaign: playersInCampaign,
                    UsersInCampaign: usersNotInCampaign);
            }
        }

        public async Task<bool?> UpdateActiveCampaignElements(ActiveMapDto activeMap)
        {
            var camapign = await campaignRepository
                .Include(c => c.ActiveMap)
                .Include(c => c.Maps)
                .FirstAsync(c => c.Id == activeMap.CampaignId);

            if (camapign is null)
            {
                return null;
            }

            if (activeMap.MapId is int mapId)
            {
                var map = camapign.Maps.FirstOrDefault(m => m.Id == mapId);

                if (camapign.ActiveMap == map)
                {
                    return false;
                }

                camapign.ActiveMap = map;
            }
            else
            {
                camapign.ActiveMap = null;
            }

            await dbContext.UpdateAsync(camapign);
            await updateNotifier.Send(activeMap.CampaignId, UpdateEntity.Map);

            return true;
        }
    }
}
