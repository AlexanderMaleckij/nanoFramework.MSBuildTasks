using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Pipelines.ResX.Steps;

namespace nanoFramework.MSBuildTasks.UnitTests.Pipelines.ResX.Steps
{
    [TestClass]
    public class GenerateResXFileStepTests
    {
        #region Constructor Tests

        public static IEnumerable<object[]> ConstructorTestData
        {
            get
            {
                var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<ITask>());
                var fileSystem = Mock.Of<IFileSystem>();

                var nullTaskLoggingHelper = null as TaskLoggingHelper;
                var nullFileSystem = null as FileSystem;

                return new[]
                {
                    new object[] { taskLoggingHelper, nullFileSystem, "fileSystem" },
                    new object[] { nullTaskLoggingHelper, fileSystem, "taskLoggingHelper" },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConstructorTestData))]
        public void GivenConstructor_WhenParameterIsNull_ThenShouldThrowArgumentNullException(
            TaskLoggingHelper taskLoggingHelper,
            IFileSystem fileSystem,
            string nullParamName)
        {
            // Act
            Action act = () => _ = new GenerateResXFileStep(taskLoggingHelper, fileSystem);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName(nullParamName);
        }

        #endregion

        [TestMethod]
        public void GivenHandle_WhenCalled_ThenShouldCreateSingleFileAtExpectedLocation()
        {
            // Arrange
            var context = new ResXGenerationContext
            {
                TaskInput = new GenerateResXTaskInput
                {
                    ResXFileName = "Resources.resx"
                },
                FileResourceInfos = Array.Empty<FileResourceInfo>()
            };

            var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<IBuildEngine>(), "TestTask");
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(), @"C:\Folder");

            var step = new GenerateResXFileStep(taskLoggingHelper, mockFileSystem);

            // Act
            step.Handle(context);

            // Assert
            mockFileSystem.AllFiles.Should().HaveCount(1);
            mockFileSystem.AllFiles.First().Should().Be(@"C:\Folder\Resources.resx");
        }

        [TestMethod]
        public void GivenHandle_WhenCalled_ThenShouldCreateFileWithExpectedContent()
        {
            var context = new ResXGenerationContext
            {
                TaskInput = new GenerateResXTaskInput
                {
                    ResXFileName = "Resources.resx"
                },
                FileResourceInfos = new[]
                {
                    new FileResourceInfo { Name = "index.html", Path = @"C:\Assets\index.html" },
                    new FileResourceInfo { Name = "js/script.js", Path = @"F:\Folder\script.js" },
                    new FileResourceInfo { Name = "some/resource/name", Path = @"C:\file.txt" },
                }
            };

            var expectedContent = File.ReadAllText(@"Pipelines\ResX\Steps\Resources.resx.txt");

            var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<IBuildEngine>(), "TestTask");
            var mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>(), @"C:\Folder");

            var step = new GenerateResXFileStep(taskLoggingHelper, mockFileSystem);

            // Act
            step.Handle(context);

            // Assert
            mockFileSystem.File.ReadAllText(mockFileSystem.AllFiles.First()).Should().Be(expectedContent);
        }
    }
}
