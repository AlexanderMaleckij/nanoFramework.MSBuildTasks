using System;
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
        public void GivenConstructor_WhenLoggerIsNull_ThenShouldThrowArgumentNullException()
        {
            // Arrange
            var taskLoggingHelper = null as TaskLoggingHelper;

            // Act
            Action act = () => _ = new ValidateInputStep(taskLoggingHelper);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("taskLoggingHelper");
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

            var buildEngineMock = new Mock<IBuildEngine>();
            var taskLoggingHelper = new TaskLoggingHelper(buildEngineMock.Object, "TestTask");
            var step = new ValidateInputStep(taskLoggingHelper);

            // Act
            Action act = () => step.Handle(context);

            // Assert
            buildEngineMock.Verify(x => x.LogErrorEvent(It.IsAny<BuildErrorEventArgs>()), Times.Never);
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

            var buildEngineMock = new Mock<IBuildEngine>();
            var taskLoggingHelper = new TaskLoggingHelper(buildEngineMock.Object, "TestTask");
            var step = new ValidateInputStep(taskLoggingHelper);

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
