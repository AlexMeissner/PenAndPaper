namespace DataTransfer.Sound
{
    public class SoundDto
    {
        public int Id { get; set; }
        public string Checksum { get; set; }

        public SoundDto(int id, string checksum)
        {
            Id = id;
            Checksum = checksum;
        }
    }
}