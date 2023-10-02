using DataTransfer.Sound;
using DataTransfer.WebSocket;
using Server.Models;

namespace Server.Services.BusinessLogic
{
    public interface ISound
    {
        Task<ActiveAmbientSoundDto?> GetActiveAmbientSound(int campaignId);
        Task<ActiveSoundEffectDto?> GetActiveSoundEffect(int campaignId);
        Task<bool?> PlayAmbient(ActiveAmbientSoundDto sound);
        Task<bool?> PlayEffect(ActiveSoundEffectDto sound);
    }

    public class Sound : ISound
    {
        private readonly IRepository<DbActiveCampaignElements> _activeElementsRepository;
        private readonly IUpdateNotifier _updateNotifier;

        public Sound(IRepository<DbActiveCampaignElements> activeElementsRepository, IUpdateNotifier updateNotifier)
        {
            _activeElementsRepository = activeElementsRepository;
            _updateNotifier = updateNotifier;
        }

        public async Task<ActiveAmbientSoundDto?> GetActiveAmbientSound(int campaignId)
        {
            var activeCampaignElements = await _activeElementsRepository.FirstAsync(x => x.CampaignId == campaignId);

            if (activeCampaignElements is null)
            {
                return null;
            }

            return new ActiveAmbientSoundDto(activeCampaignElements.CampaignId, activeCampaignElements.AmbientId);
        }

        public async Task<ActiveSoundEffectDto?> GetActiveSoundEffect(int campaignId)
        {
            var activeCampaignElements = await _activeElementsRepository.FirstAsync(x => x.CampaignId == campaignId);

            if (activeCampaignElements is null)
            {
                return null;
            }

            return new ActiveSoundEffectDto(activeCampaignElements.CampaignId, activeCampaignElements.EffectId);
        }

        public async Task<bool?> PlayAmbient(ActiveAmbientSoundDto sound)
        {
            var activeCampaignElements = await _activeElementsRepository.FirstAsync(x => x.CampaignId == sound.CampaignId);

            if (activeCampaignElements is null)
            {
                return null;
            }

            if (activeCampaignElements.AmbientId == sound.AmbientId)
            {
                return false;
            }

            activeCampaignElements.AmbientId = sound.AmbientId;
            await _activeElementsRepository.Update(activeCampaignElements);

            await _updateNotifier.Send(sound.CampaignId, UpdateEntity.AmbientSound);

            return true;
        }

        public async Task<bool?> PlayEffect(ActiveSoundEffectDto sound)
        {
            var activeCampaignElements = await _activeElementsRepository.FirstAsync(x => x.CampaignId == sound.CampaignId);

            if (activeCampaignElements is null)
            {
                return null;
            }

            if (activeCampaignElements.EffectId == sound.EffectId)
            {
                return false;
            }

            activeCampaignElements.EffectId = sound.EffectId;
            await _activeElementsRepository.Update(activeCampaignElements);

            await _updateNotifier.Send(sound.CampaignId, UpdateEntity.SoundEffect);

            return true;
        }
    }
}
