using DataTransfer.Types;

namespace Backend.MouseIndicators;

public record MouseIndicatorChannelMessage(int CampaignId, Vector2D Position, Vector3D Color);
