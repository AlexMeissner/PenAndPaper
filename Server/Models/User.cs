using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }

        #region Relationships
        public virtual ICollection<Campaign> GamemasterCampaigns { get; set; } = [];

        public virtual ICollection<Campaign> PlayerCampaigns { get; set; } = [];

        public virtual ICollection<Note> Notes { get; set; } = [];

        [InverseProperty(nameof(Character.User))]
        public virtual ICollection<Character> Characters { get; set; } = [];
        #endregion
    }
}
