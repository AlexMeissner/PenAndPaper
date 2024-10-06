using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class ShaderProgram(IJSObjectReference jsObjectReference)
{
    public IJSObjectReference JSObjectReference => jsObjectReference;

    public async Task<bool> Compile(string vertexSource, string fragmentSource)
    {
        return await jsObjectReference.InvokeAsync<bool>("compile", vertexSource, fragmentSource);
    }
}
