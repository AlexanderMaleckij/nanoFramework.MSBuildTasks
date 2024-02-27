using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using nanoFramework.MSBuildTasks.Validation;

namespace nanoFramework.MSBuildTasks.UnitTests.Validation
{
    [TestClass]
    public class RegexAttributeTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void GivenIsValid_WhenStringIsNullOrEmpty_ThenReturnsTrue(string value)
        {
            // Arrange
            var regexAttribute = new RegexAttribute();

            // Act
            var isValid = regexAttribute.IsValid(value);

            // Assert
            isValid.Should().BeTrue();
        }

        [TestMethod]
        public void GivenIsValid_WhenRegexIsValid_ThenReturnsTrue()
        {
            // Arrange
            var regexAttribute = new RegexAttribute();
            var validRegex = @"^\d+$";

            // Act
            var isValid = regexAttribute.IsValid(validRegex);

            // Assert
            isValid.Should().BeTrue();
        }

        [TestMethod]
        public void GivenIsValid_WhenRegexIsInvalid_ThenReturnsFalse()
        {
            // Arrange
            var regexAttribute = new RegexAttribute();
            var invalidRegex = @"[a-z";

            // Act
            var isValid = regexAttribute.IsValid(invalidRegex);

            // Assert
            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void GivenIsValid_WhenInputIsNotString_ThenReturnsFalse()
        {
            // Arrange
            var regexAttribute = new RegexAttribute();
            var nonStringInput = 123;

            // Act
            var isValid = regexAttribute.IsValid(nonStringInput);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}
