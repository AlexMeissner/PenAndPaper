using System.Collections.Concurrent;

namespace Website.Events;

public interface ICampaignEventHub
{
    CampaignDispatcher ForCampaign(int campaignId);
}

public class CampaignEventHub : ICampaignEventHub
{
    private readonly ConcurrentDictionary<int, CampaignDispatcher> _dispatchers = new();

    public CampaignDispatcher ForCampaign(int campaignId) => _dispatchers.GetOrAdd(campaignId, _ => new CampaignDispatcher());
}
