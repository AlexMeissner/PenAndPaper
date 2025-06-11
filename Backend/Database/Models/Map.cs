using Backend.Tokens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Models;

public class Map
{
    public int Id { get; init; }
    public bool IsActive { get; set; }
    [MaxLength(64)] public required string Name { get; set; }
    public required byte[] Image { get; set; }
    public required bool IsGridActive { get; set; }
    public required int GridSize { get; set; }
    public required string Script { get; set; }

    #region Relations

    public int CampaignId { get; init; }

    public int? ActingTokenId { get; set; }
    [ForeignKey(nameof(ActingTokenId))]
    public Token? ActingToken { get; set; }

    public ICollection<Token> Tokens { get; set; } = [];

    #endregion
}