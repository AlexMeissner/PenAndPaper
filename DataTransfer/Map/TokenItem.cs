using System.Collections.ObjectModel;

namespace DataTransfer.Map
{
    public class TokenCreationDto
    {
        public int CampaignId { get; set; }
        public int MapId { get; set; }
        public int? CharacterId { get; set; }
        public int? MonsterId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class TokenItem
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }

    public class TokensDto
    {
        public ICollection<TokenItem> Items { get; set; } = new ObservableCollection<TokenItem>();
    }
}