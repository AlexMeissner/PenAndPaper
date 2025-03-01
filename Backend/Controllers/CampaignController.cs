using Backend.Services.Repositories;
using DataTransfer.Campaign;
using Microsoft.AspNetCore.Mvc;
using Backend.Extensions;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns")]
public class CampaignsControllerController(ICampaignRepository campaignRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CampaignCreationDto payload)
    {
        var response = await campaignRepository.CreateAsync(payload);

        return response.Match<IActionResult>(
            campaignId => CreatedAtAction(nameof(GetById), campaignId),
            this.StatusCode);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var response = campaignRepository.GetAll();

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpGet("{campaignId:int}")]
    public async Task<IActionResult> GetById(int campaignId)
    {
        var response = await campaignRepository.GetAsync(campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPatch("{campaignId:int}")]
    public async Task<IActionResult> Update(int campaignId, CampaignUpdateDto payload)
    {
        var response = await campaignRepository.UpdateAsync(campaignId, payload);

        return this.StatusCode(response.StatusCode);
    }
}