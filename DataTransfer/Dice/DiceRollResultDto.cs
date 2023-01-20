namespace DataTransfer.Dice
{
    public class DiceRollResultDto
    {
        public string Name { get; set; }
        public List<bool> Succeeded { get; set; }
    }
}