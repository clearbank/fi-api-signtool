using System;
using FI.API.SignTool.Parameters.Interfaces;
using FI.API.SignTool.SigningProviders.Interfaces;
using FI.API.SignTool.Types;

namespace FI.API.SignTool.SigningProviders
{
    public class SigningProviderFactory
    {
        public static ISigningProvider GetSigningProvider(ISignArguments arguments)
        {
            switch (arguments.SigningProvider)
            {
                case SigningProviderType.FileName:
                    return new PrivateKeyFileSigningProvider(arguments.PrivateKeyFileName);

                case SigningProviderType.AzureKeyVault:
                    return new AzureKeyVaultSigningProvider(arguments.AzureKeyVaultConnectionString);

                default:
                    throw new ArgumentOutOfRangeException($"Unable to locate {nameof(ISigningProvider)} for {arguments.SigningProvider}");
            }
        }
    }
}
