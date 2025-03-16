using System.ComponentModel.DataAnnotations;

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

    public ICollection<Token> Tokens { get; set; } = [];

    #endregion
}