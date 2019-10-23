using System.IO;
using System.Security.Cryptography;
using FI.API.SignTool.Helpers;
using FI.API.SignTool.SigningProviders.Interfaces;

namespace FI.API.SignTool.SigningProviders
{
    public class PrivateKeyFileSigningProvider : ISigningProvider
    {
        private readonly RSACryptoServiceProvider _privateKey;
        public string FileName { get; set; }

        public PrivateKeyFileSigningProvider(string fileName)
        {
            FileName = fileName;

            _privateKey = CertificateHelper.ImportPrivateKey(File.ReadAllText(FileName));
        }

        public byte[] SignHash(byte[] bytes)
        {
            var signedBytes = _privateKey.SignHash(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return signedBytes;
        }
    }
}
