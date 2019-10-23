using System;
using System.Threading.Tasks;
using FI.API.SignTool.SigningProviders.Interfaces;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace FI.API.SignTool.SigningProviders
{
    public class AzureKeyVaultSigningProvider : ISigningProvider
    {
        private readonly KeyVaultConnectionString _connectionString;

        public AzureKeyVaultSigningProvider(string connectionString)
        {
            _connectionString = new KeyVaultConnectionString(connectionString);
            _connectionString.Validate();
        }

        private KeyVaultClient GetClient()
        {
            var keyVaultClient = new KeyVaultClient(GetToken);

            return keyVaultClient;
        }

        protected async Task<string> GetToken(string authority, string resource, string scope)
        {
            var authContext = new AuthenticationContext(authority);

            var clientCred = new ClientCredential(
                _connectionString.ClientId,
                _connectionString.ClientSecret
            );

            var result = await authContext.AcquireTokenAsync(resource, clientCred);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the KeyVault JWT token");
            }

            return result.AccessToken;
        }

        public byte[] SignHash(byte[] bytes)
        {
            var client = GetClient();

            var signedBytes = SignAsync(client, bytes)
                .GetAwaiter()
                .GetResult();

            return signedBytes.Result;
        }

        public async Task<KeyOperationResult> SignAsync(KeyVaultClient client, byte[] bytes)
        {
            KeyOperationResult result;

            if (_connectionString.HasKeyName)
            {
                result = await client.SignAsync(
                    _connectionString.Url,
                    _connectionString.KeyName,
                    _connectionString.KeyVersion,
                    _connectionString.Algorithm,
                    bytes
                );
            }
            else
            {
                result = await client.SignAsync(
                    _connectionString.Url,
                    _connectionString.Algorithm,
                    bytes
                );
            }

            return result;
        }
    }
}
