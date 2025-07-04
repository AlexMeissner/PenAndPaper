﻿using ApiClient;
using DataTransfer.Chat;
using DataTransfer.Dice;
using DataTransfer.Grid;
using DataTransfer.Initiative;
using DataTransfer.Map;
using DataTransfer.Mouse;
using DataTransfer.Sound;
using DataTransfer.Token;
using Microsoft.AspNetCore.SignalR.Client;
using static Website.Services.ServiceExtension;

namespace Website.Services;

internal interface ICampaignEvents
{
    Task Connect(int campaignId);

    event Func<ChatMessageEventArgs, Task>? ChatMessageReceived;
    event Func<CombatantAddedEventArgs, Task>? CombatantAdded;
    event Func<CombatantRemovedEventArgs, Task>? CombatantRemoved;
    event Func<CombatantUpdatedEventArgs, Task>? CombatantUpdated;
    event Func<DiceRolledEventArgs, Task>? DiceRolled;
    event Func<GridChangedEventArgs, Task>? GridChanged;
    event Func<MapChangedEventArgs, Task>? MapChanged;
    event Func<MouseMoveEventArgs, Task>? MouseMoved;
    event Func<SoundStartedEventArgs, Task>? SoundStarted;
    event Func<SoundStoppedEventArgs, Task>? SoundStopped;
    event Func<TokenAddedEventArgs, Task>? TokenAdded;
    event Func<TokenMovedEventArgs, Task>? TokenMoved;
    event Func<TurnChangedEventArgs, Task>? TurnChanged;
}

[ScopedService]
internal class CampaignEvents(IEndPointProvider endPointProvider, ITokenProvider tokenProvider) : ICampaignEvents, IAsyncDisposable
{
    private HubConnection? _hubConnection;

    public event Func<ChatMessageEventArgs, Task>? ChatMessageReceived;
    public event Func<CombatantAddedEventArgs, Task>? CombatantAdded;
    public event Func<CombatantRemovedEventArgs, Task>? CombatantRemoved;
    public event Func<CombatantUpdatedEventArgs, Task>? CombatantUpdated;
    public event Func<DiceRolledEventArgs, Task>? DiceRolled;
    public event Func<GridChangedEventArgs, Task>? GridChanged;
    public event Func<MapChangedEventArgs, Task>? MapChanged;
    public event Func<MouseMoveEventArgs, Task>? MouseMoved;
    public event Func<SoundStartedEventArgs, Task>? SoundStarted;
    public event Func<SoundStoppedEventArgs, Task>? SoundStopped;
    public event Func<TokenAddedEventArgs, Task>? TokenAdded;
    public event Func<TokenMovedEventArgs, Task>? TokenMoved;
    public event Func<TurnChangedEventArgs, Task>? TurnChanged;

    public async Task Connect(int campaignId)
    {
        var url = endPointProvider.BaseUrl + "campaign-updates/" + campaignId;
        var token = await tokenProvider.GetToken();

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(url, options => { options.AccessTokenProvider = () => Task.FromResult<string?>(token); })
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<ChatMessageEventArgs>("ChatMessageReceived", OnChatMessageReceived);
        _hubConnection.On<CombatantAddedEventArgs>("CombatantAdded", OnCombatantAdded);
        _hubConnection.On<CombatantRemovedEventArgs>("CombatantRemoved", OnCombatantRemoved);
        _hubConnection.On<CombatantUpdatedEventArgs>("CombatantUpdated", OnCombatantUpdated);
        _hubConnection.On<DiceRolledEventArgs>("DiceRolled", OnDiceRolled);
        _hubConnection.On<GridChangedEventArgs>("GridChanged", OnGridChanged);
        _hubConnection.On<MapChangedEventArgs>("MapChanged", OnMapChanged);
        _hubConnection.On<MouseMoveEventArgs>("MouseMoved", OnMouseMoved);
        _hubConnection.On<SoundStartedEventArgs>("SoundStarted", OnSoundStarted);
        _hubConnection.On<SoundStoppedEventArgs>("SoundStopped", OnSoundStopped);
        _hubConnection.On<TokenAddedEventArgs>("TokenAdded", OnTokenAdded);
        _hubConnection.On<TokenMovedEventArgs>("TokenMoved", OnTokenMoved);
        _hubConnection.On<TurnChangedEventArgs>("TurnChanged", OnTurnChanged);

        await _hubConnection.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }

    private async Task OnChatMessageReceived(ChatMessageEventArgs e)
    {
        if (ChatMessageReceived is not null)
        {
            await ChatMessageReceived.Invoke(e);
        }
    }

    private async Task OnCombatantAdded(CombatantAddedEventArgs e)
    {
        if (CombatantAdded is not null)
        {
            await CombatantAdded.Invoke(e);
        }
    }

    private async Task OnCombatantRemoved(CombatantRemovedEventArgs e)
    {
        if (CombatantRemoved is not null)
        {
            await CombatantRemoved.Invoke(e);
        }
    }

    private async Task OnCombatantUpdated(CombatantUpdatedEventArgs e)
    {
        if (CombatantUpdated is not null)
        {
            await CombatantUpdated.Invoke(e);
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

    private async Task OnTurnChanged(TurnChangedEventArgs e)
    {
        if (TurnChanged is not null)
        {
            await TurnChanged.Invoke(e);
        }
    }
}