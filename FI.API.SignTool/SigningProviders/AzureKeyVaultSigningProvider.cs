using System;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using FI.API.SignTool.SigningProviders.Interfaces;

namespace FI.API.SignTool.SigningProviders;

public class AzureKeyVaultSigningProvider : ISigningProvider
{
    private readonly KeyVaultConnectionString _connectionString;

    public AzureKeyVaultSigningProvider(string? connectionString)
    {
        _connectionString = new KeyVaultConnectionString(connectionString);
        _connectionString.Validate();
    }

    public byte[] SignHash(byte[] bytes)
    {
        var credential = GetTokenCredential();

        var keyClient = new KeyClient(new Uri(_connectionString.Url!), credential);
        var key = keyClient.GetKey(_connectionString.KeyName, _connectionString.KeyVersion);

        var cryptoClient = new CryptographyClient(key.Value.Id, credential);
        var signResult = cryptoClient.Sign(_connectionString.Algorithm, bytes);

        return signResult.Signature;
    }

    private TokenCredential GetTokenCredential() => _connectionString.HasClientCredentials
        ? new ClientSecretCredential(_connectionString.TenantId, _connectionString.ClientId, _connectionString.ClientSecret)
        : new DefaultAzureCredential();
}
