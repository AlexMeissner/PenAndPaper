using Client.Services.API;
using DataTransfer.Campaign;
using System;
using System.Timers;

namespace Client.Services
{
    public interface ICampaignUpdates
    {
        event EventHandler MapChanged;
        event EventHandler MapCollectionChanged;
        event EventHandler TokenChanged;
        event EventHandler DiceRolled;
        event EventHandler MusicChanged;
    }

    public class CampaignUpdates : ICampaignUpdates
    {
        public event EventHandler? MapChanged;
        public event EventHandler? MapCollectionChanged;
        public event EventHandler? TokenChanged;
        public event EventHandler? DiceRolled;
        public event EventHandler? MusicChanged;

        private readonly Timer _timer = new();
        private readonly ISessionData _sessionData;
        private readonly ICampaignUpdatesApi _campaignUpdatesApi;

        private readonly CampaignUpdateDto _campaignUpdates = new()
        {
            DiceRoll = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };

        public CampaignUpdates(ISessionData sessionData, ICampaignUpdatesApi campaignUpdatesApi)
        {
            _sessionData = sessionData;
            _campaignUpdatesApi = campaignUpdatesApi;

            _timer.Elapsed += Update;
            _timer.Interval = 1000;
            _timer.Enabled = true;
        }

        private async void Update(object? source, ElapsedEventArgs e)
        {
            if (_sessionData.CampaignId is int campaignId)
            {
                var campaignUpdates = await _campaignUpdatesApi.GetAsync(campaignId);

                if (campaignUpdates is not null && campaignUpdates.Data is not null)
                {
                    _campaignUpdates.MapChange = RaiseEvent(MapChanged, campaignUpdates.Data.MapChange, _campaignUpdates.MapChange);
                    _campaignUpdates.MapCollectionChange = RaiseEvent(MapCollectionChanged, campaignUpdates.Data.MapCollectionChange, _campaignUpdates.MapCollectionChange);
                    _campaignUpdates.TokenChange = RaiseEvent(TokenChanged, campaignUpdates.Data.TokenChange, _campaignUpdates.TokenChange);
                    _campaignUpdates.DiceRoll = RaiseEvent(DiceRolled, campaignUpdates.Data.DiceRoll, _campaignUpdates.DiceRoll);
                    _campaignUpdates.SoundChange = RaiseEvent(MusicChanged, campaignUpdates.Data.SoundChange, _campaignUpdates.SoundChange);
                }
            }
        }

        private long RaiseEvent(EventHandler? eventHandler, long serverTime, long clientTime)
        {
            if (serverTime > clientTime)
            {
                eventHandler?.Invoke(this, EventArgs.Empty);
                return serverTime;
            }

            return clientTime;
        }
    }
}