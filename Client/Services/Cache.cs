using System;
using System.IO;
using System.Threading.Tasks;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public enum CacheType
    {
        AmbientSound,
        SoundEffect,
    }

    public interface ICache
    {
        Task Add(CacheType type, string filename, byte[] data);
        bool Contains(CacheType type, string filename);
        Task<byte[]> GetData(CacheType type, string filename);
        string GetPath(CacheType type, string filename);
    }

    [TransistentService]
    public class Cache : ICache
    {
        private readonly string CacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PenAndPaper");

        public Cache()
        {
            if (!Directory.Exists(CacheDirectory))
            {
                Directory.CreateDirectory(CacheDirectory);
            }
        }

        public Task Add(CacheType type, string filename, byte[] data)
        {
            var filepath = GetPath(type, filename);
            var directory = Path.GetDirectoryName(filepath)!;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return File.WriteAllBytesAsync(filepath, data);
        }

        public bool Contains(CacheType type, string filename)
        {
            return File.Exists(GetPath(type, filename));
        }

        public Task<byte[]> GetData(CacheType type, string filename)
        {
            return File.ReadAllBytesAsync(GetPath(type, filename));
        }

        public string GetPath(CacheType type, string filename)
        {
            return Path.Combine(CacheDirectory, type.ToString(), filename);
        }
    }
}
