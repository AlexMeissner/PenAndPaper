using DataTransfer.Script;
using System.Windows.Controls;
using System.Windows;

namespace Client.Controls
{
    public partial class ScriptControl : UserControl
    {
        public ScriptDto Script { get; set; }
        private string UneditedText = string.Empty;

        public ScriptControl()
        {
            Script = new();
            InitializeComponent();
        }

        private void OnEdit(object sender, RoutedEventArgs e)
        {
            UneditedText = Script.Markdown;
            MarkdownEdit.Visibility = Visibility.Visible;
            MarkdownRender.Visibility = Visibility.Collapsed;
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Script.Markdown = UneditedText;
            MarkdownEdit.Visibility = Visibility.Collapsed;
            MarkdownRender.Visibility = Visibility.Visible;
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            MarkdownEdit.Visibility = Visibility.Collapsed;
            MarkdownRender.Visibility = Visibility.Visible;
        }
    }
}