using DataTransfer.Mouse;
using static Website.Services.ServiceExtension;

namespace Website.Services.API;

public interface IMouseApi
{
    public Task<HttpResponse> PostAsync(MouseMoveEventArgs payload);
}

[TransistentService]
public class MouseApi(IEndPointProvider endPointProvider, ITokenProvider tokenProvider) : IMouseApi
{
    private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Mouse", tokenProvider);

    public Task<HttpResponse> PostAsync(MouseMoveEventArgs payload)
    {
        return _request.PostAsync(payload);
    }
}