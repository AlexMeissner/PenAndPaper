namespace Website.Database.Models;

public class Audio
{
    public required string Id { get; init; }
    public required byte[] Data { get; set; }
}
