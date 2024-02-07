using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Roll { get; set; }

        #region Relationships
        [InverseProperty(nameof(User.GamemasterCampaigns))]
        public virtual User Gamemaster { get; set; } = default!;

        [InverseProperty(nameof(User.PlayerCampaigns))]
        public virtual ICollection<User> Players { get; set; } = [];

        [InverseProperty(nameof(Character.Campaign))]
        public virtual ICollection<Character> Characters { get; set; } = [];

        [InverseProperty(nameof(Map.Campaign))]
        public virtual ICollection<Map> Maps { get; set; } = [];

        [InverseProperty(nameof(Map.ActiveCampaign))]
        public virtual Map? ActiveMap { get; set; }

        public virtual int? ActiveEffectId { get; set; }
        [InverseProperty(nameof(Sound.EffectInCampaigns))]
        public virtual Sound? ActiveEffect { get; set; }

        public virtual int? ActiveAmbientId { get; set; }
        [InverseProperty(nameof(Sound.AmbientInCampaigns))]
        public virtual Sound? ActiveAmbient { get; set; }
        #endregion
    }
}
