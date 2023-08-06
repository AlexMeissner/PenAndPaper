namespace DataTransfer.Sound
{
    public record SoundCreationDto(string Name, SoundType Type, ICollection<string> Tags, byte[] Data);
}
