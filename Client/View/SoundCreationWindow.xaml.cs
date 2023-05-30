using DataTransfer.Sound;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Client.View
{
    public partial class SoundCreationWindow : Window
    {
        public SoundCreationDto CreationData { get; set; } = new SoundCreationDto(string.Empty, SoundType.Ambient, new ObservableCollection<string>(), Array.Empty<byte>());

        public static IEnumerable<SoundType> SoundTypeValues => Enum.GetValues(typeof(SoundType)).Cast<SoundType>();

        public SoundCreationWindow()
        {
            InitializeComponent();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OnCreate(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnAddTag(object sender, RoutedEventArgs e)
        {
            if (TagTextBox.Text.Length > 0 && !CreationData.Tags.Any(x => x.ToLower() == TagTextBox.Text.ToLower()))
            {
                CreationData.Tags.Add(TagTextBox.Text);
                TagTextBox.Text = string.Empty;
            }
        }

        private void OnDeleteTag(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is string tag)
            {
                CreationData.Tags.Remove(tag);
            }
        }

        private void OnUploadFile(object sender, RoutedEventArgs e)
        {

        }
    }
}
