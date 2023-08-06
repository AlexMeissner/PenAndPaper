namespace DataTransfer.Dice
{
    public record DiceRollResultDto(string Name, IList<bool> Succeeded);
}
