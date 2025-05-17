namespace DataTransfer.User;

public record UserProperties(int Id, string Username, string Color);
public record UserPropertiesUpdate(string Username, string Color);
