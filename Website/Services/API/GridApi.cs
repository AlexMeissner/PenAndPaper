using DataTransfer.Grid;

namespace Website.Services.API;

public interface IGridApi
{
    public Task<HttpResponse> PutAsync(GridUpdateDto payload);
}

[ServiceExtension.TransistentService]
public class GridApi(IEndPointProvider endPointProvider, ITokenProvider tokenProvider) : IGridApi
{
    private readonly HttpRequest _request = new(endPointProvider.BaseURL + "Grid", tokenProvider);

    public Task<HttpResponse> PutAsync(GridUpdateDto payload)
    {
        return _request.PutAsync(payload);
    }
}