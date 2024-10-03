using DataTransfer.Dice;
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
    Task DiceRolled(DiceRolledEventArgs diceRoll);
}
