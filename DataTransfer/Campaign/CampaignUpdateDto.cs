namespace DataTransfer.Campaign
{
    public record CampaignUpdateDto(
        int CampaignId,
        long MapChange,
        long MapCollectionChange,
        long TokenChange,
        long DiceRoll,
        long AmbientSoundChange,
        long SoundEffectChange);
}