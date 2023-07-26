using System;
using System.ComponentModel;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface ISessionData : INotifyPropertyChanged
    {
        public int UserId { get; set; }
        public int CampaignId { get; set; }
    }

    [SingletonService]
    public class SessionData : ISessionData
    {
        private int _userId = -1;
        public int UserId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        private int? _campaignId;
        public int CampaignId
        {
            get
            {
                if (_campaignId is null)
                {
                    throw new NullReferenceException("Campaign id not set");
                }

                return (int)_campaignId;
            }
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