using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public abstract class UniformBuffer(IJSObjectReference jsObjectReference)
{
    public IJSObjectReference JsObjectReference => jsObjectReference;
}