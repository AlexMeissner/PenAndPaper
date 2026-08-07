using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class Grid(IJSObjectReference jsObjectReference) : UniformBuffer(jsObjectReference)
{
}