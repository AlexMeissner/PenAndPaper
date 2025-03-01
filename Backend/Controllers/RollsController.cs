using DataTransfer.Dice;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}/rolls")]
public class RollsController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(int campaignId, RollDiceDto payload)
    {
        throw new NotImplementedException();
    }
}