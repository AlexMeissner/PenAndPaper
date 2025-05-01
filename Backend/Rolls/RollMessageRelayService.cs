using Backend.Extensions;
using Backend.Hubs;
using Backend.Services;
using DataTransfer.Dice;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace Backend.Rolls;

public class RollMessageRelayService(
    IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub,
    IUserConnectionTracker userConnectionTracker,
    Channel<RollChannelMessage> channel) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await channel.Reader.WaitToReadAsync(stoppingToken))
        {
            var message = await channel.Reader.ReadAsync(stoppingToken);

            var eventArgs = new DiceRolledEventArgs(message.Name, message.Result);

            if (message.ReceiverId is { } receiverId)
            {
                if (campaignUpdateHub.Clients.UserInCampaign(message.CampaignId, receiverId, userConnectionTracker) is { } receiverClient)
                {
                    await receiverClient.DiceRolled(eventArgs);
                }

                if (campaignUpdateHub.Clients.UserInCampaign(message.CampaignId, message.SenderId, userConnectionTracker) is { } senderClient)
                {
                    await senderClient.DiceRolled(eventArgs);
                }
            }
            else
            {
                await campaignUpdateHub.Clients.AllInCampaign(message.CampaignId).DiceRolled(eventArgs);
            }
        }
    }
}
