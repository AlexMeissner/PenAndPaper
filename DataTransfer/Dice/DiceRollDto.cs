namespace DataTransfer.Dice
{
    public class DiceRollDto
    {
        public string Name { get; set; }
        public List<bool> Succeeded { get; set; }
    }
}