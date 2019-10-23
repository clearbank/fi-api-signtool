using CommandLine;
using FI.API.SignTool.Exceptions;
using FI.API.SignTool.Parameters.Interfaces;
using FI.API.SignTool.Types;

namespace FI.API.SignTool.Parameters
{
    [Verb(nameof(CommandType.HashSignEncode), HelpText = "Hash, Sign and Encode input data")]
    public class HashSignEncodeArguments : IHashSignEncodeArguments
    {
        [Option('d', "data", Required = false, HelpText = "The data to be processed")]
        public string Data { get; set; }

        [Option('f', "datafilename", Required = false, HelpText = "The filename containing data to be processed")]
        public string DataFileName { get; set; }

        public string InputData => ArgumentsHelper.GetDataOrFileContent(Data, DataFileName);
        [Option('p', "provider", Required = false, Default = SigningProviderType.FileName, HelpText = "The Provider to use for Signing requests")]
        public SigningProviderType SigningProvider { get; set; }

        [Option('k', "keyfilename", Required = false, Default = null, HelpText = "FileName for Private Key")]
        public string PrivateKeyFileName { get; set; }

        [Option('v', "keyvault", Required = false, Default = null, HelpText = "Azure KeyVault connection string")]
        public string AzureKeyVaultConnectionString { get; set; }

        public virtual void Validate()
        {
            switch (SigningProvider)
            {
                case SigningProviderType.FileName:
                    if (string.IsNullOrWhiteSpace(PrivateKeyFileName))
                        throw new CommandLineParserException($"Must specify {nameof(PrivateKeyFileName)} for {nameof(SigningProvider)} {SigningProvider}");
                    break;

                case SigningProviderType.AzureKeyVault:
                    if (string.IsNullOrWhiteSpace(AzureKeyVaultConnectionString))
                        throw new CommandLineParserException($"Must specify {nameof(AzureKeyVaultConnectionString)} for {nameof(SigningProvider)} {SigningProvider}");
                    break;
            }
        }
    }
}
