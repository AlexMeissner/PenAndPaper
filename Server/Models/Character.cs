using DataTransfer.Character;

namespace Server.Models
{
    public class Character
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public Race Race { get; set; }
        public Class Class { get; set; }
        public int ExperiencePoints { get; set; }
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public required byte[] Image { get; set; }

        #region Relationships
        public virtual User User { get; set; } = default!;

        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = default!;

        public virtual ICollection<CharacterToken> Tokens { get; set; } = [];

        public virtual ICollection<CharacterNote> Notes { get; set; } = [];
        #endregion
    }
}
