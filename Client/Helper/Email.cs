using System.Net.Mail;

namespace Client.Helper
{
    public static class Email
    {
        public static bool IsValid(string email)
        {
            if (email.EndsWith("."))
            {
                return false;
            }

            try
            {
                return new MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}