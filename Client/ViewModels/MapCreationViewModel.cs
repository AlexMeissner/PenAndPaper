using System;

namespace Client.ViewModels
{
    public class MapCreationViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public int GridSize { get; set; } = 100;
        public bool GridIsActive { get; set; } = false;
    }
}
