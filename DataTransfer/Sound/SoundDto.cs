namespace DataTransfer.Sound
{
    public class SoundDto
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }

        public SoundDto(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }
    }
}