using Backend.Extensions;
using Backend.Services.Repositories;
using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
public class MapsController(IMapRepository mapRepository) : ControllerBase
{
    [HttpGet("campaigns/{campaignId:int}/active-map")]
    public async Task<IActionResult> GetActiveMap(int campaignId)
    {
        var response = await mapRepository.GetActiveMapAsync(campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPatch("campaigns/{campaignId:int}/active-map")]
    public async Task<IActionResult> UpdateActiveMap(int campaignId, ActiveMapUpdateDto payload)
    {
        var response = await mapRepository.UpdateActiveMapAsync(campaignId, payload);

        return this.StatusCode(response.StatusCode);
    }

    [HttpPost("campaigns/{campaignId:int}/maps")]
    public async Task<IActionResult> Create(int campaignId, MapCreationDto payload)
    {
        var response = await mapRepository.CreateAsync(campaignId, payload);

        return response.Match<IActionResult>(
            mapId => CreatedAtAction(nameof(Get), mapId),
            this.StatusCode);
    }

    [HttpGet("campaigns/{campaignId:int}/maps")]
    public IActionResult GetAll(int campaignId)
    {
        var response = mapRepository.GetAllAsync(campaignId);

        return Ok(response);
    }

    [HttpDelete("maps/{mapId:int}")]
    public async Task<IActionResult> Delete(int mapId)
    {
        var response = await mapRepository.DeleteAsync(mapId);

        return this.StatusCode(response.StatusCode);
    }

    [HttpGet("maps/{mapId:int}")]
    public async Task<IActionResult> Get(int mapId)
    {
        var response = await mapRepository.GetAsync(mapId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPatch("maps/{mapId:int}")]
    public async Task<IActionResult> Update(int mapId, MapUpdateDto payload)
    {
        var response = await mapRepository.UpdateAsync(mapId, payload);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }
}