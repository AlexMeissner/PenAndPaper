using Backend.Extensions;
using Backend.Hubs;
using DataTransfer.Sound;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Backend.AudioMedia;

[ApiController]
public class AudioController(IAudioRepository audioRepository,
    IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub) : ControllerBase
{
    [HttpGet("audios/{audioId}")]
    public async Task<IActionResult> Get(string audioId)
    {
        var response = await audioRepository.GetAsync(audioId);

        return response.Match<IActionResult>(
            data =>
            {
                var stream = new MemoryStream(data);

                return File(stream, "audio/mpeg", true);
            },
            this.StatusCode);
    }

    [HttpDelete("campaigns/{campaignId:int}/audios/{audioId}")]
    public async Task<IActionResult> Stop(int campaignId, string audioId)
    {
        var audioExists = await audioRepository.ContainsAsync(audioId);

        if (!audioExists) return NotFound(audioId);

        var eventArgs = new SoundStartedEventArgs(audioId, true, true);
        await campaignUpdateHub.Clients.AllInCampaign(campaignId).SoundStarted(eventArgs);

        return Ok();
    }

    [HttpPost("campaigns/{campaignId:int}/audios/{audioId}")]
    public async Task<IActionResult> Start(int campaignId, string audioId)
    {
        var audioExists = await audioRepository.ContainsAsync(audioId);

        if (!audioExists) return NotFound(audioId);

        var eventArgs = new SoundStoppedEventArgs(audioId, true);
        await campaignUpdateHub.Clients.AllInCampaign(campaignId).SoundStopped(eventArgs);

        return Ok();
    }
}