using Website.Events;

namespace Website.Services.Sound;

[ServiceExtension.ScopedService]
internal sealed class AudioPlayer(ICampaignEventHub campaignEventHub, SoundContext soundContext) : IAsyncDisposable
{
    private readonly Dictionary<string, Sound> _sounds = [];

    private const double FadeDuration = 1.0;
    private const double FadeVolume = 0.0;

    private ICampaignSubscription? soundStartedSubscription;
    private ICampaignSubscription? soundStoppedSubscription;

    public void Initialize(int campaignId)
    {
        var campaignDispathcer = campaignEventHub.ForCampaign(campaignId);
        soundStartedSubscription = campaignDispathcer.Subscribe<SoundStartedEvent>(OnSoundStarted);
        soundStoppedSubscription = campaignDispathcer.Subscribe<SoundStoppedEvent>(OnSoundStopped);
    }

    public async ValueTask DisposeAsync()
    {
        soundStartedSubscription?.Dispose();
        soundStoppedSubscription?.Dispose();

        foreach (var sound in _sounds.Values)
        {
            await sound.DisposeAsync();
        }

        _sounds.Clear();
    }

    private async ValueTask OnSoundStarted(SoundStartedEvent e)
    {
        if (!_sounds.TryGetValue(e.Identifier, out var sound))
        {
            sound = await soundContext.CreateSound(e.Identifier, e.IsLooped);
            _sounds.Add(e.Identifier, sound);
        }

        if (e.IsFaded)
        {
            await sound.FadeIn(FadeDuration, FadeVolume);
        }
        else
        {
            await sound.Play();
        }
    }

    private async ValueTask OnSoundStopped(SoundStoppedEvent e)
    {
        if (!_sounds.TryGetValue(e.Identifier, out var sound))
        {
            return;
        }

        if (e.IsFaded)
        {
            await sound.FadeOut(FadeDuration);
        }
        else
        {
            await sound.Stop();
        }
    }
}