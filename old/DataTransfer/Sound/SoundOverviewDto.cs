namespace DataTransfer.Sound
{
    public record SoundOverviewItemDto(int Id, string Name, SoundType Type, ICollection<string> Tags);

    public record SoundOverviewDto(ICollection<SoundOverviewItemDto> Items);
}
