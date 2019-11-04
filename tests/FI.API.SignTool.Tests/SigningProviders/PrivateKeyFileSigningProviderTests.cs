using System;
using System.IO;
using FI.API.SignTool.SigningProviders;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.SigningProviders
{
    public class PrivateKeyFileSigningProviderTests
    {
        [Fact]
        public void PrivateKeyFileSigningProvider_can_construct_with_valid_private_key_file()
        {
            // Act
            var result = new PrivateKeyFileSigningProvider(KnownData.KnownPrivateKeyFileName);

            // Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public void PrivateKeyFileSigningProvider_fails_to_construct_with_invvalid_private_key_file()
        {
            // Arrange
            var tempFileName = Path.GetTempFileName();
            File.WriteAllText(tempFileName, Guid.NewGuid().ToString());

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => new PrivateKeyFileSigningProvider(tempFileName)
            );

            // Assert
            ex.ShouldNotBeNull();
            ex.ParamName.ShouldBe("privateKeyPEM");

            File.Delete(tempFileName);
        }

        [Fact]
        public void SignHash_can_generate_the_correct_hash()
        {
            // Arrange
            var signingProvider = new PrivateKeyFileSigningProvider(KnownData.KnownPrivateKeyFileName);

            // Act
            var result = signingProvider.SignHash(KnownData.KnownBodyHash);

            // Assert
            result.ShouldBe(KnownData.KnownBodySignature);
        }
    }
}
