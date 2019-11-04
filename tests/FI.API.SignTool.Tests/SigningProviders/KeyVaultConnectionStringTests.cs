using System;
using FI.API.SignTool.SigningProviders;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.SigningProviders
{
    public class KeyVaultConnectionStringTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("some random text")]
        public void KeyVaultConnectionString_can_construct_with_any_string_but_fails_validation(string connectionString)
        {
            // Act
            var result = new KeyVaultConnectionString(connectionString);
            var ex = Assert.Throws<ArgumentException>(
                () => result.Validate()
            );

            // Assert
            result.ShouldNotBeNull();
            ex.ShouldNotBeNull();
        }

        [Theory]
        [InlineData("URL=myurl;cliENTid=12345;clientSECRET=67890;keyname=myKey;KEYVERsion=1;AlGoRiThM=RS512", true, "myurl", "12345", "67890", "myKey", "1", "RS512")]
        [InlineData("UrL=myurl;", false, "myurl", null, null, null, KeyVaultConnectionString.DefaultKeyVersion, KeyVaultConnectionString.DefaultAlgorithm)]
        [InlineData("clientid=12345;URL=myurl", false, "myurl", "12345", null, null, KeyVaultConnectionString.DefaultKeyVersion, KeyVaultConnectionString.DefaultAlgorithm)]
        [InlineData("clientid=12345;URL=myurl;clientsecret=ABCDE", false, "myurl", "12345", "ABCDE", null, KeyVaultConnectionString.DefaultKeyVersion, KeyVaultConnectionString.DefaultAlgorithm)]
        [InlineData("clientid=12345;keyname=qqq;URL=myurl;clientsecret=ABCDE", true, "myurl", "12345", "ABCDE", "qqq", KeyVaultConnectionString.DefaultKeyVersion, KeyVaultConnectionString.DefaultAlgorithm)]
        [InlineData("keyversion=57;clientid=12345;keyname=qqq;URL=myurl;clientsecret=ABCDE", true, "myurl", "12345", "ABCDE", "qqq", "57", KeyVaultConnectionString.DefaultAlgorithm)]
        [InlineData("keyversion=57;clientid=12345;algorithm=EC256;keyname=qqq;URL=myurl;clientsecret=ABCDE", true, "myurl", "12345", "ABCDE", "qqq", "57", "EC256")]
        [InlineData("keyversion=57;algorithm=EC256;keyname=qqq;URL=myurl;clientsecret=ABCDE", false, "myurl", null, "ABCDE", "qqq", "57", "EC256")]
        [InlineData("keyversion=57;clientid=12345;algorithm=EC256;keyname=qqq;URL=myurl", false, "myurl", "12345", null, "qqq", "57", "EC256")]
        public void KeyVaultConnectionString_can_construct_with_any_string_successfully(string connectionString, bool validates, string url, string clientId, string clientSecret, string keyName, string keyVersion, string algorithm)
        {
            // Act
            var result = new KeyVaultConnectionString(connectionString);

            var success = true;
            try
            {
                result.Validate();
            }
            catch (Exception e)
            {
                success = false;
            }

            // Assert
            result.ShouldNotBeNull();
            success.ShouldBe(validates);
            result.Url.ShouldBe(url);
            result.ClientId.ShouldBe(clientId);
            result.ClientSecret.ShouldBe(clientSecret);
            result.KeyName.ShouldBe(keyName);
            result.KeyVersion.ShouldBe(keyVersion);
            result.Algorithm.ShouldBe(algorithm);

            result.HasKeyName.ShouldBe(!string.IsNullOrWhiteSpace(keyName));
            result.HasKeyVersion.ShouldBe(!string.IsNullOrWhiteSpace(keyVersion));
        }
    }
}
