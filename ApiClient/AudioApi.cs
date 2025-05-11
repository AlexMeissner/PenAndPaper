using DataTransfer.Response;
using DataTransfer.Sound;

namespace ApiClient;

public interface IAudioApi
{
    Task<Response> Create(AudioCreationDto payload);
    Task<Response> Start(int campaignId, string soundId, bool isLooped);
    Task<Response> Stop(int campaignId, string soundId);
    Task<Response> Update(string soundId, AudioUpdateDto payload);
}

public class AudioApi(IRequestBuilder requestBuilder) : IAudioApi
{
    public Task<Response> Create(AudioCreationDto payload)
    {
        return requestBuilder.Path("audios").PostAsync(payload);
    }

    public Task<Response> Start(int campaignId, string soundId, bool isLooped)
    {
        var payload = new SoundStartDto(isLooped);
        return requestBuilder.Path("campaigns", campaignId, "audios", soundId).PostAsync(payload);
    }

    public Task<Response> Stop(int campaignId, string soundId)
    {
        return requestBuilder.Path("campaigns", campaignId, "audios", soundId).DeleteAsync();
    }

    public Task<Response> Update(string soundId, AudioUpdateDto payload)
    {
        return requestBuilder.Path("audios", soundId).PutAsync(payload);
    }
}