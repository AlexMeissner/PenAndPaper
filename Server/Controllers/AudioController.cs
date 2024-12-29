using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AudioController : ControllerBase
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
}