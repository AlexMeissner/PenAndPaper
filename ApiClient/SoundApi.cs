using DataTransfer.Response;
using DataTransfer.Sound;

namespace ApiClient;

public interface ISoundApi
{
    Task<Response> Start(int campaignId, int soundId, bool isLooped);
    Task<Response> Stop(int campaignId, int soundId);
}

public class SoundApi(IRequest request) : ISoundApi
{
    public Task<Response> Start(int campaignId, int soundId, bool isLooped)
    {
        var payload = new SoundStartDto(isLooped);
        return request.Path("campaigns", campaignId, "sounds", soundId).PostAsync(payload);
    }

    public Task<Response> Stop(int campaignId, int soundId)
    {
        return request.Path("campaigns", campaignId, "sounds", soundId).DeleteAsync();
    }
}