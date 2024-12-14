using DataTransfer.Dice;
using DataTransfer.Map;
using DataTransfer.Mouse;
using DataTransfer.Token;
using Microsoft.AspNetCore.SignalR.Client;
using static Website.Services.ServiceExtension;

namespace Website.Services;

internal interface ICampaignEvents
{
    event Func<DiceRolledEventArgs, Task>? DiceRolled;
    event Func<MapChangedEventArgs, Task>? MapChanged;
    event Func<MapCollectionChangedEventArgs, Task>? MapCollectionChanged;
    event Func<MouseMoveEventArgs, Task>? MouseMoved;
    event Func<TokenAddedEventArgs, Task>? TokenAdded;
    event Func<TokenMovedEventArgs, Task>? TokenMoved;
}

[ScopedService]
internal class CampaignEvents : ICampaignEvents, IAsyncDisposable
{
    private readonly HubConnection _hubConnection =
        new HubConnectionBuilder().WithUrl("https://localhost:7099/CampaignUpdates").Build();

    public event Func<DiceRolledEventArgs, Task>? DiceRolled;
    public event Func<MapChangedEventArgs, Task>? MapChanged;
    public event Func<MapCollectionChangedEventArgs, Task>? MapCollectionChanged;
    public event Func<MouseMoveEventArgs, Task>? MouseMoved;
    public event Func<TokenAddedEventArgs, Task>? TokenAdded;
    public event Func<TokenMovedEventArgs, Task>? TokenMoved;

    public CampaignEvents()
    {
        _hubConnection.On<DiceRolledEventArgs>("DiceRolled", OnDiceRolled);
        _hubConnection.On<MapChangedEventArgs>("MapChanged", OnMapChanged);
        _hubConnection.On<MouseMoveEventArgs>("MouseMoved", OnMouseMoved);
        _hubConnection.On<TokenAddedEventArgs>("TokenAdded", OnTokenAdded);
        _hubConnection.On<TokenMovedEventArgs>("TokenMoved", OnTokenMoved);
        _hubConnection.StartAsync(); // ToDo: Async in constructr
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }

    private async Task OnDiceRolled(DiceRolledEventArgs e)
    {
        if (DiceRolled is not null)
        {
            await DiceRolled.Invoke(e);
        }
    }

    private async Task OnMapChanged(MapChangedEventArgs e)
    {
        if (MapChanged is not null)
        {
            await MapChanged.Invoke(e);
        }
    }

    private async Task OnMouseMoved(MouseMoveEventArgs e)
    {
        if (MouseMoved is not null)
        {
            await MouseMoved.Invoke(e);
        }
    }

    private async Task OnTokenAdded(TokenAddedEventArgs e)
    {
        if (TokenAdded is not null)
        {
            await TokenAdded.Invoke(e);
        }
    }

    private async Task OnTokenMoved(TokenMovedEventArgs e)
    {
        if (TokenMoved is not null)
        {
            await TokenMoved.Invoke(e);
        }
    }
}