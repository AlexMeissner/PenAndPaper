using System.Collections.Concurrent;

namespace Website.Events;

public interface ICampaignSubscription : IDisposable;

public sealed class CampaignDispatcher
{
    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<Guid, Func<ICampaignEvent, ValueTask>>> _handlers = new();

    public ICampaignSubscription Subscribe<T>(Func<T, ValueTask> handler) where T : ICampaignEvent
    {
        var typedHandlers = _handlers.GetOrAdd(
            typeof(T),
            _ => new ConcurrentDictionary<Guid, Func<ICampaignEvent, ValueTask>>());

        var id = Guid.NewGuid();
        typedHandlers[id] = msg => handler((T)msg);

        return new Subscription(() => typedHandlers.TryRemove(id, out _));
    }

    public void Publish(ICampaignEvent message)
    {
        if (!_handlers.TryGetValue(message.GetType(), out var handlers))
            return;

        foreach (var handler in handlers.Values)
        {
            // Fire-and-forget, fully concurrent
            _ = Task.Run(() => handler(message));
        }
    }

    private sealed class Subscription(Action dispose) : ICampaignSubscription
    {
        private int _disposed;

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 0)
            {
                dispose();
            }
        }
    }
}
