using Backend.Database;
using DataTransfer.Response;
using System.Net;

namespace Backend.AudioMedia;

public interface IAudioRepository
{
    public Task<bool> ContainsAsync(string id);
    public Task<Response<string>> Create(string id, byte[] data);
    public Task<Response<byte[]>> GetAsync(string id);
    public Task<Response> Update(string id, byte[] data);
}

public class AudioRepository(PenAndPaperDatabase dbContext) : IAudioRepository
{
    public async Task<bool> ContainsAsync(string id)
    {
        var audio = await dbContext.Audios.FindAsync(id);

        return audio is not null;
    }

    public async Task<Response<string>> Create(string id, byte[] data)
    {
        if (await dbContext.Audios.FindAsync(id) is not null) return new Response<string>(HttpStatusCode.Conflict);

        var audio = new Audio()
        {
            Id = id,
            Data = data
        };

        await dbContext.AddAsync(audio);
        await dbContext.SaveChangesAsync();

        return new Response<string>(HttpStatusCode.OK, audio.Id);
    }

    public async Task<Response<byte[]>> GetAsync(string id)
    {
        var audio = await dbContext.Audios.FindAsync(id);

        if (audio is null) return new Response<byte[]>(HttpStatusCode.NotFound);

        return new Response<byte[]>(HttpStatusCode.OK, audio.Data);
    }

    public async Task<Response> Update(string id, byte[] data)
    {
        var audio = await dbContext.Audios.FindAsync(id);

        if (audio is null) return new Response(HttpStatusCode.NotFound);

        audio.Data = data;

        await dbContext.SaveChangesAsync();

        return new Response(HttpStatusCode.OK);
    }
}
