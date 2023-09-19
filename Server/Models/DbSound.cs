using DataTransfer.Sound;

namespace Server.Models
{
    public class DbSound
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SoundType Type { get; set; }
        public string Tags { get; set; }
        public byte[] Data { get; set; }
    }
}
