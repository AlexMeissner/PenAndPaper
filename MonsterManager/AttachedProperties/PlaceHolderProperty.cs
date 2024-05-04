using System.Windows;

namespace MonsterManager.AttachedProperties
{
    internal static class PlaceholderProperty
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
