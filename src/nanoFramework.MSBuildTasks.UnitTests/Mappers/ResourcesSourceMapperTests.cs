using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Mappers;
using nanoFramework.MSBuildTasks.Models;

namespace nanoFramework.MSBuildTasks.UnitTests.Mappers
{
    [TestClass]
    public class ResourcesSourceMapperTests
    {
        [TestMethod]
        public void GivenMap_WhenMapToResourcesSource_ThenShouldReturnExpectedResult()
        {
            // Arrange
            var folderPath = "Resources";
            var regexFilter = ".+html";

            var taskItemMock = new Mock<ITaskItem>();

            taskItemMock
                .Setup(x => x.ItemSpec)
                .Returns(folderPath);

            taskItemMock
                .Setup(x => x.GetMetadata("RegexFilter"))
                .Returns(regexFilter);

            var expectedResult = new ResourcesSource
            {
                FolderPath = folderPath,
                RegexFilter = regexFilter
            };

            var mapper = new ResourcesSourceMapper();

            // Act
            var result = mapper.Map(taskItemMock.Object);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
