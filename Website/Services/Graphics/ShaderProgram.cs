using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class ShaderProgram(IJSObjectReference jsObjectReference)
{
    public IJSObjectReference JsObjectReference => jsObjectReference;

    public async Task AddUniformBuffer(UniformBuffer uniformBuffer)
    {
        await jsObjectReference.InvokeVoidAsync("addUniformBuffer", uniformBuffer.JsObjectReference);
    }

    public async Task<bool> Compile(string vertexSource, string fragmentSource)
    {
        return await jsObjectReference.InvokeAsync<bool>("compile", vertexSource, fragmentSource);
    }
}