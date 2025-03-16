using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Models;

public class User
{
    public int Id { get; init; }
    [MaxLength(64)] public required string Email { get; init; }
    [MaxLength(32)] public required string Username { get; set; }

    #region Relations

    public ICollection<Character> Characters { get; set; } = [];

    [InverseProperty(nameof(Campaign.GameMaster))]
    public ICollection<Campaign> GameMasterCampaigns { get; set; } = [];

    [InverseProperty(nameof(Campaign.Players))]
    public ICollection<Campaign> PlayerCampaigns { get; set; } = [];

    #endregion
}