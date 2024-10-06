using Microsoft.JSInterop;
using static Website.Services.ServiceExtension;

namespace Website.Services.Graphics;

[ScopedService]
public class RenderContext(ILogger<RenderContext> logger, IJSRuntime jsRuntime) : IAsyncDisposable
{
    private IJSObjectReference? _renderContext;

    private readonly Grid _grid = new();

    public async Task<bool> Initialize(string identifier)
    {
        try
        {
            _renderContext = await jsRuntime.InvokeAsync<IJSObjectReference>("createRenderContext");

            if (_renderContext is null)
            {
                logger.LogError("Could not create a render context");
                return false;
            }

            var isInitialized = await _renderContext.InvokeAsync<bool>("initialize", identifier);

            if (!isInitialized)
            {
                logger.LogError("Could not initialize the render context");
                return false;
            }

            await _grid.Initialize();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create render context");
        }

        return false;
    }

    public async ValueTask DisposeAsync()
    {
        await _grid.DisposeAsync();

        if (_renderContext is not null)
        {
            await _renderContext.InvokeVoidAsync("destroy");
        }

        GC.SuppressFinalize(this);
    }
}
