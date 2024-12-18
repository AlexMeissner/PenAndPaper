using DataTransfer.Mouse;
using static Website.Services.ServiceExtension;

namespace Website.Services.API;

public interface IMouseApi
{
    public Task<HttpResponse> PostAsync(MouseMoveEventArgs payload);
}

[TransistentService]
public class MouseApi(IEndPointProvider endPointProvider, IIdentityProvider identityProvider) : IMouseApi
{
    private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Mouse", identityProvider);

    public Task<HttpResponse> PostAsync(MouseMoveEventArgs payload)
    {
        return _request.PostAsync(payload);
    }
}