using System.IO;
using System.Threading.Tasks;

namespace Client.Converter
{
    public static class Mp3Converter
    {
        public static async Task<byte[]> FromFile(string filepath)
        {
            return await File.ReadAllBytesAsync(filepath);
        }

        public static async void ToFile(string filepath, byte[] data)
        {
            await File.WriteAllBytesAsync(filepath, data);
        }
    }
}
