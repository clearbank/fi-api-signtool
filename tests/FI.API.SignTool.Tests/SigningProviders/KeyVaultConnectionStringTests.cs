using System;
using FI.API.SignTool.SigningProviders;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.SigningProviders;

public class KeyVaultConnectionStringTests
{
    [Fact]
    public void KeyVaultConnectionString_AlgorithmNotProvided_UsesDefaultAlgorithm()
    {
        var connectionString = "Url=myUrl;KeyName=myKey;KeyVersion=abc;";

        var keyVaultConnectionString = new KeyVaultConnectionString(connectionString);

        keyVaultConnectionString.Url.ShouldBe("myUrl");
        keyVaultConnectionString.KeyName.ShouldBe("myKey");
        keyVaultConnectionString.KeyVersion.ShouldBe("abc");
        keyVaultConnectionString.Algorithm.ShouldBe(KeyVaultConnectionString.DefaultAlgorithm);
    }


    [Fact]
    public void HasClientCredentials_ClientCredentialsProvided_ReturnsTrue()
    {
        var connectionString = "Url=myUrl;KeyName=myKey;TenantId=123;ClientId=456;ClientSecret=789";

        var keyVaultConnectionString = new KeyVaultConnectionString(connectionString);

        keyVaultConnectionString.HasClientCredentials.ShouldBeTrue();
        keyVaultConnectionString.TenantId.ShouldBe("123");
        keyVaultConnectionString.ClientId.ShouldBe("456");
        keyVaultConnectionString.ClientSecret.ShouldBe("789");
    }

    [Theory]
    [InlineData("Url=myUrl;KeyName=myKey;KeyVersion=abc")]
    [InlineData("Url=myUrl;KeyName=myKey;KeyVersion=abc;ClientId=456")]
    [InlineData("Url=myUrl;KeyName=myKey;KeyVesrion=abc;TenantId=123;ClientId=456")]
    [InlineData("Url=myUrl;KeyName=myKey;KeyVesrion=abc;ClientId=456;ClientSecret=789")]
    public void HasClientCredentials_ClientCredentialsNotProvided_ReturnsFalse(string connectionString)
    {
        var keyVaultConnectionString = new KeyVaultConnectionString(connectionString);

        keyVaultConnectionString.HasClientCredentials.ShouldBeFalse();
    }

    [Theory]
    [InlineData(null, nameof(KeyVaultConnectionString.Url))]
    [InlineData("", nameof(KeyVaultConnectionString.Url))]
    [InlineData("some random text", nameof(KeyVaultConnectionString.Url))]
    [InlineData("KeyName=myKey", nameof(KeyVaultConnectionString.Url))]
    [InlineData("Url=myurl", nameof(KeyVaultConnectionString.KeyName))]
    [InlineData("Url=myurl;KeyName=myKey;TenantId=123", nameof(KeyVaultConnectionString.ClientId))]
    [InlineData("Url=myurl;KeyName=myKey;TenantId=123;ClientId=456", nameof(KeyVaultConnectionString.ClientSecret))]
    [InlineData("Url=myurl;KeyName=myKey;ClientId=456;ClientSecret=789", nameof(KeyVaultConnectionString.TenantId))]
    [InlineData("Url=myurl;KeyName=myKey;ClientId=456;", nameof(KeyVaultConnectionString.TenantId))]
    [InlineData("Url=myurl;KeyName=myKey;ClientSecret=789", nameof(KeyVaultConnectionString.TenantId))]
    public void Validate_ConnectionStringIsInvalid_ThrowsArgumentException(string? connectionString, string missingArgument)
    {
        var keyVaultConnectionString = new KeyVaultConnectionString(connectionString);

        Should.Throw<ArgumentException>(keyVaultConnectionString.Validate)
            .ShouldSatisfyAllConditions(
                ex => ex.Message.ShouldStartWith($"{nameof(KeyVaultConnectionString)}: Invalid or missing {missingArgument}"),
                ex => ex.ParamName.ShouldBe(missingArgument)
            );
    }
}
