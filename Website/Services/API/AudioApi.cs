using DataTransfer.Sound;

namespace Website.Services.API;

public interface IAudioApi
{
    Task<HttpResponse> Start(SoundStartedEventArgs payload);
    Task<HttpResponse> Stop(SoundStoppedEventArgs payload);
}

[ServiceExtension.TransistentService]
public class AudioApi(IEndPointProvider endPointProvider, ITokenProvider tokenProvider) : IAudioApi
{
    private readonly HttpRequest _startRequest = new(endPointProvider.BaseURL + "Audio/Play", tokenProvider);
    private readonly HttpRequest _stopRequest = new(endPointProvider.BaseURL + "Audio/Stop", tokenProvider);

    public Task<HttpResponse> Start(SoundStartedEventArgs payload)
    {
        return _startRequest.PostAsync(payload);
    }

    public Task<HttpResponse> Stop(SoundStoppedEventArgs payload)
    {
        return _stopRequest.PostAsync(payload);
    }
}