namespace DataTransfer.Token;

public record TokensDto(int TokenId, int UserId, int X, int Y, string Name, byte[] Image);