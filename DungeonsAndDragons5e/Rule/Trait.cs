namespace DungeonsAndDragons5e.Rule
{
    public sealed class Trait
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public Trait(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}