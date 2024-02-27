using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Pipelines.ResX.FilePathFilters;

namespace nanoFramework.MSBuildTasks.UnitTests.Pipelines.ResX.FilePathFilters
{
    [TestClass]
    public class SearchPatternFilePathFilterTests
    {
        public static IEnumerable<object[]> ConstructorTestData
        {
            get
            {
                var searchPattern = "*.html";
                var fileSystem = Mock.Of<IFileSystem>();

                var nullSearchPattern = null as string;
                var nullFileSystem = null as FileSystem;

                return new[]
                {
                    new object[] { searchPattern, nullFileSystem, "fileSystem" },
                    new object[] { nullSearchPattern, fileSystem, "searchPattern" },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConstructorTestData))]
        public void GivenConstructor_WhenParameterIsNull_ThenShouldThrowArgumentNullException(
            string searchPattern,
            IFileSystem fileSystem,
            string nullParamName)
        {
            // Act
            Action act = () => _ = new SearchPatternFilePathFilter(searchPattern, fileSystem);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName(nullParamName);
        }

        [TestMethod]
        public void GivenGetMatchingFilePath_WhenCalledWithNullBasePath_ThenShouldThrowArgumentNullException()
        {
            // Arrange
            var basePath = null as string;
            var filter = new SearchPatternFilePathFilter("*.html", Mock.Of<IFileSystem>());

            // Act
            Action act = () => _ = filter.GetMatchingFilePaths(basePath);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("basePath");
        }

        [TestMethod]
        public void GivenGetMatchingFilePaths_WhenCalledWithValidBasePath_ThenShouldReturnExpectedResult()
        {
            // Arrange
            var path = @"C:\Folder";
            var searchPattern = "*.html";

            var expectedFilePaths = new[]
            {
                @"C:\Folder\index.html",
                @"C:\Folder\page1.html",
            };

            var distractingFilePaths = new[]
            {
                @"C:\Folder\ignore.txt",
                @"C:\Stuff\index.html",
                @"C:\Folder\js\script.js",
                @"C:\Folder\css\styles.css",
            };

            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(), path);
            var filter = new SearchPatternFilePathFilter(searchPattern, mockFileSystem);

            foreach (var testFilePath in expectedFilePaths.Concat(distractingFilePaths))
            {
                mockFileSystem.AddEmptyFile(testFilePath);
            }

            // Act
            var actualFilePaths = filter.GetMatchingFilePaths(path);

            // Assert
            actualFilePaths.Should().BeEquivalentTo(expectedFilePaths);
        }
    }
}
