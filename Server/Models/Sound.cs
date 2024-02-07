using DataTransfer.Sound;

namespace Server.Models
{
    public class Sound
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public SoundType Type { get; set; }
        public required string Tags { get; set; }
        public required byte[] Data { get; set; }

        #region Relationships
        public virtual Setting? DiceSuccessSetting { get; set; }
        public virtual Setting? DiceFailSetting { get; set; }
        public virtual Setting? DiceCritSuccessSetting { get; set; }
        public virtual Setting? DiceCritFailSetting { get; set; }
        public virtual ICollection<Campaign> AmbientInCampaigns { get; set; } = [];
        public virtual ICollection<Campaign> EffectInCampaigns { get; set; } = [];
        #endregion
    }
}
