using DataTransfer.Mouse;
using DataTransfer.Response;

namespace ApiClient;

public interface IMouseApi
{
    Task<Response> SetIndicatorAsync(int campaignId, MouseIndicatorDto payload);
}

public class MouseApi(IRequestBuilder requestBuilder) : IMouseApi
{
    public Task<Response> SetIndicatorAsync(int campaignId, MouseIndicatorDto payload)
    {
        return requestBuilder.Path("campaigns", campaignId, "mouse-indicators").PostAsync(payload);
    }
}