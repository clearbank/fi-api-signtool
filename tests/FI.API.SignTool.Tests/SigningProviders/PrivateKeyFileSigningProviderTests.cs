using System;
using System.IO;
using FI.API.SignTool.SigningProviders;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.SigningProviders;

public class PrivateKeyFileSigningProviderTests
{
    [Fact]
    public void PrivateKeyFileSigningProvider_ValidPrivateKeyFile_ConstructsProviderSuccessfully()
    {
        var result = new PrivateKeyFileSigningProvider(KnownData.KnownPrivateKeyFileName);

        result.ShouldNotBeNull();
    }

    [Fact]
    public void PrivateKeyFileSigningProvider_InvalidPrivateKeyFile_ThrowsArgumentException()
    {
        var tempFileName = Path.GetTempFileName();
        File.WriteAllText(tempFileName, Guid.NewGuid().ToString());

        Should.Throw<ArgumentException>(() => new PrivateKeyFileSigningProvider(tempFileName))
            .ShouldSatisfyAllConditions(
                ex => ex.Message.ShouldStartWith("Invalid PEM"),
                ex => ex.ParamName.ShouldBe("pem")
            );

        File.Delete(tempFileName);
    }

    [Fact]
    public void PrivateKeyFileSigningProvider_UnknownPrivateKeyFile_ThrowsFileNotFoundException()
    {
        var unknownFileName = Guid.NewGuid().ToString();

        Should.Throw<FileNotFoundException>(() => new PrivateKeyFileSigningProvider(unknownFileName))
            .ShouldSatisfyAllConditions(
                ex => ex.Message.ShouldStartWith("Could not find file"),
                ex => ex.Message.ShouldContain(unknownFileName)
            );
    }

    [Fact]
    public void SignHash_ValidPrivateKeyFile_GeneratesHashSuccessfully()
    {
        var signingProvider = new PrivateKeyFileSigningProvider(KnownData.KnownPrivateKeyFileName);

        var result = signingProvider.SignHash(KnownData.KnownBodyHash);

        result.ShouldBe(KnownData.KnownBodySignature);
    }
}
