using Microsoft.JSInterop;
using static Website.Services.ServiceExtension;

namespace Website.Services.Sound;

[ScopedService]
public class SoundContext(IJSRuntime jsRuntime, IEndPointProvider endpointProvider)
{
    public async Task<Sound> CreateSound(string fileName, bool isLooped)
    {
        var url = $"{endpointProvider.BaseURL}Audio?name={fileName}";
        var jsSound = await jsRuntime.InvokeAsync<IJSObjectReference>("createSound", url, isLooped);
        return new Sound(jsSound);
    }
}