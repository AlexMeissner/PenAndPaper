namespace DungeonsAndDragons5e.Rule
{
    public sealed class Spell
    {
        public string Name { get; set; }
        public SpellSchool SpellSchool { get; set; }
        public int Level { get; set; }
        public bool IsRitual { get; set; }
        public string Range { get; set; }
        public string CastTime { get; set; }
        public string? Duration { get; set; }
        public List<SpellComponent> Components { get; set; }
        public string? Material { get; set; }
        public string Description { get; set; }
    }
    // TODO: 262 - 265 fehlen
}