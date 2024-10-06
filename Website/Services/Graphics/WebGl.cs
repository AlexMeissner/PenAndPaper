using Microsoft.JSInterop;
using System.Numerics;
using static Website.Services.ServiceExtension;

namespace Website.Services.Graphics;

public interface IWebGl
{
    Task AttachShader(IWebGlProgram program, IWebGLShader shader);
    Task BindBuffer(BufferTarget target, IWebGlBuffer buffer);
    Task BindVertexArray(IWebGLVertexArrayObject vao);
    Task BufferData(BufferTarget target, float[] srcData, BufferUsage usage);
    Task Clear();
    Task ClearColor(double red, double green, double blue, double alpha);
    Task CompileShader(IWebGLShader shader);
    Task<IWebGlBuffer> CreateBuffer();
    Task<IWebGlProgram> CreateProgram();
    Task<IWebGLShader> CreateShader();
    Task<IWebGLVertexArrayObject> CreateVertexArray();
    Task DeleteProgram(IWebGlProgram program);
    Task DrawElements(DrawMode mode, long count, ElementArrayType type, BigInteger offset);
    Task EnableVertexAttribArray(uint index);
    Task<object> GetProgramParameter(IWebGlProgram program, ProgramParameterName pname);
    Task<IWebGLUniformLocation> GetUniformLocation(IWebGlProgram program, string name);
    Task<bool> Initialize();
    Task LinkProgram(IWebGlProgram program);
    Task ShaderSource(IWebGLShader shader, string source);
    Task StartRenderLoop();
    Task Uniform1f(IWebGLUniformLocation location, float x);
    Task UseProgram(IWebGlProgram program);
    Task VertexAttribPointer(uint index, uint size, VertexAttribPointerType type, bool normalized, uint stride, uint offset);
    Task Viewport(uint x, uint y, uint width, uint height);
}

// ToDo: Check if I need to clean up the webgl context and make this disposable
[ScopedService]
public class WebGl(IJSRuntime jsRuntime) : IWebGl
{
    private IJSObjectReference? gl;

    public async Task AttachShader(IWebGlProgram program, IWebGLShader shader)
    {
        await jsRuntime.InvokeVoidAsync("attachShader", gl, program, shader);
    }

    public async Task BindBuffer(BufferTarget target, IWebGlBuffer buffer)
    {
        await jsRuntime.InvokeVoidAsync("bindBuffer", gl, target, buffer);
    }

    public async Task BindVertexArray(IWebGLVertexArrayObject vao)
    {
        await jsRuntime.InvokeVoidAsync("bindVertexArray", gl, vao);
    }

    public async Task BufferData(BufferTarget target, float[] srcData, BufferUsage usage)
    {
        await jsRuntime.InvokeVoidAsync("bufferData", gl, target, srcData, usage);
    }

    public async Task Clear()
    {
        await jsRuntime.InvokeVoidAsync("clear", gl);
    }

    public async Task ClearColor(double red, double green, double blue, double alpha)
    {
        await jsRuntime.InvokeVoidAsync("clearColor", gl, red, green, blue, alpha);
    }

    public async Task CompileShader(IWebGLShader shader)
    {
        await jsRuntime.InvokeVoidAsync("compileShader", gl, shader);
    }

    public async Task<IWebGlBuffer> CreateBuffer()
    {
        return await jsRuntime.InvokeAsync<IWebGlBuffer>("createBuffer", gl);
    }

    public async Task<IWebGlProgram> CreateProgram()
    {
        return await jsRuntime.InvokeAsync<IWebGlProgram>("createProgram", gl);
    }

    public async Task<IWebGLShader> CreateShader()
    {
        return await jsRuntime.InvokeAsync<IWebGLShader>("createShader", gl);
    }

    public async Task<IWebGLVertexArrayObject> CreateVertexArray()
    {
        return await jsRuntime.InvokeAsync<IWebGLVertexArrayObject>("createVertexArray", gl);
    }

    public async Task DeleteProgram(IWebGlProgram program)
    {
        await jsRuntime.InvokeVoidAsync("deleteProgram", gl, program);
    }

    public async Task DrawElements(DrawMode mode, long count, ElementArrayType type, BigInteger offset)
    {
        await jsRuntime.InvokeVoidAsync("drawElements", gl, mode, count, type, offset);
    }

    public async Task EnableVertexAttribArray(uint index)
    {
        await jsRuntime.InvokeVoidAsync("enableVertexAttribArray", gl, index);
    }

    public async Task<object> GetProgramParameter(IWebGlProgram program, ProgramParameterName pname)
    {
        return await jsRuntime.InvokeAsync<object>("getProgramParameter", gl, program, pname);
    }

    public async Task<IWebGLUniformLocation> GetUniformLocation(IWebGlProgram program, string name)
    {
        return await jsRuntime.InvokeAsync<IWebGLUniformLocation>("getUniformLocation", gl, program, name);
    }

    public async Task<bool> Initialize()
    {
        try
        {
            gl = await jsRuntime.InvokeAsync<IJSObjectReference>("getContext", "renderCanvas");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            return false;
        }

        return gl is not null;
    }

    public async Task LinkProgram(IWebGlProgram program)
    {
        await jsRuntime.InvokeVoidAsync("linkProgram", gl, program);
    }

    public async Task ShaderSource(IWebGLShader shader, string source)
    {
        await jsRuntime.InvokeVoidAsync("shaderSource", gl, shader, source);
    }

    public async Task StartRenderLoop()
    {
        await jsRuntime.InvokeVoidAsync("startRenderLoop");
    }

    public async Task Uniform1f(IWebGLUniformLocation location, float x)
    {
        await jsRuntime.InvokeVoidAsync("uniform1f", gl, location, x);
    }

    public async Task UseProgram(IWebGlProgram program)
    {
        await jsRuntime.InvokeVoidAsync("useProgram", gl, program);
    }

    public async Task VertexAttribPointer(uint index, uint size, VertexAttribPointerType type, bool normalized, uint stride, uint offset)
    {
        await jsRuntime.InvokeVoidAsync("vertexAttribPointer", gl, index, size, type, normalized, stride, offset);
    }

    public async Task Viewport(uint x, uint y, uint width, uint height)
    {
        await jsRuntime.InvokeVoidAsync("viewport", gl, x, y, width, height);
    }
}
