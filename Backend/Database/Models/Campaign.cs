using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Database.Models;

public class Campaign
{
    public int Id { get; init; }
    [MaxLength(64)] public required string Name { get; set; }

    #region Relations

    public int GameMasterId { get; init; }

    [InverseProperty(nameof(User.GameMasterCampaigns))]
    public required User GameMaster { get; init; }

    public ICollection<Map> Maps { get; set; } = [];

    [InverseProperty(nameof(User.PlayerCampaigns))]
    public ICollection<User> Players { get; set; } = [];

    public ICollection<Character> Characters { get; set; } = [];

    #endregion
}