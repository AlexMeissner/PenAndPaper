using DataTransfer.Dice;
using DataTransfer.Map;
using DataTransfer.Token;
using Microsoft.AspNetCore.SignalR.Client;
using static Website.Services.ServiceExtension;

namespace Website.Services;

internal interface ICampaignEvents
{
    event Func<DiceRolledEventArgs, Task>? DiceRolled;
    event Func<MapChangedEventArgs, Task>? MapChanged;
    event Func<MapCollectionChangedEventArgs, Task>? MapCollectionChanged;
    event Func<TokenAddedEventArgs, Task>? TokenAdded;
    event Func<TokenMovedEventArgs, Task>? TokenMoved;
}

[ScopedService]
internal class CampaignEvents : ICampaignEvents, IAsyncDisposable
{
    private readonly HubConnection _hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7099/CampaignUpdates").Build();

    public event Func<DiceRolledEventArgs, Task>? DiceRolled;
    public event Func<MapChangedEventArgs, Task>? MapChanged;
    public event Func<MapCollectionChangedEventArgs, Task>? MapCollectionChanged;
    public event Func<TokenAddedEventArgs, Task>? TokenAdded;
    public event Func<TokenMovedEventArgs, Task>? TokenMoved;

    public CampaignEvents()
    {
        _hubConnection.On<DiceRolledEventArgs>("DiceRolled", OnDiceRolled);
        _hubConnection.StartAsync(); // ToDo: Async in constructr
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.DisposeAsync();
    }

    private async Task OnDiceRolled(DiceRolledEventArgs diceRoll)
    {
        if (DiceRolled is not null)
        {
            await DiceRolled.Invoke(diceRoll);
        }
    }
}
