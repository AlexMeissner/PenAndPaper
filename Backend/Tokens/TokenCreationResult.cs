namespace Backend.Tokens;

public record TokenCreationResult(int TokenId, int OwnerId, int X, int Y, byte[] Image);
