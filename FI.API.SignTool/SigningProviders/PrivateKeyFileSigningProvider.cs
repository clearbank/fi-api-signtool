using System;
using System.IO;
using System.Security.Cryptography;
using FI.API.SignTool.SigningProviders.Interfaces;

namespace FI.API.SignTool.SigningProviders;

public class PrivateKeyFileSigningProvider : ISigningProvider
{
    private readonly RSA _rsa;

    public PrivateKeyFileSigningProvider(string? fileName)
    {
        ArgumentNullException.ThrowIfNull(fileName, nameof(fileName));
        
        _rsa = ImportPem(File.ReadAllText(fileName));
    }

    public byte[] SignHash(byte[] bytes)
    {
        return _rsa.SignHash(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }

    private static RSA ImportPem(string pem)
    {
        var rsa = RSA.Create();

        try
        {
            rsa.ImportFromPem(pem);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Invalid PEM", nameof(pem), ex);
        }

        return rsa;
    }
}
