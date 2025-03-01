using ApiClient;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controller;

[Route("Audio")]
[ApiController]
public class AudioProxyController(HttpClient httpClient, IEndPointProvider endPointProvider) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(string name)
    {
        var apiUrl = $"{endPointProvider.BaseUrl}Audio?name={name}";

        var response = await httpClient.GetAsync(apiUrl, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
        var stream = await response.Content.ReadAsStreamAsync();

        return File(stream, contentType);
    }
}