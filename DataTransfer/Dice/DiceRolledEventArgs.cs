namespace DataTransfer.Dice;

public record DiceRolledEventArgs(string Name, IEnumerable<bool> Successes);
