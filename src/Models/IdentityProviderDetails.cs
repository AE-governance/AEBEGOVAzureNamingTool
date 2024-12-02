using System.Security.Claims;

namespace AzureNamingTool.Models
{
    /// <summary>
    /// Represents the details of an identity provider.
    /// </summary>
    public class IdentityProviderDetails
    {
        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public string CurrentUser { get; set; } = "System";

        /// <summary>
        /// Gets or sets the current identity provider.
        /// </summary>
        public string CurrentIdentityProvider { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the current claims principal.
        /// </summary>
        public ClaimsPrincipal? CurrentClaimsPrincipal { get; set; }

        /// <summary>
        /// Gets a flag indicating whether the current user is an admin.
        /// </summary>
        /// <remarks>
        /// Returns true if the CurrentClaimsPrincipal is not null and the user has the admin claim.
        /// </remarks>
        // TODO set correct admin claim value
        public bool IsAdmin { get => CurrentClaimsPrincipal != null && CurrentClaimsPrincipal.HasClaim(c => c.Type == "roles" && c.Value == "admin"); }
    }
}
