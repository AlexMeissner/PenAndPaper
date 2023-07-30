using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Client.Helper
{
    public static class RandomBackgroundImage
    {
        public static ImageSource GetImageFromResource()
        {
            int prefix = new Random().Next(1, 7);
            string resourceName = string.Format("Resource/splash{0}.jpg", prefix);
            Uri uri = new("pack://application:,,,/Client;component/" + resourceName, UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(uri);
        }
    }
}