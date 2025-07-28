using System;
using System.Collections.Generic;
using System.Linq;
using FI.API.SignTool.Parameters.Interfaces;

namespace FI.API.SignTool.SigningProviders;

public class KeyVaultConnectionString : IValidatable
{
    public const string DefaultAlgorithm = "RS256";

    public string? Url { get; init;  }
    public string? KeyName { get; init; }
    public string? KeyVersion { get; init; }
    public string? Algorithm { get; init; }
    public string? TenantId { get; init; }
    public string? ClientId { get; init; }
    public string? ClientSecret { get; init; }

    public bool HasClientCredentials => 
        !string.IsNullOrWhiteSpace(TenantId) 
        && !string.IsNullOrWhiteSpace(ClientId) 
        && !string.IsNullOrWhiteSpace(ClientSecret);

    public KeyVaultConnectionString(string? connectionString)
    {
        var dict = (connectionString ?? string.Empty)
            .Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split("=".ToCharArray()))
            .ToDictionary(x => x.First().ToLower(), x => string.Join("=", x.Skip(1)));

        Url = dict.GetValueOrDefault(nameof(Url).ToLowerInvariant());
        KeyName = dict.GetValueOrDefault(nameof(KeyName).ToLowerInvariant());
        KeyVersion = dict.GetValueOrDefault(nameof(KeyVersion).ToLowerInvariant());
        Algorithm = dict.GetValueOrDefault(nameof(Algorithm).ToLowerInvariant()) ?? DefaultAlgorithm;
        TenantId = dict.GetValueOrDefault(nameof(TenantId).ToLowerInvariant());
        ClientId = dict.GetValueOrDefault(nameof(ClientId).ToLowerInvariant());
        ClientSecret = dict.GetValueOrDefault(nameof(ClientSecret).ToLowerInvariant());
    }

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Url))
            throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(Url)}", nameof(Url));

        if (string.IsNullOrWhiteSpace(KeyName))
            throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(KeyName)}", nameof(KeyName));

        var clientCredentialsProvided = new[] { TenantId, ClientId, ClientSecret }
            .Any(argument => !string.IsNullOrWhiteSpace(argument));

        if (clientCredentialsProvided)
        {
            if (string.IsNullOrWhiteSpace(TenantId))
                throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(TenantId)}", nameof(TenantId));

            if (string.IsNullOrWhiteSpace(ClientId))
                throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(ClientId)}", nameof(ClientId));

            if (string.IsNullOrWhiteSpace(ClientSecret))
                throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(ClientSecret)}", nameof(ClientSecret));
        }
    }
}
