using System;
using FI.API.SignTool.Parameters.Interfaces;
using FI.API.SignTool.SigningProviders;
using FI.API.SignTool.Types;
using Moq;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.SigningProviders;

public class SigningProviderFactoryTests
{
    [Fact]
    public void GetSigningProvider_FileNameSigningProviderType_ReturnsPrivateKeyFileSigningProvider()
    {
        var argumentsMock = new Mock<ISignArguments>();
        argumentsMock.Setup(x => x.PrivateKeyFileName).Returns(KnownData.KnownPrivateKeyFileName);

        var result = SigningProviderFactory.GetSigningProvider(argumentsMock.Object);

        result.ShouldNotBeNull();
    }

    [Fact]
    public void GetSigningProvider_AzureKeyVaultSigningProviderType_ReturnsAzureKeyVaultSigningProvider()
    {
        var argumentsMock = new Mock<ISignArguments>();
        argumentsMock.Setup(x => x.SigningProvider).Returns(SigningProviderType.AzureKeyVault);
        argumentsMock.Setup(x => x.AzureKeyVaultConnectionString).Returns("Url=myUrl;KeyName=myKey;");

        var result = SigningProviderFactory.GetSigningProvider(argumentsMock.Object);

        result.ShouldNotBeNull();
    }

    [Fact]
    public void GetSigningProvider_UnknownSigningProviderType_ThrowsArgumentOutOfRangeException()
    {
        var unknownSigningProvider = (SigningProviderType)int.MaxValue;

        var argumentsMock = new Mock<ISignArguments>();
        argumentsMock.Setup(x => x.SigningProvider).Returns(unknownSigningProvider);

        Should.Throw<ArgumentOutOfRangeException>(() => SigningProviderFactory.GetSigningProvider(argumentsMock.Object))
            .ShouldSatisfyAllConditions(
                ex => ex.Message.ShouldStartWith($"Unable to locate ISigningProvider for {unknownSigningProvider}"),
                ex => ex.ParamName.ShouldBe("arguments")
            );
    }
}
