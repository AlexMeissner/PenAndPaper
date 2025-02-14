namespace Backend.Database.Models;

public class Token
{
    public int Id { get; init; }
    public int X { get; set; }
    public int Y { get; set; }

    #region Relations

    public int MapId { get; init; }

    public int OwnerId { get; init; }
    public required User Owner { get; init; }

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