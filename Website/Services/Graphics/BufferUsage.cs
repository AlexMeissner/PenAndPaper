namespace Website.Services.Graphics;

public enum BufferUsage
{
    StaticDraw = 0x88E4,
    DynamicDraw = 0x88E8,
    StreamDraw = 0x88E0,
    StaticRead = 0x88E5,
    DynamicRead = 0x88E9,
    StreamRead = 0x88E1,
    StaticCopy = 0x88E6,
    DynamicCopy = 0x88EA,
    StreamCopy = 0x88E2,
}
