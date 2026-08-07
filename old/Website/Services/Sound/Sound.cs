using Microsoft.JSInterop;

namespace Website.Services.Sound;

public sealed class Sound(IJSObjectReference jsObjectReference) : IAsyncDisposable
{
    public async Task<Sound> Clone()
    {
        var jsSound = await jsObjectReference.InvokeAsync<IJSObjectReference>("clone");
        return new Sound(jsSound);
    }

    public async Task FadeIn(double duration, double volume)
    {
        await jsObjectReference.InvokeVoidAsync("fadeIn", duration, volume);
    }

    public async Task FadeOut(double duration)
    {
        await jsObjectReference.InvokeVoidAsync("fadeOut", duration);
    }

    public async Task Play()
    {
        await jsObjectReference.InvokeVoidAsync("play");
    }

    public async Task SetVolume(double volume)
    {
        await jsObjectReference.InvokeVoidAsync("setVolume", volume);
    }

    public async Task Stop()
    {
        await jsObjectReference.InvokeVoidAsync("stop");
    }

    public async ValueTask DisposeAsync() => await jsObjectReference.DisposeAsync();
}