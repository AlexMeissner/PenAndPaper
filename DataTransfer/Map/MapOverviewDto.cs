namespace DataTransfer.Map
{
    public record MapOverviewItemDto(string Name, int MapId, byte[] ImageData)
    {
        public string ImageBase64 => Convert.ToBase64String(ImageData);
    }

    public record MapOverviewDto(ICollection<MapOverviewItemDto> Items);
}