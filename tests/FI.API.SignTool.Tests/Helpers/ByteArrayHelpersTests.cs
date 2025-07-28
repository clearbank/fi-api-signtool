using FI.API.SignTool.Helpers;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.Helpers;

public class ByteArrayHelpersTests
{
    [Fact]
    public void TranslateByteArray_ValidBytesString_ReturnsByteArray()
    {
        const string text = "10,20,30";

        var bytes = ByteArrayHelper.TranslateByteArray(text);

        bytes.Length.ShouldBe(3);
        bytes[0].ShouldBe((byte)10);
        bytes[1].ShouldBe((byte)20);
        bytes[2].ShouldBe((byte)30);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void TranslateByteArray_NullOrEmptyBytesString_ReturnsEmptyByteArray(string? bytesText)
    {
        var bytes = ByteArrayHelper.TranslateByteArray(bytesText);

        bytes.Length.ShouldBe(0);
    }


    [Fact]
    public void DescribeByteArray_ValidByteArray_ReturnsBytesString()
    {
        var bytes = new[]
        {
            (byte)10,
            (byte)20,
            (byte)30
        };

        var text = ByteArrayHelper.DescribeByteArray(bytes);

        text.ShouldBe("10,20,30");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(new byte[0])]
    public void DescribeByteArray_NullOrEmptyByteArray_ReturnsEmptyString(byte[]? bytes)
    {
        var text = ByteArrayHelper.DescribeByteArray(bytes);

        text.ShouldBe(string.Empty);
    }
}
