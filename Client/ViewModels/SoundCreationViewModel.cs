using DataTransfer.Sound;
using System;
using System.Collections.ObjectModel;

namespace Client.ViewModels
{
    public class SoundCreationViewModel : BaseViewModel
    {
        public string Name;
        public SoundType Type;
        public ObservableCollection<string> Tags;
        public byte[] Data;

        public SoundCreationViewModel(SoundType type)
        {
            Name = string.Empty;
            Type = type;
            Tags = new();
            Data = Array.Empty<byte>();
        }
    }
}
