using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Models;

public class Character
{
    public int Id { get; init; }
    [MaxLength(32)] public required string Name { get; set; }
    public required byte[] Image { get; set; }

    #region Relations

    public int UserId { get; init; }
    public required User User { get; init; }
    public int CampaignId { get; init; }
    public required Campaign Campaign { get; init; }
    public ICollection<CharacterToken> Tokens { get; set; } = [];

    #endregion
}