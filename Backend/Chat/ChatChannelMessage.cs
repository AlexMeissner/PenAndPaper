using DataTransfer.Chat;

namespace Backend.Chat;

public record ChatChannelMessage(int CampaignId, int SenderId, string SenderUsername, string? Image, ChatMessageDto ChatMessage);