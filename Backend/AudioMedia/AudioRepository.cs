using Backend.Database;
using DataTransfer.Response;
using System.Net;

namespace Backend.AudioMedia;

public interface IAudioRepository
{
    public Task<bool> ContainsAsync(string id);
    public Task<Response<byte[]>> GetAsync(string id);
}

public class AudioRepository(PenAndPaperDatabase dbContext) : IAudioRepository
{
    public async Task<bool> ContainsAsync(string id)
    {
        var audio = await dbContext.Audios.FindAsync(id);

        return audio is not null;
    }

    public async Task<Response<byte[]>> GetAsync(string id)
    {
        var audio = await dbContext.Audios.FindAsync(id);

        if (audio is null) return new Response<byte[]>(HttpStatusCode.NotFound);

        return new Response<byte[]>(HttpStatusCode.OK, audio.Data);
    }
}
