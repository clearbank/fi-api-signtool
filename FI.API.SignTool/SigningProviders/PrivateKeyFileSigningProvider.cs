using System.IO;
using System.Security.Cryptography;
using FI.API.SignTool.Helpers;
using FI.API.SignTool.SigningProviders.Interfaces;

namespace FI.API.SignTool.SigningProviders
{
    public class PrivateKeyFileSigningProvider : ISigningProvider
    {
        private readonly RSA _rsa;
        public string FileName { get; set; }

        public PrivateKeyFileSigningProvider(string fileName)
        {
            FileName = fileName;

            _rsa = CryptographyHelper.ImportPem(File.ReadAllText(FileName));
        }

        public byte[] SignHash(byte[] bytes)
        {
            var signedBytes = _rsa.SignHash(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return signedBytes;
        }
    }
}
