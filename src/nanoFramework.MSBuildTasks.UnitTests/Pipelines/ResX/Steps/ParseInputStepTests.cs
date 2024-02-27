using System;
using System.Collections.Generic;

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
    public class ParseInputStepTests
    {
        [TestMethod]
        public void GivenConstructor_WhenTaskLoggingHelperIsNull_ThenShouldThrowArgumentNullException()
        {
            // Arrange
            var taskLoggingHelper = null as TaskLoggingHelper;

            // Act
            Action act = () => _ = new ParseInputStep(taskLoggingHelper);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("taskLoggingHelper");
        }

        [TestMethod]
        public void GivenHandle_WhenCalled_ThenShouldPopulateResourcesSourceInputsContextProperty()
        {
            // Arrange
            var context = new ResXGenerationContext
            {
                TaskInput = new GenerateResXTaskInput
                {
                    TaskItems = new[]
                    {
                        new TaskItem("TestDirPath1", new Dictionary<string, string> {{ "RegexFilter", ".*html" }}),
                        new TaskItem("TestDirPath2", new Dictionary<string, string> {{ "SearchPattern", "*.html" }})
                    }
                }
            };

            var expectedResourcesSourceInputs = new[]
            {
                new ResourcesSourceParsedInput
                {
                    DirectoryPath = "TestDirPath1",
                    SearchPattern = string.Empty,
                    RegexFilter = ".*html",
                },
                new ResourcesSourceParsedInput
                {
                    DirectoryPath = "TestDirPath2",
                    SearchPattern = "*.html",
                    RegexFilter = string.Empty,
                }
            };

            var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<IBuildEngine>(), "TestTask");
            var step = new ParseInputStep(taskLoggingHelper);

            // Act
            step.Handle(context);

            // Assert
            context.ResourcesSourceInputs.Should()
                .BeEquivalentTo(expectedResourcesSourceInputs);
        }
    }
}
