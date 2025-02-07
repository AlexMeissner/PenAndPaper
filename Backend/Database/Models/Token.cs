namespace Backend.Database.Models;

public class Token
{
    public int Id { get; init; }
    public int X { get; set; }
    public int Y { get; set; }

    #region Relations

    public int MapId { get; init; }
    public required Map Map { get; init; }

    #endregion
}

public class CharacterToken : Token
{
    #region Relations

    public required Character Character { get; init; }

    #endregion
}

public class MonsterToken : Token
{
    #region Relations

    public required Monster Monster { get; init; }

    #endregion
}