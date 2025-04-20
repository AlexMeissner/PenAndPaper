using System.Collections.Concurrent;

namespace Backend.Services;

public interface IUserConnectionTracker
{
    void Add(int userId, int campaignId, string connectionId);
    void Remove(string connectionId);
    string? GetConnectionId(int userId);
    string? GetConnectionId(int campaignId, int userId);
}

public class UserConnectionTracker : IUserConnectionTracker
{
    private record ConnectionDetails(int CampaignId, string ConnectionId);

    private readonly ConcurrentDictionary<int, ConnectionDetails> _connections = [];

    public void Add(int userId, int campaignId, string connectionId)
    {
        _connections.TryAdd(userId, new ConnectionDetails(campaignId, connectionId));
    }

    public void Remove(string connectionId)
    {
        var connections = _connections.Where(c => c.Value.ConnectionId == connectionId);

        foreach (var connection in connections)
        {
            _connections.TryRemove(connection.Key, out _);
        }
    }

    public string? GetConnectionId(int userId)
    {
        return _connections.TryGetValue(userId, out var connectionDetails) ? connectionDetails.ConnectionId : null;
    }

    public string? GetConnectionId(int campaignId, int userId)
    {
        return _connections.TryGetValue(userId, out var connectionDetails) && connectionDetails.CampaignId == campaignId
            ? connectionDetails.ConnectionId
            : null;
    }
}