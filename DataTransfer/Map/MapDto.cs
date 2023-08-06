namespace DataTransfer.Map
{
    public record GridDto(int Size, bool IsActive);

    public record MapDto(int Id, int CampaignId, string Name, byte[] ImageData, GridDto Grid);
}