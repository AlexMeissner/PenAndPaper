using DataTransfer.Types;
using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class MouseIndicator(IJSObjectReference jsObjectReference) : TexturedQuad(jsObjectReference)
{
    public async Task Update(Vector2D position, Vector3D color)
    {
        await JsObjectReference.InvokeVoidAsync("update", position.X, position.Y, color.R, color.G, color.B);
    }
}