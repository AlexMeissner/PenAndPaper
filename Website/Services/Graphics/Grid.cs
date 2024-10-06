namespace Website.Services.Graphics;

public class Grid : IAsyncDisposable
{
    public Task Initialize()
    {
        return Task.CompletedTask;
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
