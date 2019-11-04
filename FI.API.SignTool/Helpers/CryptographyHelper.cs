using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace FI.API.SignTool.Helpers
{
    public class CryptographyHelper
    {
        public static byte[] GenerateDigitalSignature(string body, string privateKeyText)
        {
            var hash = HashBody(body);

            byte[] signedHash;
            using (var privateKey = ImportPrivateKey(privateKeyText))
            {
                signedHash = privateKey.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }

            return signedHash;
        }

        public static bool VerifyDigitalSignature(string digitalSignature, string contents, string publicKeyText)
        {
            var hash = HashBody(contents);

            bool verified;
            try
            {
                using (var publicKey = ImportPublicKey(publicKeyText))
                {
                    verified = publicKey.VerifyHash(hash, Convert.FromBase64String(digitalSignature), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
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

            using (var provider = new SHA256Managed())
            {
                var hash = provider.ComputeHash(contentBytes);

                return hash;
            }
        }

        public static RSACryptoServiceProvider ImportPrivateKey(string privateKeyPEM)
        {
            var pr = new PemReader(new StringReader(privateKeyPEM));
            var privateKey = (RsaPrivateCrtKeyParameters) pr.ReadObject();
            if (privateKey == null)
                throw new ArgumentException("Invalid Private Key", nameof(privateKeyPEM));
            var rsaParameters = DotNetUtilities.ToRSAParameters(privateKey);
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParameters);
            return csp;
        }

        public static RSACryptoServiceProvider ImportPublicKey(string publicKeyPEM)
        {
            var pr = new PemReader(new StringReader(publicKeyPEM));
            var publicKey = (RsaKeyParameters)pr.ReadObject();
            if (publicKey == null)
                throw new ArgumentException("Invalid Public Key", nameof(publicKeyPEM));
            var rsaParameters = DotNetUtilities.ToRSAParameters(publicKey);
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParameters);
            return csp;
        }
    }
}
