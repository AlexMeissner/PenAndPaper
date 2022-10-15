using System.ComponentModel;

namespace Client.Services
{
    public interface ISessionData : INotifyPropertyChanged
    {
        public int? UserId { get; set; }
        public int? CampaignId { get; set; }
    }

    public class SessionData : ISessionData
    {
        private int? _userId;
        public int? UserId
        {
            get => _userId;
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        private int? _campaignId;
        public int? CampaignId
        {
            get => _campaignId;
            set
            {
                _campaignId = value;
                OnPropertyChanged(nameof(CampaignId));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}