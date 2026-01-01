using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Website.Services;

public class UserClaimsCircuitHandler(IUserClaims userClaims) : CircuitHandler
{
    public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        await userClaims.InitializeAsync();
    }
}