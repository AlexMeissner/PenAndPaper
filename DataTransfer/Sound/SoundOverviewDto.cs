using System.Collections.ObjectModel;

namespace DataTransfer.Sound
{
    public class SoundOverviewItemDto
    {
        public string Name { get; set; }
        public SoundType Type { get; set; }
        public ICollection<string> Tags { get; set; }

        public SoundOverviewItemDto()
        {
            Name = "Test";
            Type = SoundType.Effect;
            Tags = new List<string>() { "Fatal" };
        }

        public SoundOverviewItemDto(string name, SoundType type, string tags)
        {
            Name = name;
            Type = type;
            Tags = tags.Split(';');
        }
    }

    public class SoundOverviewDto : PropertyChangedNotifier
    {
        public ICollection<SoundOverviewItemDto> Items { get; set; } = new ObservableCollection<SoundOverviewItemDto>();
    }
}
