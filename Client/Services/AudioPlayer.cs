using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Client.Services
{
    // Cache sound files on client side to reduce data which needs to be sent
    public interface IAudioPlayer
    {
        void Pause();
        Task Play(byte[] data);
        Task Play(string filepath);
        Task Resume();
        void Stop();
    }

    public class AudioPlayer : IAudioPlayer
    {
        private SoundPlayer? _soundPlayer;
        private long _pause;

        public void Pause()
        {
            _pause = _soundPlayer?.Stream.Position ?? 0;
            _soundPlayer?.Stop();
        }

        public Task Play(byte[] data)
        {
            _pause = 0;

            return Task.Run(() =>
            {
                var ms = new MemoryStream(data);
                _soundPlayer = new(ms);                
                //_soundPlayer.Stream.Seek(0, SeekOrigin.Begin);
                //_soundPlayer.Stream.Write(data, 0, data.Length);
                _soundPlayer.Play();
            });
        }

        public async Task Play(string filepath)
        {
            //var data = await File.ReadAllBytesAsync(filepath);
            //await Play(data);

            //var data = await File.ReadAllBytesAsync(filepath);
            //var s = new MemoryStream(data);
            //var player = new MediaPlayer();
            //player.Source = MediaSource.
            //IRandomAccessStream ras = s.As
            //player.Open(new Uri(filepath));
            //player.Play();
            //player.Volume = 0.9;

            //var s = new SoundPlayer(filepath);
            //s.Play();

            //var ms = new MemoryStream(data, true);
            //var a = new SoundPlayer(ms);
            //a.Play();
        }

        public Task Resume()
        {
            return Task.Run(() =>
            {
                _soundPlayer.Stream.Seek(_pause, SeekOrigin.Begin);
                _soundPlayer.Play();
            });
        }

        public void Stop()
        {
            _pause = 0;
            _soundPlayer.Stop();
        }
    }
}