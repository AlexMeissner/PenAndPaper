using System.Windows;

namespace Client.AttachedProperties
{
    public static class PlaceholderProperty
    {
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.RegisterAttached("PlaceholderText", typeof(string), typeof(PlaceholderProperty), new PropertyMetadata(null));

        public static string GetPlaceholderText(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderTextProperty);
        }

        public static void SetPlaceholderText(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderTextProperty, value);
        }
    }
}
