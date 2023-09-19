namespace DungeonsAndDragons5e.Rule
{
    public sealed class Spell
    {
        public string Name { get; set; } = default!;
        public SpellSchool SpellSchool { get; set; }
        public int Level { get; set; }
        public bool IsRitual { get; set; }
        public string Range { get; set; } = default!;
        public string CastTime { get; set; } = default!;
        public string? Duration { get; set; }
        public List<SpellComponent> Components { get; set; } = default!;
        public string? Material { get; set; }
        public string Description { get; set; } = default!;
    }
    // TODO: 262 - 265 fehlen
}