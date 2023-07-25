using Client.Services.API;
using DataTransfer.Campaign;
using System;
using System.Timers;
using static Client.Services.ServiceExtension;

namespace Client.Services
{
    public interface ICampaignUpdates
    {
        event EventHandler MapChanged;
        event EventHandler MapCollectionChanged;
        event EventHandler TokenChanged;
        event EventHandler DiceRolled;
        event EventHandler AmbientSoundChanged;
        event EventHandler SoundEffectChanged;
    }

    [SingletonService]
    public class CampaignUpdates : ICampaignUpdates
    {
        public event EventHandler? MapChanged;
        public event EventHandler? MapCollectionChanged;
        public event EventHandler? TokenChanged;
        public event EventHandler? DiceRolled;
        public event EventHandler? AmbientSoundChanged;
        public event EventHandler? SoundEffectChanged;

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
                var response = await _campaignUpdatesApi.GetAsync(campaignId);

                response.Match(success =>
                {
                    _campaignUpdates.MapChange = RaiseEvent(MapChanged, success.MapChange, _campaignUpdates.MapChange);
                    _campaignUpdates.MapCollectionChange = RaiseEvent(MapCollectionChanged, success.MapCollectionChange, _campaignUpdates.MapCollectionChange);
                    _campaignUpdates.TokenChange = RaiseEvent(TokenChanged, success.TokenChange, _campaignUpdates.TokenChange);
                    _campaignUpdates.DiceRoll = RaiseEvent(DiceRolled, success.DiceRoll, _campaignUpdates.DiceRoll);
                    _campaignUpdates.AmbientSoundChange = RaiseEvent(AmbientSoundChanged, success.AmbientSoundChange, _campaignUpdates.AmbientSoundChange);
                    _campaignUpdates.SoundEffectChange = RaiseEvent(SoundEffectChanged, success.SoundEffectChange, _campaignUpdates.SoundEffectChange);
                });


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