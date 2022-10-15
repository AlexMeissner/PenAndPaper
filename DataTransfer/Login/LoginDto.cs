namespace DataTransfer.Login
{
    public enum LoginState
    {
        Success,
        UnknownEmail,
        InvalidPassword
    }

    public class LoginDto
    {
        public LoginState State { get; set; }
        public int? UserId { get; set; }
    }
}