using DataTransfer.Sound;

namespace Server.Models
{
    public class DbSound
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public SoundType Type { get; set; }
        public string Tags { get; set; } = default!;
        public byte[] Data { get; set; } = default!;
    }
}
