using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.UnitTests.Services
{
    [TestClass]
    public class FileSystemServiceTests
    {
        [TestMethod]
        public void GivenConstructor_WhenFileSystemIsNull_ThenShouldThrowArgumentNullException()
        {
            // Arrange
            var fileSystem = null as IFileSystem;

            // Act
            Action act = () => new FileSystemService(fileSystem);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("fileSystem");
        }

        [TestMethod]
        public void GivenGetAbsolutePath_WhenCalledWithAbsolutePath_ThenShouldReturnTheSamePath()
        {
            // Arrange
            var absolutePath = @"C:\Folder\file.txt";
            var basePath = @"F:\Files";

            var mockFileSystem = new MockFileSystem();
            var fileSystemService = new FileSystemService(mockFileSystem);

            // Act
            var resultPath = fileSystemService.GetAbsolutePath(absolutePath, basePath);

            // Assert
            resultPath.Should().Be(absolutePath);
        }

        [TestMethod]
        public void GivenGetAbsolutePath_WhenCalledWithRelativePath_ThenShouldReturnCorrectAbsolutePath()
        {
            // Arrange
            var path = @"Assets\file.txt";
            var basePath = @"F:\Files";
            var expectedPath = @"F:\Files\Assets\file.txt";

            var mockFileSystem = new MockFileSystem();
            var fileSystemService = new FileSystemService(mockFileSystem);

            // Act
            var resultPath = fileSystemService.GetAbsolutePath(path, basePath);

            // Assert
            resultPath.Should().BeEquivalentTo(expectedPath);
        }
    }
}
