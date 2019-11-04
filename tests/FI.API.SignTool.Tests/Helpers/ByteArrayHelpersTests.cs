using FI.API.SignTool.Helpers;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.Helpers
{
    public class ByteArrayHelpersTests
    {
        [Fact]
        public void TranslateByteArray_can_process_text_successfully()
        {
            // Arrange
            const string text = "10,20,30";

            // Act
            var bytes = ByteArrayHelper.TranslateByteArray(text);

            // Assert
            bytes.Length.ShouldBe(3);
            bytes[0].ShouldBe((byte)10);
            bytes[1].ShouldBe((byte)20);
            bytes[2].ShouldBe((byte)30);
        }

        [Fact]
        public void TranslateByteArray_can_handle_empty_string_successfully()
        {
            // Arrange
            const string text = "";

            // Act
            var bytes = ByteArrayHelper.TranslateByteArray(text);

            // Assert
            bytes.Length.ShouldBe(0);
        }

        [Fact]
        public void TranslateByteArray_can_handle_null_string_successfully()
        {
            // Arrange
            const string text = null;

            // Act
            var bytes = ByteArrayHelper.TranslateByteArray(text);

            // Assert
            bytes.Length.ShouldBe(0);
        }

        [Fact]
        public void DescribeByteArray_can_return_text_successfully()
        {
            // Arrange
            var bytes = new []
            {
                (byte)10,
                (byte)20,
                (byte)30
            };

            // Act
            var text = ByteArrayHelper.DescribeByteArray(bytes);

            // Assert
            text.ShouldBe("10,20,30");
        }

        [Fact]
        public void DescribeByteArray_can_handle_empty_byte_array_successfully()
        {
            // Arrange
            var bytes = new byte[] { };

            // Act
            var text = ByteArrayHelper.DescribeByteArray(bytes);

            // Assert
            text.ShouldBe("");
        }

        [Fact]
        public void DescribeByteArray_can_handle_null_string_successfully()
        {
            // Arrange
            byte[] bytes = null;

            // Act
            var text = ByteArrayHelper.DescribeByteArray(bytes);

            // Assert
            text.ShouldBe("");
        }
    }
}
