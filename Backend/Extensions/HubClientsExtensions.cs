using Backend.Services;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Extensions;

public static class HubClientsExtensions
{
    public static T AllInCampaign<T>(this IHubClients<T> hubClients, int campaignId) where T : class
    {
        var id = $"campaign_{campaignId}";
        return hubClients.Group(id);
    }

    public static T AllInCampaignExcept<T>(this IHubClients<T> hubClients, int campaignId, int userId,
        IUserConnectionTracker userConnectionTracker) where T : class
    {
        var id = $"campaign_{campaignId}";
        var connectionId = userConnectionTracker.GetConnectionId(userId);
        return connectionId is null ? hubClients.Group(id) : hubClients.GroupExcept(id, connectionId);
    }

    public static T? UserInCampaign<T>(this IHubClients<T> hubClients, int campaignId, int userId,
        IUserConnectionTracker userConnectionTracker) where T : class
    {
        var connectionId = userConnectionTracker.GetConnectionId(userId);

        return connectionId is null ? null : hubClients.Client(connectionId);
    }
}