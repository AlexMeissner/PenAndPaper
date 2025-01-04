using DataTransfer.Chat;
using static Website.Services.ServiceExtension;

namespace Website.Services.API;

public interface IChatApi
{
    public Task<HttpResponse<ChatUsersDto>> GetAsync(int campaignId);
    public Task<HttpResponse> PostAsync(ChatMessageDto payload);
}

[TransistentService]
public class ChatApi(IEndPointProvider endPointProvider, ITokenProvider tokenProvider)
    : IChatApi
{
    private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Chat", tokenProvider);

    public Task<HttpResponse<ChatUsersDto>> GetAsync(int campaignId)
    {
        return _request.GetAsync<ChatUsersDto>($"campaignId={campaignId}");
    }

    public Task<HttpResponse> PostAsync(ChatMessageDto payload)
    {
        return _request.PostAsync(payload);
    }
}