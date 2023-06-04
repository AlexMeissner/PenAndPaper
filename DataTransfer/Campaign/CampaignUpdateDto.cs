namespace DataTransfer.Campaign
{
    public class CampaignUpdateDto
    {
        public int CampaignId { get; set; }
        public long MapChange { get; set; }
        public long MapCollectionChange { get; set; }
        public long TokenChange { get; set; }
        public long DiceRoll { get; set; }
        public long AmbientSoundChange { get; set; }
        public long SoundEffectChange { get; set; }
    }
}