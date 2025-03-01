using DataTransfer.Chat;
using DataTransfer.Response;

namespace ApiClient;

public interface IChatApi
{
    Task<Response<IEnumerable<ChatUserDto>>> GetUsers(int campaignId);
    Task<Response> SendMessageAsync(int campaignId, ChatMessageDto payload);
}

public class ChatApi(IRequest request) : IChatApi
{
    public Task<Response<IEnumerable<ChatUserDto>>> GetUsers(int campaignId)
    {
        return request.Path("campaigns", campaignId, "chat-users").GetAsync<IEnumerable<ChatUserDto>>();
    }

    public Task<Response> SendMessageAsync(int campaignId, ChatMessageDto payload)
    {
        return request.Path("campaigns", campaignId, "chat").PostAsync(payload);
    }
}