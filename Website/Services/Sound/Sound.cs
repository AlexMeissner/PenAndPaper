using Microsoft.JSInterop;

namespace Website.Services.Sound;

public class Sound(IJSObjectReference jsObjectReference)
{
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
}