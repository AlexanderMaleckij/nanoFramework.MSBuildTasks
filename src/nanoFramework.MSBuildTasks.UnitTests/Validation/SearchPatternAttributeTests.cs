using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using nanoFramework.MSBuildTasks.Validation;

namespace nanoFramework.MSBuildTasks.UnitTests.Validation
{
    [TestClass]
    public class SearchPatternAttributeTests
    {
        [TestMethod]
        [DataRow(null)]
        [DataRow("")]
        public void GivenIsValid_WhenValueIsNull_ThenReturnTrue(string value)
        {
            // Arrange
            var searchPatternAttribute = new SearchPatternAttribute();

            // Act
            var isValid = searchPatternAttribute.IsValid(value);

            // Assert
            isValid.Should().BeTrue();
        }

        [TestMethod]
        public void GivenIsValid_WhenValueIsNotString_ThenReturnFalse()
        {
            // Arrange
            var value = 123;
            var searchPatternAttribute = new SearchPatternAttribute();

            // Act
            var isValid = searchPatternAttribute.IsValid(value);

            // Assert
            isValid.Should().BeFalse();
        }

        [TestMethod]
        public void GivenIsValid_WhenSearchPatternIsValid_ThenReturnTrue()
        {
            // Arrange
            var value = "*.*";
            var searchPatternAttribute = new SearchPatternAttribute();

            // Act
            var isValid = searchPatternAttribute.IsValid(value);

            // Assert
            isValid.Should().BeTrue();
        }

        [TestMethod]
        public void GivenIsValid_WhenSearchPatternIsInvalid_ThenReturnFalse()
        {
            // Arrange
            var value = "..\\";
            var searchPatternAttribute = new SearchPatternAttribute();

            // Act
            var isValid = searchPatternAttribute.IsValid(value);

            // Assert
            isValid.Should().BeFalse();
        }
    }
}
