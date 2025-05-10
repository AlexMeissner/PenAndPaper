using DataTransfer.Response;
using DataTransfer.Sound;

namespace ApiClient;

public interface IAudioApi
{
    Task<Response> Start(int campaignId, int soundId, bool isLooped);
    Task<Response> Stop(int campaignId, int soundId);
}

public class AudioApi(IRequestBuilder requestBuilder) : IAudioApi
{
    public Task<Response> Start(int campaignId, int soundId, bool isLooped)
    {
        var payload = new SoundStartDto(isLooped);
        return requestBuilder.Path("campaigns", campaignId, "sounds", soundId).PostAsync(payload);
    }

    public Task<Response> Stop(int campaignId, int soundId)
    {
        return requestBuilder.Path("campaigns", campaignId, "sounds", soundId).DeleteAsync();
    }
}