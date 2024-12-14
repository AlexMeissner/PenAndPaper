namespace DataTransfer.Token;

public record TokenAddedEventArgs(int Id, byte[] Image, int X, int Y);