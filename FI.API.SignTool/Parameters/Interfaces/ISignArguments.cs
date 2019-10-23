using FI.API.SignTool.Types;

namespace FI.API.SignTool.Parameters.Interfaces
{
    public interface ISignArguments : IBaseArguments, IValidatable
    {
        SigningProviderType SigningProvider { get; set; }
        string PrivateKeyFileName { get; set; }
        string AzureKeyVaultConnectionString { get; set; }
    }
}