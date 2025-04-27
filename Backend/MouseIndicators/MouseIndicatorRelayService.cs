using Backend.Extensions;
using Backend.Hubs;
using DataTransfer.Mouse;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace Backend.MouseIndicators;

public class MouseIndicatorRelayService(
    IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub,
    Channel<MouseIndicatorChannelMessage> mouseIndicatorMessageChannel) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await mouseIndicatorMessageChannel.Reader.WaitToReadAsync(stoppingToken))
        {
            var (campaignId, position, color) = await mouseIndicatorMessageChannel.Reader.ReadAsync(stoppingToken);

            var eventArgs = new MouseMoveEventArgs(position, color);

            await campaignUpdateHub.Clients.AllInCampaign(campaignId).MouseMoved(eventArgs);
        }
    }
}
