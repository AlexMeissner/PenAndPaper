using Microsoft.JSInterop;

namespace Website.Services.Graphics;

public class Token(IJSObjectReference jsObjectReference) : TexturedQuad(jsObjectReference)
{
    public int Id { get; set; }
}