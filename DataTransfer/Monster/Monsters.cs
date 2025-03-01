namespace DataTransfer.Monster;

public record MonstersDto(
    int Id,
    string Name,
    byte[] Image,
    SizeCategory SizeCategory,
    string Type,
    string Alignment,
    double ChallengeRating);