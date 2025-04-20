using System.Threading.Channels;
using Backend.Extensions;
using Backend.Hubs;
using DataTransfer.Chat;
using DataTransfer.Dice;
using DataTransfer.Grid;
using DataTransfer.Map;
using DataTransfer.Mouse;
using DataTransfer.Sound;
using DataTransfer.Token;
using Microsoft.AspNetCore.SignalR;

namespace Backend.Services;

public class CampaignUpdateBackground(
    IHubContext<CampaignUpdateHub, ICampaignUpdate> campaignUpdateHub,
    Channel<ChatMessageEventArgs> chatMessageChannel,
    Channel<DiceRolledEventArgs> diceRollChannel,
    Channel<GridChangedEventArgs> gridChannel,
    Channel<MapChangedEventArgs> mapChannel,
    Channel<MapCollectionChangedEventArgs> mapCollectionChannel,
    Channel<MouseMoveEventArgs> mouseChannel,
    Channel<SoundStartedEventArgs> soundStartedChannel,
    Channel<SoundStoppedEventArgs> soundStoppedChannel,
    Channel<TokenAddedEventArgs> tokenAddedChannel,
    Channel<TokenMovedEventArgs> tokenMovedChannel) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            while (chatMessageChannel.Reader.TryRead(out var chatMessage))
            {
                const int campaignId = 1;
                await campaignUpdateHub.Clients.AllInCampaign(campaignId).ChatMessageReceived(chatMessage);

                //await campaignUpdateHub.Clients.All.ChatMessageReceived(chatMessage);
            }
        }
    }
}