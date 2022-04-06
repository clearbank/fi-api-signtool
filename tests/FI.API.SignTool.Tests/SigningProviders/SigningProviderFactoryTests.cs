using System;
using System.IO;
using FI.API.SignTool.Parameters.Interfaces;
using FI.API.SignTool.SigningProviders;
using FI.API.SignTool.Types;
using Moq;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.SigningProviders
{
    public class SigningProviderFactoryTests
    {
        [Fact]
        public void GetSigningProvider_for_unknown_value_fails()
        {
            // Arrange
            var argumentsMock = new Mock<ISignArguments>();
            argumentsMock
                .Setup(x => x.SigningProvider)
                .Returns((SigningProviderType)int.MaxValue);

            // Act
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => SigningProviderFactory.GetSigningProvider(argumentsMock.Object)
            );

            // Assert
            ex.ShouldNotBeNull();
            ex.ParamName.ShouldBe("arguments");
        }

        [Fact]
        public void GetSigningProvider_can_create_a_PrivateKeyFileSigningProvider_successfully()
        {
            // Arrange
            var argumentsMock = new Mock<ISignArguments>();
            argumentsMock
                .Setup(x => x.PrivateKeyFileName)
                .Returns(KnownData.KnownPrivateKeyFileName);

            // Act
            var result = SigningProviderFactory.GetSigningProvider(argumentsMock.Object);

            // Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public void GetSigningProvider_fails_to_create_a_PrivateKeyFileSigningProvider_with_unknown_key_file()
        {
            // Arrange
            var argumentsMock = new Mock<ISignArguments>();
            argumentsMock
                .Setup(x => x.PrivateKeyFileName)
                .Returns(Guid.NewGuid().ToString);

            // Act
            var ex = Assert.Throws<FileNotFoundException>(
                () => SigningProviderFactory.GetSigningProvider(argumentsMock.Object)
            );

            // Assert
            ex.ShouldNotBeNull();
        }

        [Fact]
        public void GetSigningProvider_fails_to_create_a_PrivateKeyFileSigningProvider_with_invalid_key_file()
        {
            // Arrange
            var tempFileName = Path.GetTempFileName();
            var argumentsMock = new Mock<ISignArguments>();
            argumentsMock
                .Setup(x => x.PrivateKeyFileName)
                .Returns(tempFileName);

            // Act
            var ex = Assert.Throws<ArgumentException>(
                () => SigningProviderFactory.GetSigningProvider(argumentsMock.Object)
            );

            // Assert
            ex.ShouldNotBeNull();
            ex.ParamName.ShouldBe("pem");

            File.Delete(tempFileName);
        }
    }
}
