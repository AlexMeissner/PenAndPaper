using System.Net;
using System.Windows;

namespace Client.View
{
    internal static class MessageBoxUtility
    {
        public static void Show(HttpStatusCode statusCode)
        {
            var message = string.Format("{0} ({1})", statusCode, (int)statusCode);
            MessageBox.Show(message, "Authentifizierungsfehler", MessageBoxButton.OK);
        }
    }
}
