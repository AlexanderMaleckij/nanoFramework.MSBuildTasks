using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq.Expressions;

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
    public class ValidateInputStepTests
    {
        public static IEnumerable<object[]> ConstructorTestData
        {
            get
            {
                var fileSystem = Mock.Of<IFileSystem>();
                var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<ITask>());

                var nullFileSystem = null as FileSystem;
                var nullTaskLoggingHelper = null as TaskLoggingHelper;

                return new[]
                {
                    new object[] { nullFileSystem, taskLoggingHelper, "fileSystem" },
                    new object[] { fileSystem, nullTaskLoggingHelper, "taskLoggingHelper" },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConstructorTestData))]
        public void GivenConstructor_WhenParameterIsNull_ThenShouldThrowArgumentNullException(
            IFileSystem fileSystem,
            TaskLoggingHelper taskLoggingHelper,
            string nullParamName)
        {
            // Act
            Action act = () => _ = new ValidateInputStep(fileSystem, taskLoggingHelper);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName(nullParamName);
        }

        [TestMethod]
        public void GivenHandle_WhenAllResourcesSourceInputsAreValid_ThenShouldNotLogAnyErrors()
        {
            // Arrange
            var context = new ResXGenerationContext
            {
                ResourcesSourceInputs = new ResourcesSourceParsedInput[]
                {
                    new ResourcesSourceParsedInput { DirectoryPath = "/Resources1" },
                    new ResourcesSourceParsedInput { DirectoryPath = "/Resources2" }
                }
            };

            var directoryMock = new Mock<IDirectory>();

            directoryMock
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            var fileSystemMock = new Mock<IFileSystem>();

            fileSystemMock
                .SetupGet(x => x.Directory)
                .Returns(directoryMock.Object);

            var buildEngineMock = new Mock<IBuildEngine>();
            var taskLoggingHelper = new TaskLoggingHelper(buildEngineMock.Object, "TestTask");
            var step = new ValidateInputStep(fileSystemMock.Object, taskLoggingHelper);

            // Act
            Action act = () => step.Handle(context);

            // Assert
            buildEngineMock.Verify(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>()), Times.Never);
        }

        [TestMethod]
        public void GivenHandle_WhenDirectoryDoesNotExist_ThenShouldLogError()
        {
            // Arrange
            var context = new ResXGenerationContext
            {
                TaskInput = new GenerateResXTaskInput
                {
                    ProjectFullPath = "nfproj-full-path"
                },
                ResourcesSourceInputs = new ResourcesSourceParsedInput[]
                {
                    new ResourcesSourceParsedInput { DirectoryPath = "/Resources1" },
                    new ResourcesSourceParsedInput { DirectoryPath = "/Resources2" }
                }
            };

            var directoryMock = new Mock<IDirectory>();

            directoryMock.Setup(x => x.Exists("/Resources1")).Returns(true);
            directoryMock.Setup(x => x.Exists("/Resources2")).Returns(false);

            var fileSystemMock = new Mock<IFileSystem>();

            fileSystemMock
                .SetupGet(x => x.Directory)
                .Returns(directoryMock.Object);

            var buildEngineMock = new Mock<IBuildEngine>();
            var taskLoggingHelper = new TaskLoggingHelper(buildEngineMock.Object, "TestTask");
            var step = new ValidateInputStep(fileSystemMock.Object, taskLoggingHelper);

            Expression<Func<BuildErrorEventArgs, bool>> matcher = e =>
                   e.Subcategory == "validation"
                && e.Code == "RG0001"
                && e.HelpKeyword == null
                && e.File == "nfproj-full-path"
                && e.LineNumber == 0
                && e.ColumnNumber == 0
                && e.EndLineNumber == 0
                && e.EndColumnNumber == 0
                && e.Message == "Directory specified in the ResourcesSource node does not exist. Directory: /Resources2";

            // Act
            step.Handle(context);

            // Assert
            buildEngineMock.Verify(x => x.LogErrorEvent(It.Is(matcher)), Times.Once);
        }

        [TestMethod]
        public void GivenHandle_WhenAtLeastOneResourcesSourceInputIsInvalid_ThenShouldLogError()
        {
            // Arrange
            var context = new ResXGenerationContext
            {
                TaskInput = new GenerateResXTaskInput
                {
                    ProjectFullPath = "nfproj-full-path"
                },
                ResourcesSourceInputs = new ResourcesSourceParsedInput[]
                {
                    new ResourcesSourceParsedInput { DirectoryPath = "" },
                    new ResourcesSourceParsedInput { DirectoryPath = "/Resources" }
                }
            };

            var directoryMock = new Mock<IDirectory>();

            directoryMock
                .Setup(x => x.Exists(It.IsAny<string>()))
                .Returns(true);

            var fileSystemMock = new Mock<IFileSystem>();

            fileSystemMock
                .SetupGet(x => x.Directory)
                .Returns(directoryMock.Object);

            var buildEngineMock = new Mock<IBuildEngine>();
            var taskLoggingHelper = new TaskLoggingHelper(buildEngineMock.Object, "TestTask");
            var step = new ValidateInputStep(fileSystemMock.Object, taskLoggingHelper);

            Expression<Func<BuildErrorEventArgs, bool>> matcher = e =>
                   e.Subcategory == "validation"
                && e.Code == "RG0001"
                && e.HelpKeyword == null
                && e.File == "nfproj-full-path"
                && e.LineNumber == 0
                && e.ColumnNumber == 0
                && e.EndLineNumber == 0
                && e.EndColumnNumber == 0;

            // Act
            step.Handle(context);

            // Assert
            buildEngineMock.Verify(x => x.LogErrorEvent(It.Is(matcher)), Times.Once);
        }
    }
}
