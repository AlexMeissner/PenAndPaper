using DataTransfer.Sound;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Client.Windows
{
    public partial class SoundCreationWindow : Window
    {
        public SoundCreationDto CreationData { get; set; }

        public SoundCreationWindow(SoundType type)
        {
            CreationData = new(string.Empty, type, new ObservableCollection<string>(), Array.Empty<byte>());
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

        private async void OnUploadFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "MP3 (*.mp3)|*.mp3"
            };

            if (openFileDialog.ShowDialog() is true)
            {
                CreationData.Data = await File.ReadAllBytesAsync(openFileDialog.FileName);
            }
        }
    }
}
