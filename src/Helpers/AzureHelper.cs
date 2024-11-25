using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace AzureNamingTool.Helpers
{
    /// <summary>
    /// Provides helper methods for interacting with Azure services.
    /// </summary>
    public static class AzureHelper
    {
        /// <summary>
        /// Retrieves a secret from an Azure Key Vault.
        /// </summary>
        /// <param name="tenantId">The tenant ID to use for authentication.</param>
        /// <param name="vaultUri">The URI of the Azure Key Vault.</param>
        /// <param name="secretName">The name of the secret to retrieve.</param>
        /// <returns>The value of the secret.</returns>
        /// <exception cref="Azure.RequestFailedException">Thrown when the request to Azure Key Vault fails.</exception>
        public static string GetKeyVaultSecret(string tenantId, string vaultUri, string secretName)
        {
            DefaultAzureCredentialOptions options = new()
            {
                // Specify the tenant ID to use the dev credentials when running the app locally
                // in Visual Studio.
                VisualStudioTenantId = tenantId,
                SharedTokenCacheTenantId = tenantId
            };

            var client = new SecretClient(new Uri(vaultUri), new DefaultAzureCredential(options));
            var secret = client.GetSecretAsync(secretName).Result;

            return secret.Value.Value;
        }
    }
}
