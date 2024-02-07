using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Token
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        #region Relationships
        public int MapId { get; set; }
        public virtual Map Map { get; set; } = default!;
        #endregion
    }

    public class CharacterToken : Token
    {
        #region Relationships
        [InverseProperty(nameof(Character.Tokens))]
        public virtual Character Character { get; set; } = default!;
        #endregion
    }

    public class MonsterToken : Token
    {
        #region Relationships
        [InverseProperty(nameof(Monster.Tokens))]
        public virtual Monster Monster { get; set; } = default!;
        #endregion
    }
}
