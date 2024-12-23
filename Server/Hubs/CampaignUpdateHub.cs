using DataTransfer.Dice;
using DataTransfer.Grid;
using DataTransfer.Map;
using DataTransfer.Mouse;
using DataTransfer.Token;
using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public class CampaignUpdateHub : Hub<ICampaignUpdate>
{
    public override Task OnConnectedAsync()
    {
        return base.OnConnectedAsync();
    }
}

public interface ICampaignUpdate
{
    Task DiceRolled(DiceRolledEventArgs e);
    Task GridChanged(GridChangedEventArgs e);
    Task MapChanged(MapChangedEventArgs e);
    Task MapCollectionChanged(MapCollectionChangedEventArgs e);
    Task MouseMoved(MouseMoveEventArgs e);
    Task TokenAdded(TokenAddedEventArgs e);
    Task TokenMoved(TokenMovedEventArgs e);
}