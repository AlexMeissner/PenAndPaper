namespace DataTransfer
{
    public static class Checksum
    {
        public static string CreateHash(byte[] data)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var hash = md5.ComputeHash(data);
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}
