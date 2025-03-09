using DataTransfer.Chat;
using DataTransfer.Response;

namespace ApiClient;

public interface IChatApi
{
    Task<Response<IEnumerable<ChatUserDto>>> GetUsers(int campaignId);
    Task<Response> SendMessageAsync(int campaignId, ChatMessageDto payload);
}

public class ChatApi(IRequestBuilder requestBuilder) : IChatApi
{
    public Task<Response<IEnumerable<ChatUserDto>>> GetUsers(int campaignId)
    {
        return requestBuilder.Path("campaigns", campaignId, "chat-users").GetAsync<IEnumerable<ChatUserDto>>();
    }

    public Task<Response> SendMessageAsync(int campaignId, ChatMessageDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "chat").PostAsync(payload);
    }
}