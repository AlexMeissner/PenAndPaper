using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class TexturedQuad(IJSObjectReference jsObjectReference)
{
    public IJSObjectReference JsObjectReference => jsObjectReference;

    public async Task SetShaderProgram(ShaderProgram shaderProgram)
    {
        await jsObjectReference.InvokeVoidAsync("setShaderProgram", shaderProgram.JsObjectReference);
    }

    public async Task SetTexture(byte[] image)
    {
        var imageBase64 = $"data:image/png;base64,{Convert.ToBase64String(image)}";
        await jsObjectReference.InvokeVoidAsync("setTexture", imageBase64);
    }

    public async Task SetVertices(float[] vertices)
    {
        await jsObjectReference.InvokeVoidAsync("setVertices", vertices);
    }

    public async Task SetUniform(string name, float value)
    {
        await jsObjectReference.InvokeVoidAsync("setUniform", name, value);
    }

    public async Task<float> GetUniform(string name)
    {
        return await jsObjectReference.InvokeAsync<float>("getUniform", name);
    }

    public async Task UpdateVertices(float[] vertices)
    {
        await jsObjectReference.InvokeVoidAsync("updateVertices", vertices);
    }
}