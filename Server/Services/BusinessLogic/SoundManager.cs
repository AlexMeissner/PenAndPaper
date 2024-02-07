using DataTransfer.Sound;
using DataTransfer.WebSocket;
using Server.Models;

namespace Server.Services.BusinessLogic
{
    public interface ISoundManager
    {
        Task<ActiveAmbientSoundDto?> GetActiveAmbientSound(int campaignId);
        Task<ActiveSoundEffectDto?> GetActiveSoundEffect(int campaignId);
        Task<bool> PlayAmbient(ActiveAmbientSoundDto sound);
        Task<bool> PlayEffect(ActiveSoundEffectDto sound);
    }

    public class SoundManager(IDatabaseContext dbContext, IRepository<Campaign> campaignRepository, IUpdateNotifier updateNotifier) : ISoundManager
    {
        public async Task<ActiveAmbientSoundDto?> GetActiveAmbientSound(int campaignId)
        {
            var campaign = await campaignRepository.FindAsync(campaignId);

            if (campaign is null)
            {
                return null;
            }

            return new ActiveAmbientSoundDto(campaign.Id, campaign.ActiveAmbientId);
        }

        public async Task<ActiveSoundEffectDto?> GetActiveSoundEffect(int campaignId)
        {
            var campaign = await campaignRepository.FindAsync(campaignId);

            if (campaign is null)
            {
                return null;
            }

            return new ActiveSoundEffectDto(campaign.Id, campaign.ActiveEffectId);
        }

        public async Task<bool> PlayAmbient(ActiveAmbientSoundDto sound)
        {
            var campaign = await campaignRepository.FindAsync(sound.CampaignId);

            if (campaign is null)
            {
                return false;
            }

            campaign.ActiveAmbientId = sound.AmbientId;
            await dbContext.UpdateAsync(campaign);

            await updateNotifier.Send(sound.CampaignId, UpdateEntity.AmbientSound);

            return true;
        }

        public async Task<bool> PlayEffect(ActiveSoundEffectDto sound)
        {
            var campaign = await campaignRepository.FindAsync(sound.CampaignId);

            if (campaign is null)
            {
                return false;
            }

            campaign.ActiveEffectId = sound.EffectId;
            await dbContext.UpdateAsync(campaign);

            await updateNotifier.Send(sound.CampaignId, UpdateEntity.SoundEffect);

            return true;
        }
    }
}
