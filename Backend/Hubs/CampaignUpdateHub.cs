using Backend.Services;
using DataTransfer.Chat;
using DataTransfer.Dice;
using DataTransfer.Grid;
using DataTransfer.Initiative;
using DataTransfer.Map;
using DataTransfer.Mouse;
using DataTransfer.Sound;
using DataTransfer.Token;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Hubs;

public interface ICampaignUpdate
{
    Task ChatMessageReceived(ChatMessageEventArgs e);
    Task CombatantAdded(CombatantAddedEventArgs e);
    Task CombatantRemoved(CombatantRemovedEventArgs e);
    Task CombatantUpdated(CombatantUpdatedEventArgs e);
    Task DiceRolled(DiceRolledEventArgs e);
    Task GridChanged(GridChangedEventArgs e);
    Task MapChanged(MapChangedEventArgs e);
    Task MouseMoved(MouseMoveEventArgs e);
    Task SoundStarted(SoundStartedEventArgs e);
    Task SoundStopped(SoundStoppedEventArgs e);
    Task TokenAdded(TokenAddedEventArgs e);
    Task TokenMoved(TokenMovedEventArgs e);
    Task TurnChanged(TurnChangedEventArgs e);
}

public class CampaignUpdateHub(
    ILogger<CampaignUpdateHub> logger,
    IIdentity identity,
    IUserConnectionTracker userConnectionTracker) : Hub<ICampaignUpdate>
{
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();

        if (httpContext is null)
        {
            logger.LogError("Http context not accessible during hub connection");
            Context.Abort();
            return;
        }

        var campaignIdString = httpContext.Request.RouteValues["campaignId"]?.ToString();

        if (string.IsNullOrEmpty(campaignIdString))
        {
            logger.LogError("Hub connection without campaign id");
            Context.Abort();
            return;
        }

        if (!int.TryParse(campaignIdString, out var campaignId))
        {
            logger.LogError("Campaign id is not an integer");
            Context.Abort();
            return;
        }

        var identityClaims = await identity.FromClaimsPrincipal(httpContext.User);

        if (identityClaims is null)
        {
            logger.LogWarning("Unauthorized user on hub connection");
            Context.Abort();
            return;
        }

        userConnectionTracker.Add(identityClaims.User.Id, campaignId, Context.ConnectionId);

        var groupId = $"campaign_{campaignId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        userConnectionTracker.Remove(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}