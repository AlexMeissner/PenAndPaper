namespace DataTransfer.Sound
{
    public class SoundDataDto
    {
        public byte[] Data { get; set; }

        public SoundDataDto(byte[] data)
        {
            Data = data;
        }
    }
}