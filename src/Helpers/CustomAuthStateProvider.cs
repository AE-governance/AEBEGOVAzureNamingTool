using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace AzureNamingTool.Helpers
{
    /// <summary>
    /// Custom authentication state provider for handling user authentication state.
    /// </summary>
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        /// <summary>
        /// Gets the current authentication state asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the <see cref="AuthenticationState"/>.</returns>
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        /// <summary>
        /// Authenticates the user with the specified user identifier.
        /// </summary>
        /// <param name="userIdentifier">The identifier of the user to authenticate.</param>
        public void AuthenticateUser(string userIdentifier)
        {
            var identity = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, userIdentifier),
        ], "Custom Authentication");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(
                Task.FromResult(new AuthenticationState(user)));
        }


        /// <summary>
        /// Checks if the user is an administrator.
        /// </summary>
        public async Task<bool> IsAdmin() {
            AuthenticationState authState = await GetAuthenticationStateAsync();
            return authState.User.IsInRole("Admin");
        }
    }
}
