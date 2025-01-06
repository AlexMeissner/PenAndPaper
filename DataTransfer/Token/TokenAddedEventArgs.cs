namespace DataTransfer.Token;

public record TokenAddedEventArgs(int Id, int UserId, byte[] Image, int X, int Y);