using DataTransfer.Map;
using DataTransfer.WebSocket;
using Server.Models;

namespace Server.Services.BusinessLogic
{
    public interface ICampaign
    {
        Task<ActiveMapDto?> GetActiveCampaignElements(int campaignId);
        Task<bool?> UpdateActiveCampaignElements(ActiveMapDto activeMap);
    }

    public class Campaign : ICampaign
    {
        private readonly IRepository<DbActiveCampaignElements> _activeElementsRepository;
        private readonly IUpdateNotifier _updateNotifier;

        public Campaign(IRepository<DbActiveCampaignElements> activeElementsRepository, IUpdateNotifier updateNotifier)
        {
            _activeElementsRepository = activeElementsRepository;
            _updateNotifier = updateNotifier;
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
            await _activeElementsRepository.Update(activeCampaignElements);
            await _updateNotifier.Send(activeMap.CampaignId, UpdateEntity.Map);

            return true;
        }
    }
}
