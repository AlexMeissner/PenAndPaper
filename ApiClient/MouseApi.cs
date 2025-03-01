using DataTransfer.Mouse;
using DataTransfer.Response;

namespace ApiClient;

public interface IMouseApi
{
    Task<Response> SetIndicatorAsync(int campaignId, MouseIndicatorDto payload);
}

public class MouseApi(IRequest request) : IMouseApi
{
    public Task<Response> SetIndicatorAsync(int campaignId, MouseIndicatorDto payload)
    {
        return request.Path("campaigns", campaignId, "mouse-indicators").PostAsync(payload);
    }
}