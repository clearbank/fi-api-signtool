namespace FI.API.SignTool.SigningProviders.Interfaces
{
    public interface ISigningProvider
    {
        byte[] SignHash(byte[] bytes);
    }
}
