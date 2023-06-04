using System;
using System.IO;

namespace Client.Services
{
    public enum CacheType
    {
        AmbientSound,
        SoundEffect,
    }

    public interface ICache
    {
        bool Add(CacheType type, string filename, byte[] data);
        bool Contains(CacheType type, string filename);
        byte[] GetData(CacheType type, string filename);
        string GetPath(CacheType type, string filename);
    }

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

        public bool Add(CacheType type, string filename, byte[] data)
        {
            throw new NotImplementedException();
        }

        public bool Contains(CacheType type, string filename)
        {
            throw new NotImplementedException();
        }

        public byte[] GetData(CacheType type, string filename)
        {
            throw new NotImplementedException();
        }

        public string GetPath(CacheType type, string filename)
        {
            throw new NotImplementedException();
        }
    }
}
