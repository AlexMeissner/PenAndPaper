using Microsoft.AspNetCore.Mvc;
using Website.Database;

namespace Website.Controller;

[ApiController]
public class AudioProxyController(PenAndPaperDatabase dbContext) : ControllerBase
{
    [HttpGet("audios/{audioId}")]
    public async Task<IActionResult> Get(string audioId)
    {
        var audio = await dbContext.Audios.FindAsync(audioId);

        if (audio is null) return NotFound();

        var stream = new MemoryStream(audio.Data);

        return File(stream, "audio/mpeg", true);
    }
}