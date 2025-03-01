using System.Net;
using Backend.Database;
using DataTransfer.Response;
using DataTransfer.Script;

namespace Backend.Services.Repositories;

public interface IScriptRepository
{
    Task<Response<ScriptDto>> GetAsync(int mapId);
    Task<Response<IEnumerable<ScriptsDto>>> GetAllAsync(int campaignId);
}

public class ScriptRepository(PenAndPaperDatabase dbContext) : IScriptRepository
{
    public async Task<Response<ScriptDto>> GetAsync(int mapId)
    {
        var map = await dbContext.Maps.FindAsync(mapId);

        if (map is null)
        {
            return new Response<ScriptDto>(HttpStatusCode.NotFound);
        }

        var scriptDto = new ScriptDto(map.Script);

        return new Response<ScriptDto>(HttpStatusCode.OK, scriptDto);
    }

    public async Task<Response<IEnumerable<ScriptsDto>>> GetAllAsync(int campaignId)
    {
        var campaign = await dbContext.Campaigns.FindAsync(campaignId);

        if (campaign is null)
        {
            return new Response<IEnumerable<ScriptsDto>>(HttpStatusCode.NotFound);
        }

        await dbContext.Entry(campaign).Collection(c => c.Maps).LoadAsync();

        var scripts = campaign.Maps.Select(m => new ScriptsDto(m.Id, m.Name));

        return new Response<IEnumerable<ScriptsDto>>(HttpStatusCode.OK, scripts);
    }
}