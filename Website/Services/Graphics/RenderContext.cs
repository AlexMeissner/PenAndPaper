﻿using DataTransfer.Types;
using Microsoft.JSInterop;
using static Website.Services.ServiceExtension;

namespace Website.Services.Graphics;

[ScopedService]
public class RenderContext(ILogger<RenderContext> logger, IJSRuntime jsRuntime) : IAsyncDisposable
{
    private IJSObjectReference? _renderContext;

    public async Task<bool> Initialize(string identifier)
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

        return true;
    }

    public async Task AddMouseIndicator(MouseIndicator mouseIndicator)
    {
        await _renderContext!.InvokeVoidAsync("addMouseIndicator", mouseIndicator.JsObjectReference);
    }

    public async Task AddToken(TexturedQuad token)
    {
        await _renderContext!.InvokeVoidAsync("addToken", token.JsObjectReference);
    }

    public async Task ClearTokens()
    {
        await _renderContext!.InvokeVoidAsync("clearTokens");
    }

    public async Task Cleanup()
    {
        await _renderContext!.InvokeAsync<IJSObjectReference>("cleanup");
    }

    public async Task<ShaderProgram> CreateShaderProgram()
    {
        var jsObjectReference = await _renderContext!.InvokeAsync<IJSObjectReference>("createShaderProgram");
        return new ShaderProgram(jsObjectReference);
    }

    public async Task<TexturedQuad> CreateTexturedQuad()
    {
        var jsObjectReference = await _renderContext!.InvokeAsync<IJSObjectReference>("createTexturedQuad");
        return new TexturedQuad(jsObjectReference);
    }

    public async Task<Token> CreateToken()
    {
        var jsObjectReference = await _renderContext!.InvokeAsync<IJSObjectReference>("createToken");
        return new Token(jsObjectReference);
    }

    public async Task<MouseIndicator> CreateMouseIndicator()
    {
        var jsObjectReference = await _renderContext!.InvokeAsync<IJSObjectReference>("createMouseIndicator");
        return new MouseIndicator(jsObjectReference);
    }

    public async Task<Camera> GetCamera()
    {
        var jsObjectReference = await _renderContext!.InvokeAsync<IJSObjectReference>("getCamera");
        return new Camera(jsObjectReference);
    }

    public async Task<Grid> GetGrid()
    {
        var jsObjectReference = await _renderContext!.InvokeAsync<IJSObjectReference>("getGrid");
        return new Grid(jsObjectReference);
    }

    public async ValueTask DisposeAsync()
    {
        if (_renderContext is not null)
        {
            try
            {
                await _renderContext.InvokeVoidAsync("destroy");
            }
            catch (JSDisconnectedException) { }
        }

        GC.SuppressFinalize(this);
    }

    public async Task SetMap(TexturedQuad map)
    {
        await _renderContext!.InvokeVoidAsync("setMap", map.JsObjectReference);
    }

    public async Task<Vector2D> TransformPosition(Vector2D position)
    {
        var transformedPosition =
            await _renderContext!.InvokeAsync<double[]>("transformPosition", position.X, position.Y);
        return new Vector2D(transformedPosition[0], transformedPosition[1]);
    }

    public async Task UpdateGrid(bool isActive, float size, float[] color)
    {
        await _renderContext!.InvokeVoidAsync("updateGrid", isActive, size, color);
    }
}