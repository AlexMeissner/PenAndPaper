namespace DataTransfer.Map
{
    public record TokenCreationDto(int CampaignId, int MapId, int? CharacterId, int? MonsterId, int X, int Y);

    public record TokenUpdateDto(int CampaignId, int Id, int X, int Y);

    public record TokenItem(int Id, int UserId, int X, int Y, string Name, byte[] Image);

    public record TokensDto(ICollection<TokenItem> Items);
}