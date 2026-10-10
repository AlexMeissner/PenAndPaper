namespace Backend.Login;

public sealed class ReturnUrlOptions
{
    public const string SectionName = "Authentication:ReturnUrl";

    public string[] AllowedOrigins { get; set; } = [];
}
