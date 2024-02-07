using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Setting
    {
        public int Id { get; set; }

        #region Relationships
        public virtual int? DiceSuccessSoundId { get; set; }
        [InverseProperty(nameof(Sound.DiceSuccessSetting))]
        public virtual Sound? DiceSuccessSound { get; set; }

        public virtual int? DiceFailSoundId { get; set; }
        [InverseProperty(nameof(Sound.DiceFailSetting))]
        public virtual Sound? DiceFailSound { get; set; }

        public virtual int? DiceCritSuccessSoundId { get; set; }
        [InverseProperty(nameof(Sound.DiceCritSuccessSetting))]
        public virtual Sound? DiceCritSuccessSound { get; set; }

        public virtual int? DiceCritFailSoundId { get; set; }
        [InverseProperty(nameof(Sound.DiceCritFailSetting))]
        public virtual Sound? DiceCritFailSound { get; set; }
        #endregion
    }
}
