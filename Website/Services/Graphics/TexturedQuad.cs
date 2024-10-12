using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class TexturedQuad(IJSObjectReference jsObjectReference)
{
    public IJSObjectReference JSObjectReference => jsObjectReference;

    public async Task SetShaderProgram(ShaderProgram shaderProgram)
    {
        await jsObjectReference.InvokeVoidAsync("setShaderProgram", shaderProgram.JSObjectReference);
    }

    public async Task SetTexture(byte[] image)
    {
        //var imageBase64 = $"data:image/png;base64,{Convert.ToBase64String(image)}";
        var imageBase64 = Convert.ToBase64String(image);
        await jsObjectReference.InvokeVoidAsync("setTexture", imageBase64);
    }

    public async Task SetVertices(float[] vertices)
    {
        await jsObjectReference.InvokeVoidAsync("setVertices", vertices);
    }
}
