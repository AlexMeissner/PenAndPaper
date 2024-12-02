using DataTransfer.Types;
using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class MouseIndicator(IJSObjectReference jsObjectReference) : TexturedQuad(jsObjectReference)
{
    public async Task SetPosition(Vector2D position)
    {
        await JsObjectReference.InvokeVoidAsync("setPosition", position.X, position.Y);
    }
}