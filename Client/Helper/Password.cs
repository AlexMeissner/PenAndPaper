using System.Security.Cryptography;
using System.Text;

namespace Client.Helper
{
    public static class Password
    {
        public static string Encrypt(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = SHA256.Create().ComputeHash(data);
            return Encoding.ASCII.GetString(data);
        }
    }
}