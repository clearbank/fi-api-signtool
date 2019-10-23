using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace FI.API.SignTool.Helpers
{
    public class CertificateHelper
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

        private static byte[] HashBody(string content)
        {
            var contentBytes = Encoding.UTF8.GetBytes(content);

            using (var provider = new SHA256Managed())
            {
                var hash = provider.ComputeHash(contentBytes);

                return hash;
            }
        }

        public static RSACryptoServiceProvider ImportPrivateKey(string pem)
        {
            var pr = new PemReader(new StringReader(pem));
            var rsaParameters = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)pr.ReadObject());
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParameters);
            return csp;
        }

        public static RSACryptoServiceProvider ImportPublicKey(string pem)
        {
            var pr = new PemReader(new StringReader(pem));
            var publicKey = (AsymmetricKeyParameter)pr.ReadObject();
            var rsaParameters = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);
            var csp = new RSACryptoServiceProvider();
            csp.ImportParameters(rsaParameters);
            return csp;
        }
    }
}
