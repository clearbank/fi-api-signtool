using System;
using FI.API.SignTool.Extensions;
using Shouldly;
using Xunit;

namespace FI.API.SignTool.Tests.Extensions
{
    public class EnumerableExtensionsTests
    {
        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("x", true)]
        [InlineData("x,y,z", true)]
        public void HasAny_should_return_correct_result(string items, bool expectedResult)
        {
            // Arrange
            var enumerable = items?.Split(",", StringSplitOptions.RemoveEmptyEntries);

            // Act
            var result = enumerable.HasAny();

            // Assert
            result.ShouldBe(expectedResult);
        }
    }
}
