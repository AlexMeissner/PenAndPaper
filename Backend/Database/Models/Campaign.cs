using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Models;

public class Campaign
{
    public int Id { get; init; }
    [MaxLength(64)] public required string Name { get; set; }

    #region Relations

    public int GameMasterId { get; init; }
    public required User GameMaster { get; init; }

    public int? ActiveMapId { get; set; }
    public Map? ActiveMap { get; set; }

    public ICollection<Map> Maps { get; set; } = [];

    public ICollection<User> Players { get; set; } = [];

    public ICollection<Character> Characters { get; set; } = [];

    #endregion
}