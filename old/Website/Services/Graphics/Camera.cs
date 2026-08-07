using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class Camera(IJSObjectReference jsObjectReference) : UniformBuffer(jsObjectReference)
{
}