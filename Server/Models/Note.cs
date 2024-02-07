using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Note
    {
        public int Id { get; set; }
        public bool IsPublic { get; set; }
        public required string Text { get; set; }

        #region Relationships
        [InverseProperty(nameof(User.Notes))]
        public virtual User User { get; set; } = default!;
        #endregion
    }

    public class CharacterNote : Note
    {
        #region Relationships
        [InverseProperty(nameof(Character.Notes))]
        public virtual Character Character { get; set; } = default!;
        #endregion
    }
}
