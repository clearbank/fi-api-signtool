using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FI.API.SignTool.Helpers
{
    public class CryptographyHelper
    {
        public static byte[] GenerateDigitalSignature(string body, string privateKeyText)
        {
            var hash = HashBody(body);

            using var rsa = ImportPem(privateKeyText);
            var signedHash = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return signedHash;
        }

        public static bool VerifyDigitalSignature(string digitalSignature, string contents, string publicKeyText)
        {
            var hash = HashBody(contents);

            bool verified;
            try
            {
                using var rsa = ImportPem(publicKeyText);
                verified = rsa.VerifyHash(hash, Convert.FromBase64String(digitalSignature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
            catch
            {
                verified = false;
            }

            return verified;
        }

        internal static byte[] HashBody(string content)
        {
            var contentBytes = Encoding.UTF8.GetBytes(content);

            using var provider = SHA256.Create();
            var hash = provider.ComputeHash(contentBytes);

            return hash;
        }

        public static RSA ImportPem(string pem)
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
}
