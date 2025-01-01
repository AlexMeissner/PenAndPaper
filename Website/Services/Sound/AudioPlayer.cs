using DataTransfer.Sound;

namespace Website.Services.Sound;

[ServiceExtension.ScopedService]
internal sealed class AudioPlayer : IAsyncDisposable
{
    private readonly ICampaignEvents _campaignEvents;
    private readonly SoundContext _soundContext;

    private readonly Dictionary<string, Sound> _sounds = new();

    private const double FadeDuration = 1.0;
    private const double FadeVolume = 0.0;

    public AudioPlayer(ICampaignEvents campaignEvents, SoundContext soundContext)
    {
        _soundContext = soundContext;
        _campaignEvents = campaignEvents;
        _campaignEvents.SoundStarted += OnSoundStarted;
        _campaignEvents.SoundStopped += OnSoundStopped;
    }

    public async ValueTask DisposeAsync()
    {
        _campaignEvents.SoundStarted -= OnSoundStarted;
        _campaignEvents.SoundStopped -= OnSoundStopped;

        foreach (var sound in _sounds.Values)
        {
            await sound.DisposeAsync();
        }

        _sounds.Clear();
    }

    private async Task OnSoundStarted(SoundStartedEventArgs e)
    {
        if (!_sounds.TryGetValue(e.Identifier, out var sound))
        {
            sound = await _soundContext.CreateSound(e.Identifier, e.IsLooped);
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

    private async Task OnSoundStopped(SoundStoppedEventArgs e)
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