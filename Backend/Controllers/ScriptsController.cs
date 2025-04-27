using Backend.Extensions;
using Backend.Services.Repositories;
using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class ScriptsController(IScriptRepository scriptRepository) : ControllerBase
{
    [HttpGet("campaigns/{campaignId:int}/scripts")]
    public async Task<IActionResult> GetAll(int campaignId)
    {
        var response = await scriptRepository.GetAllAsync(campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpGet("maps/{mapId:int}/script")]
    public async Task<IActionResult> Get(int mapId)
    {
        var response = await scriptRepository.GetAsync(mapId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPatch("maps/{mapId:int}/script")]
    public async Task<IActionResult> Update(int mapId, ScriptUpdateDto payload)
    {
        var response = await scriptRepository.Update(mapId, payload);

        return this.StatusCode(response.StatusCode);
    }
}