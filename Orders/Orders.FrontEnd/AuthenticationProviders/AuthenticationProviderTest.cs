using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Orders.FrontEnd.AuthenticationProviders
{
    public class AuthenticationProviderTest : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonimous = new ClaimsIdentity();
            var user = new ClaimsIdentity(authenticationType: "test");
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
        }
    }
}