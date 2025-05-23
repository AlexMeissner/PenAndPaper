using Backend.Extensions;
using Backend.Hubs;
using Backend.Services.Repositories;
using DataTransfer.Grid;
using DataTransfer.Map;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace Backend.Controllers;

[ApiController]
public class MapsController(IMapRepository mapRepository, IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub) : ControllerBase
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

        if (response.IsSuccess)
        {
            var eventsArgs = new MapChangedEventArgs(payload.MapId);
            await campaignUpdateHub.Clients.AllInCampaign(campaignId).MapChanged(eventsArgs);
        }

        return this.StatusCode(response.StatusCode);
    }

    [HttpPost("campaigns/{campaignId:int}/maps")]
    public async Task<IActionResult> Create(int campaignId, MapCreationDto payload)
    {
        var response = await mapRepository.CreateAsync(campaignId, payload);

        return response.Match<IActionResult>(
            mapId => CreatedAtAction(nameof(Get), new { mapId }, mapId),
            this.StatusCode);
    }

    [HttpGet("campaigns/{campaignId:int}/maps")]
    public async Task<IActionResult> GetAll(int campaignId)
    {
        var response = await mapRepository.GetAllAsync(campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
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

        if (response.StatusCode < HttpStatusCode.BadRequest &&
            payload.Grid is not null &&
            await mapRepository.GetCampaignId(mapId) is { } campaignId)
        {
            var eventArgs = new GridChangedEventArgs(payload.Grid.IsActive, payload.Grid.Size);
            await campaignUpdateHub.Clients.AllInCampaign(campaignId).GridChanged(eventArgs);
        }

        return response.Match<IActionResult>(
           Ok,
           this.StatusCode);
    }
}