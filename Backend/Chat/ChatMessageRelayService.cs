using Backend.Extensions;
using Backend.Hubs;
using Backend.Services;
using DataTransfer.Chat;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Channels;

namespace Backend.Chat;

public class ChatMessageRelayService(
    IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub,
    IUserConnectionTracker userConnectionTracker,
    Channel<ChatChannelMessage> chatMessageChannel) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (await chatMessageChannel.Reader.WaitToReadAsync(stoppingToken))
        {
            var (campaignId, senderId, username, image, chatMessage) = await chatMessageChannel.Reader.ReadAsync(stoppingToken);

            var isPrivate = chatMessage.ReceiverId is not null;

            var receiverMessageEventArgs = new ChatMessageEventArgs
            (
                DateTime.UtcNow,
                ChatMessageType.Message,
                MessageDirection.Received,
                username,
                chatMessage.Text,
                image,
                isPrivate
            );

            if (chatMessage.ReceiverId is { } receiverId)
            {
                if (campaignUpdateHub.Clients.UserInCampaign(campaignId, receiverId, userConnectionTracker) is
                    { } receiverClient)
                {
                    await receiverClient.ChatMessageReceived(receiverMessageEventArgs);
                }
            }
            else
            {
                await campaignUpdateHub.Clients
                    .AllInCampaignExcept(campaignId, senderId, userConnectionTracker)
                    .ChatMessageReceived(receiverMessageEventArgs);
            }

            var senderMessageEventArgs = receiverMessageEventArgs with
            {
                Image = null,
                Direction = MessageDirection.Sent,
            };

            if (campaignUpdateHub.Clients.UserInCampaign(campaignId, senderId, userConnectionTracker) is
                { } senderClient)
            {
                await senderClient.ChatMessageReceived(senderMessageEventArgs);
            }
        }
    }
}