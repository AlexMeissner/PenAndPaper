using System.Reflection;
using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AudioController(IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub) : ControllerBase
{
    [HttpGet]
    public IActionResult Get(string name)
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var executingDirectory = Path.GetDirectoryName(assemblyLocation);

        if (executingDirectory is null)
        {
            return NotFound("Could not find the executing directory");
        }

        var filePath = Path.Combine(executingDirectory, "sounds", name + ".mp3");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(name);
        }

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        return new FileStreamResult(fileStream, "audio/mpeg");
    }

    [HttpPost]
    [Route("Play")]
    public async Task<IActionResult> Start(SoundStartedEventArgs payload)
    {
        await campaignUpdateHub.Clients.All.SoundStarted(payload);
        return Ok(payload);
    }

    [HttpPost]
    [Route("Stop")]
    public async Task<IActionResult> Stop(SoundStoppedEventArgs payload)
    {
        await campaignUpdateHub.Clients.All.SoundStopped(payload);
        return Ok(payload);
    }
}