using Microsoft.EntityFrameworkCore;
using Website.Database;
using Website.Database.Models;

namespace Website.Services.Repositories;

public interface IAudioRepository
{
    Task<string?> Create(string id, byte[] data);
    Task Update(string id, byte[] data);
}

public class AudioRepository(IDbContextFactory<PenAndPaperDatabase> dbContextFactory) : IAudioRepository
{
    public async Task<string?> Create(string id, byte[] data)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        if (await dbContext.Audios.FindAsync(id) is not null) return null;

        var audio = new Audio()
        {
            Id = id,
            Data = data
        };

        await dbContext.AddAsync(audio);
        await dbContext.SaveChangesAsync();

        return audio.Id;
    }

    public async Task Update(string id, byte[] data)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var audio = await dbContext.Audios.FindAsync(id);

        if (audio is null) return;

        audio.Data = data;

        await dbContext.SaveChangesAsync();
    }
}
