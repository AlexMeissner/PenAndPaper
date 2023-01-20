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

    public class RollDiceDto
    {
        public int CampaignId { get; set; }
        public int PlayerId { get; set; }
        public Dice Dice { get; set; }
    }
}