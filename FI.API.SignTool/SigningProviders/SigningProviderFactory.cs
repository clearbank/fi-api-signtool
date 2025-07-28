using FI.API.SignTool.Parameters.Interfaces;
using FI.API.SignTool.SigningProviders.Interfaces;
using FI.API.SignTool.Types;
using System;

namespace FI.API.SignTool.SigningProviders;

public class SigningProviderFactory
{
    public static ISigningProvider GetSigningProvider(ISignArguments arguments) => arguments.SigningProvider switch
    {
        SigningProviderType.FileName => new PrivateKeyFileSigningProvider(arguments.PrivateKeyFileName),
        SigningProviderType.AzureKeyVault => new AzureKeyVaultSigningProvider(arguments.AzureKeyVaultConnectionString),
        _ => throw new ArgumentOutOfRangeException(nameof(arguments), $"Unable to locate {nameof(ISigningProvider)} for {arguments.SigningProvider}"),
    };
}
