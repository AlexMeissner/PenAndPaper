namespace Website.Services;

public interface IDragAndDrop
{
    public object? Data { get; set; }
}

[ServiceExtension.ScopedService]
public class DragAndDrop : IDragAndDrop
{
    public object? Data { get; set; }
}