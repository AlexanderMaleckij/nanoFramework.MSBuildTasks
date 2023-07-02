using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Factories;
using nanoFramework.MSBuildTasks.Models;
using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.UnitTests.Factories
{
    [TestClass]
    public class ResourcesLocationProcessorFactoryTests
    {
        [TestMethod]
        public void GivenConstructor_WhenFileSystemServiceIsNull_ThenShouldThrowArgumentNullException()
        {
            // Arrange
            var fileSystemService = null as IFileSystemService;

            // Act
            Action act = () => new ResourcesLocationProcessorFactory(fileSystemService);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("fileSystemService");
        }

        [TestMethod]
        public void GivenCreate_WhenCalled_ThenShouldReturnNotNullInstanceOfResourcesSourceProcessor()
        {
            // Arrange
            var processorOptions = Mock.Of<ResourcesSourceProcessorOptions>();
            var fileSystemService = Mock.Of<IFileSystemService>();
            var factory = new ResourcesLocationProcessorFactory(fileSystemService);

            // Act
            var processor = factory.Create(processorOptions);

            // Assert
            processor.Should().NotBeNull().And.BeOfType<ResourcesSourceProcessor>();
        }
    }
}
