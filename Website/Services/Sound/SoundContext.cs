using Microsoft.JSInterop;
using static Website.Services.ServiceExtension;

namespace Website.Services.Sound;

[ScopedService]
public class SoundContext(ILogger<SoundContext> logger, IJSRuntime jsRuntime) : IAsyncDisposable
{
    private IJSObjectReference? _soundContext;

    public async Task<bool> Initialize()
    {
        _soundContext = await jsRuntime.InvokeAsync<IJSObjectReference>("createSoundContext");

        if (_soundContext is not null) return true;

        logger.LogError("Could not create a sound context");
        return false;
    }

    public async Task<Sound> CreateSound(string filePath, bool isLooped)
    {
        var jsSound = await _soundContext!.InvokeAsync<IJSObjectReference>("createSound", filePath, isLooped);
        return new Sound(jsSound);
    }

    public async ValueTask DisposeAsync()
    {
        if (_soundContext is not null)
        {
            await _soundContext.InvokeVoidAsync("destroy");
        }

        GC.SuppressFinalize(this);
    }
}