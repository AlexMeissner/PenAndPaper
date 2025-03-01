using DataTransfer.Dice;
using DataTransfer.Response;

namespace ApiClient;

public interface IRollApi
{
    Task<Response> RollAsync(int campaignId, RollDiceDto payload);
}

public class RollApi(IRequest request) : IRollApi
{
    public Task<Response> RollAsync(int campaignId, RollDiceDto payload)
    {
        return request.Path("campaigns", campaignId, "rolls").PostAsync(payload);
    }
}