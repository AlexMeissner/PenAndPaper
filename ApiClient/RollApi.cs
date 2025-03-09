using DataTransfer.Dice;
using DataTransfer.Response;

namespace ApiClient;

public interface IRollApi
{
    Task<Response> RollAsync(int campaignId, RollDiceDto payload);
}

public class RollApi(IRequestBuilder requestBuilder) : IRollApi
{
    public Task<Response> RollAsync(int campaignId, RollDiceDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "rolls").PostAsync(payload);
    }
}