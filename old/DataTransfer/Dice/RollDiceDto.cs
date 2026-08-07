namespace DataTransfer.Dice
{
    public enum Dice
    {
        D4,
        D6,
        D8,
        D10,
        D12,
        D20,
        D100
    }

    public record RollDiceDto(Dice Dice, bool IsPrivate);
}