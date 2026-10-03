namespace Backend.Authentication
{
    public sealed class ReturnUrlOptions
    {
        public const string SectionName = "Authentication:ReturnUrl";

        /// <summary>
        /// Additional origins (besides the application's own origin) that the login may redirect back to,
        /// e.g. the Vite development server "https://localhost:57396".
        /// </summary>
        public string[] AllowedOrigins { get; set; } = [];
    }
}
