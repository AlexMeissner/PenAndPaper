namespace DataTransfer.Dice
{
    public class DiceRollDto
    {
        public string Name { get; set; }
        public List<bool> Succeeded { get; set; }
    }

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

    public class DiceRollResultDto
    {
        public string Name { get; set; }
        public List<bool> Succeeded { get; set; }
    }

    public class RollDiceDto
    {
        public int CampaignId { get; set; }
        public int PlayerId { get; set; }
        public Dice Dice { get; set; }
    }
}