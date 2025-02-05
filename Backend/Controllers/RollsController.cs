using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("campaigns/{campaignId:int}/users/{userId:int}/rolls")]
public class RollsController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(int campaignId, int userId)
    {
        throw new NotImplementedException();
    }
}