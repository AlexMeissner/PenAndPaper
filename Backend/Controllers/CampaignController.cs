using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns")]
public class CampaignsControllerController : ControllerBase
{
    [HttpPost]
    public IActionResult Create()
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{campaignId:int}")]
    public IActionResult GetById(int campaignId)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{campaignId:int}")]
    public IActionResult Patch(int campaignId)
    {
        throw new NotImplementedException();
    }
}