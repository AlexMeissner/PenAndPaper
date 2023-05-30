namespace DataTransfer.Sound
{
    public class SoundCreationDto
    {
        public string Name { get; set; }
        public SoundType Type { get; set; }
        public ICollection<string> Tags { get; set; }
        public byte[] Data { get; set; }

        public SoundCreationDto(string name, SoundType type, ICollection<string> tags, byte[] data)
        {
            Name = name;
            Type = type;
            Tags = tags;
            Data = data;
        }
    }
}
