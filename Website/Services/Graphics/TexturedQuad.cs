using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class TexturedQuad(IJSObjectReference jsObjectReference)
{
    public IJSObjectReference JSObjectReference => jsObjectReference;

    public async Task SetShaderProgram(ShaderProgram shaderProgram)
    {
        await jsObjectReference.InvokeVoidAsync("setShaderProgram", shaderProgram.JSObjectReference);
    }

    public async Task SetVertices(float[] vertices)
    {
        await jsObjectReference.InvokeVoidAsync("setVertices", vertices);
    }
}
