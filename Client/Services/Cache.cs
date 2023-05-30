using System;

namespace Client.Services
{
    public interface ICache
    {
        bool Add(string filename, byte[] data);
        bool Exists(string filename);
        byte[] Get(string filename);
    }

    public class Cache : ICache
    {
        public Cache()
        {
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        public bool Add(string filename, byte[] data)
        {
            throw new NotImplementedException();
        }

        public bool Exists(string filename)
        {
            throw new NotImplementedException();
        }

        public byte[] Get(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
