namespace DataTransfer.Map
{
    public record MapOverviewItemDto(string Name, int MapId, byte[] ImageData);

    public record MapOverviewDto(ICollection<MapOverviewItemDto> Items);
}