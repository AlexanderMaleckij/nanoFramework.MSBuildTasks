using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Models;
using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.UnitTests.Services
{
    [TestClass]
    public class ResourcesSourceProcessorTests
    {
        public static IEnumerable<object[]> ConstructorTestData
        {
            get
            {
                yield return new object[]
                {
                    null,
                    Mock.Of<ResourcesSourceProcessorOptions>(),
                    "fileSystemService"
                };

                yield return new object[]
                {
                    Mock.Of<IFileSystemService>(),
                    null,
                    "processorOptions"
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConstructorTestData))]
        public void GivenConstructor_WhenParameterIsNull_ThenShouldThrowArgumentNullException(
            IFileSystemService fileSystemService,
            ResourcesSourceProcessorOptions options,
            string nullParameterName)
        {
            // Act
            Action act = () => new ResourcesSourceProcessor(fileSystemService, options);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName(nullParameterName);
        }

        [TestMethod]
        public void GivenProcess_WhenCalledWithNullSource_ThenShouldThrowArgumentNullException()
        {
            // Arrange
            var nullSource = null as ResourcesSource;
            var fileSystemService = Mock.Of<IFileSystemService>();
            var options = Mock.Of<ResourcesSourceProcessorOptions>();
            var processor = new ResourcesSourceProcessor(fileSystemService, options);

            // Act
            Action act = () => processor.Process(nullSource);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("resourcesLocation");
        }

        [TestMethod]
        public void GivenProcess_WhenCalled_ThenShouldGetAbsolutePathToResourcesSource()
        {
            // Arrange
            var fileSystemServiceMock = new Mock<IFileSystemService>();

            var source = new ResourcesSource
            {
                FolderPath = "Resources"
            };

            var options = new ResourcesSourceProcessorOptions
            {
                 NanoResXWriter = Mock.Of<INanoResXResourceWriter>(),
                 ProjectDirectory = @"C:\Projects\Project",
            };

            var processor = new ResourcesSourceProcessor(fileSystemServiceMock.Object, options);

            // Act
            processor.Process(source);

            // Assert
            fileSystemServiceMock.Verify(x => x.GetAbsolutePath(source.FolderPath, options.ProjectDirectory), Times.Once);
        }

        [TestMethod]
        public void GivenProcess_WhenGotAbsolutePathToFolderWithResources_ThenShouldGetResourceFilesPaths()
        {
            // Arrange
            var absolutePathToResourcesFolder = @"C:\Example\Resources";
            var fileSystemServiceMock = new Mock<IFileSystemService>();

            fileSystemServiceMock
                .Setup(x => x.GetAbsolutePath(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(absolutePathToResourcesFolder);

            var source = new ResourcesSource
            {
                RegexFilter = ".*"
            };

            var options = new ResourcesSourceProcessorOptions
            {
                NanoResXWriter = Mock.Of<INanoResXResourceWriter>(),
            };

            var processor = new ResourcesSourceProcessor(fileSystemServiceMock.Object, options);

            // Act
            processor.Process(source);

            // Assert
            fileSystemServiceMock.Verify(x => x.GetDirectoryFiles(absolutePathToResourcesFolder, source.RegexFilter), Times.Once);
        }

        [TestMethod]
        public void GivenProcess_WhenGotResourceFilesPaths_ThenShouldCallResourceWriterToAddThem()
        {
            // Arrange
            var source = new ResourcesSource
            {
                FolderPath = @"C:\Resources"
            };

            var resourcesFullFilesPaths = new[]
            {
                $@"{source.FolderPath}\index.html",
                $@"{source.FolderPath}\js\script.js",
            };

            var fileSystemServiceMock = new Mock<IFileSystemService>();
            var nanoResXWriterMock = new Mock<INanoResXResourceWriter>();

            fileSystemServiceMock
                .Setup(x => x.GetAbsolutePath(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(source.FolderPath);

            fileSystemServiceMock
                .Setup(x => x.GetDirectoryFiles(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(resourcesFullFilesPaths);

            var options = new ResourcesSourceProcessorOptions
            {
                NanoResXWriter = nanoResXWriterMock.Object,
            };

            var processor = new ResourcesSourceProcessor(fileSystemServiceMock.Object, options);

            // Act
            processor.Process(source);

            // Assert
            nanoResXWriterMock.Verify(x => x.Add("index.html", @"C:\Resources\index.html"), Times.Once);
            nanoResXWriterMock.Verify(x => x.Add("js/script.js", @"C:\Resources\js\script.js"), Times.Once);
            nanoResXWriterMock.VerifyNoOtherCalls();
        }
    }
}
