using Dictum.Data.Extensions;
using Xunit;

namespace Dictum.Tests
{
    public class StringExtensions
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("FirstName", "first_name")]
        [InlineData("firstName", "first_name")]
        [InlineData("firstname", "firstname")]
        [InlineData("Firstname", "firstname")]
        public void It_should_convert_string_to_snake_case(string input, string output)
        {
            Assert.Equal(output, input.ToSnakeCase());
        }
    }
}