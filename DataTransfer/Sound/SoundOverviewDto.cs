using System.Collections.ObjectModel;

namespace DataTransfer.Sound
{
    public class SoundOverviewItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SoundType Type { get; set; }
        public ICollection<string> Tags { get; set; }

        // TODO: We do not need this really, but there is an exception when removing it. Find better Fix!
        public SoundOverviewItemDto()
        {
            Name = string.Empty;
            Type = SoundType.Effect;
            Tags = new List<string>();
        }

        public SoundOverviewItemDto(int id, string name, SoundType type, string tags)
        {
            Id = id;
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
