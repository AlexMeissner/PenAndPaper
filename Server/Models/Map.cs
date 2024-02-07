using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Map
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required byte[] ImageData { get; set; }
        public bool GridIsActive { get; set; }
        public int GridSize { get; set; }
        public required string Script { get; set; }

        #region Relationships
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; } = default!;

        public int? ActiveCampaignId { get; set; }
        public virtual Campaign? ActiveCampaign { get; set; }

        [InverseProperty(nameof(Token.Map))]
        public virtual ICollection<Token> Tokens { get; set; } = [];
        #endregion
    }
}
