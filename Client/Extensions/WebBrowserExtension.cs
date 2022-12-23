using System.Windows;
using System.Windows.Controls;

namespace Client.Extensions
{
    public class WebBrowserExtension
    {
        public static readonly DependencyProperty BodyProperty = DependencyProperty.RegisterAttached("Body", typeof(string), typeof(WebBrowserExtension), new PropertyMetadata(OnBodyChanged));

        public static string GetBody(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(BodyProperty);
        }

        public static void SetBody(DependencyObject dependencyObject, string body)
        {
            dependencyObject.SetValue(BodyProperty, body);
        }

        private static void OnBodyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = (WebBrowser)dependencyObject;

            if (e.NewValue is string markdown && markdown.Length > 0)
            {
                webBrowser.NavigateToString(markdown);
            }
        }
    }

}