using ApiClient;
using Microsoft.AspNetCore.Mvc;

namespace Website.Controller;

[ApiController]
public class AudioProxyController(HttpClient httpClient, IEndPointProvider endPointProvider) : ControllerBase
{
    [HttpGet("audios/{audioId}")]
    public async Task<IActionResult> Get(string audioId)
    {
        var apiUrl = $"{endPointProvider.BaseUrl}audios/{audioId}";

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