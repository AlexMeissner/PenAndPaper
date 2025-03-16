using Backend.Services.Repositories;
using DataTransfer.Campaign;
using Microsoft.AspNetCore.Mvc;
using Backend.Extensions;
using Backend.Services;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns")]
public class CampaignsControllerController(IIdentity identity, ICampaignRepository campaignRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CampaignCreationDto payload)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await campaignRepository.CreateAsync(identityClaims, payload);

        return response.Match<IActionResult>(
            campaignId => CreatedAtAction(nameof(GetById), new { campaignId }, campaignId),
            this.StatusCode);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = campaignRepository.GetAll(identityClaims);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpGet("{campaignId:int}")]
    public async Task<IActionResult> GetById(int campaignId)
    {
        var identityClaims = await identity.FromClaimsPrincipal(User);

        if (identityClaims is null) return Unauthorized();

        var response = await campaignRepository.GetAsync(identityClaims, campaignId);

        return response.Match<IActionResult>(
            Ok,
            this.StatusCode);
    }

    [HttpPut("{campaignId:int}")]
    public async Task<IActionResult> Update(int campaignId, CampaignUpdateDto payload)
    {
        var response = await campaignRepository.UpdateAsync(campaignId, payload);

        return this.StatusCode(response.StatusCode);
    }
}