using Backend.Database.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Tokens;

public class Token
{
    public int Id { get; init; }
    public int X { get; set; }
    public int Y { get; set; }
    public uint? Initiative { get; set; }

    #region Relations

    public int MapId { get; init; }
    public required Map Map { get; init; }

    [InverseProperty(nameof(Map.ActingToken))]
    public Map? ActingOnMap { get; set; }

    public int OwnerId { get; init; }

    #endregion
}

public class CharacterToken : Token
{
    #region Relations

    public int CharacterId { get; init; }
    public required Character Character { get; init; }

    #endregion
}

public class MonsterToken : Token
{
    #region Relations

    public int MonsterId { get; init; }
    public required Monster Monster { get; init; }

    #endregion
}