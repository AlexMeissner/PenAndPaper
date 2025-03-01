using ApiClient;
using DataTransfer.Chat;
using DataTransfer.Dice;
using DataTransfer.Grid;
using DataTransfer.Map;
using DataTransfer.Mouse;
using DataTransfer.Sound;
using DataTransfer.Token;
using Microsoft.AspNetCore.SignalR.Client;
using static Website.Services.ServiceExtension;

namespace Website.Services;

internal interface ICampaignEvents
{
    event Func<ChatMessageEventArgs, Task>? ChatMessageReceived;
    event Func<DiceRolledEventArgs, Task>? DiceRolled;
    event Func<GridChangedEventArgs, Task>? GridChanged;
    event Func<MapChangedEventArgs, Task>? MapChanged;
    event Func<MapCollectionChangedEventArgs, Task>? MapCollectionChanged;
    event Func<MouseMoveEventArgs, Task>? MouseMoved;
    event Func<SoundStartedEventArgs, Task>? SoundStarted;
    event Func<SoundStoppedEventArgs, Task>? SoundStopped;
    event Func<TokenAddedEventArgs, Task>? TokenAdded;
    event Func<TokenMovedEventArgs, Task>? TokenMoved;
}

[ScopedService]
internal class CampaignEvents : ICampaignEvents, IAsyncDisposable
{
    private readonly HubConnection _hubConnection;

    public event Func<ChatMessageEventArgs, Task>? ChatMessageReceived;
    public event Func<DiceRolledEventArgs, Task>? DiceRolled;
    public event Func<GridChangedEventArgs, Task>? GridChanged;
    public event Func<MapChangedEventArgs, Task>? MapChanged;
    public event Func<MapCollectionChangedEventArgs, Task>? MapCollectionChanged;
    public event Func<MouseMoveEventArgs, Task>? MouseMoved;
    public event Func<SoundStartedEventArgs, Task>? SoundStarted;
    public event Func<SoundStoppedEventArgs, Task>? SoundStopped;
    public event Func<TokenAddedEventArgs, Task>? TokenAdded;
    public event Func<TokenMovedEventArgs, Task>? TokenMoved;

    public CampaignEvents(IEndPointProvider endPointProvider)
    {
        var url = endPointProvider.BaseUrl + "CampaignUpdates";
        _hubConnection = new HubConnectionBuilder().WithUrl(url).Build();
        _hubConnection.On<ChatMessageEventArgs>("ChatMessageReceived", OnChatMessageReceived);
        _hubConnection.On<DiceRolledEventArgs>("DiceRolled", OnDiceRolled);
        _hubConnection.On<GridChangedEventArgs>("GridChanged", OnGridChanged);
        _hubConnection.On<MapChangedEventArgs>("MapChanged", OnMapChanged);
        _hubConnection.On<MapCollectionChangedEventArgs>("MapCollectionChanged", OnMapCollectionChanged);
        _hubConnection.On<MouseMoveEventArgs>("MouseMoved", OnMouseMoved);
        _hubConnection.On<SoundStartedEventArgs>("SoundStarted", OnSoundStarted);
        _hubConnection.On<SoundStoppedEventArgs>("SoundStopped", OnSoundStopped);
        _hubConnection.On<TokenAddedEventArgs>("TokenAdded", OnTokenAdded);
        _hubConnection.On<TokenMovedEventArgs>("TokenMoved", OnTokenMoved);
        _hubConnection.StartAsync(); // ToDo: Async in constructor
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }

    private async Task OnChatMessageReceived(ChatMessageEventArgs e)
    {
        if (ChatMessageReceived is not null)
        {
            await ChatMessageReceived.Invoke(e);
        }
    }

    private async Task OnDiceRolled(DiceRolledEventArgs e)
    {
        if (DiceRolled is not null)
        {
            await DiceRolled.Invoke(e);
        }
    }

    private async Task OnGridChanged(GridChangedEventArgs e)
    {
        if (GridChanged is not null)
        {
            await GridChanged.Invoke(e);
        }
    }

    private async Task OnMapChanged(MapChangedEventArgs e)
    {
        if (MapChanged is not null)
        {
            await MapChanged.Invoke(e);
        }
    }

    private async Task OnMapCollectionChanged(MapCollectionChangedEventArgs e)
    {
        if (MapCollectionChanged is not null)
        {
            await MapCollectionChanged.Invoke(e);
        }
    }

    private async Task OnMouseMoved(MouseMoveEventArgs e)
    {
        if (MouseMoved is not null)
        {
            await MouseMoved.Invoke(e);
        }
    }

    private async Task OnSoundStarted(SoundStartedEventArgs e)
    {
        if (SoundStarted is not null)
        {
            await SoundStarted.Invoke(e);
        }
    }

    private async Task OnSoundStopped(SoundStoppedEventArgs e)
    {
        if (SoundStopped is not null)
        {
            await SoundStopped.Invoke(e);
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