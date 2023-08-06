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

        private long MapChange;
        private long MapCollectionChange;
        private long TokenChange;
        private long DiceRoll = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private long AmbientSoundChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        private long SoundEffectChange = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

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
                    MapChange = RaiseEvent(MapChanged, success.MapChange, MapChange);
                    MapCollectionChange = RaiseEvent(MapCollectionChanged, success.MapCollectionChange, MapCollectionChange);
                    TokenChange = RaiseEvent(TokenChanged, success.TokenChange, TokenChange);
                    DiceRoll = RaiseEvent(DiceRolled, success.DiceRoll, DiceRoll);
                    AmbientSoundChange = RaiseEvent(AmbientSoundChanged, success.AmbientSoundChange, AmbientSoundChange);
                    SoundEffectChange = RaiseEvent(SoundEffectChanged, success.SoundEffectChange, SoundEffectChange);
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