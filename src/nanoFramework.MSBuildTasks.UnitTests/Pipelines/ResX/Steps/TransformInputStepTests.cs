using System;
using System.Collections.Generic;
using System.IO.Abstractions;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Pipelines.ResX.FilePathFilters;
using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Pipelines.ResX.Steps;

namespace nanoFramework.MSBuildTasks.UnitTests.Pipelines.ResX.Steps
{
    [TestClass]
    public class TransformInputStepTests
    {
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
            Action act = () => _ = new TransformInputStep(taskLoggingHelper, fileSystem);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName(nullParamName);
        }

        [TestMethod]
        public void GivenHandle_WhenContextHasValidResourcesSourceInputs_ThenShouldPopulateContextWithResourcesSources()
        {
            // Arrange
            var context = new ResXGenerationContext
            {
                ResourcesSourceInputs = new[]
                {
                    new ResourcesSourceParsedInput
                    {
                        DirectoryPath = "DirectoryPath1",
                        RegexFilter = ".*html",
                        SearchPattern = ""
                    },
                    new ResourcesSourceParsedInput
                    {
                        DirectoryPath = "DirectoryPath2",
                        RegexFilter = "",
                        SearchPattern = "*.html"
                    },
                    new ResourcesSourceParsedInput
                    {
                        DirectoryPath = "DirectoryPath3"
                    }
                }
            };

            var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<IBuildEngine>(), "TestTask");
            var step = new TransformInputStep(taskLoggingHelper, Mock.Of<IFileSystem>());

            // Act
            step.Handle(context);

            // Assert
            context.ResourcesSources.Should().HaveCount(3);

            context.ResourcesSources[0].DirectoryPath.Should().Be("DirectoryPath1");
            context.ResourcesSources[0].PathFilter.Should().BeOfType<RegexFilePathFilter>()
                .Which.Pattern.Should().Be(".*html");

            context.ResourcesSources[1].DirectoryPath.Should().Be("DirectoryPath2");
            context.ResourcesSources[1].PathFilter.Should().BeOfType<SearchPatternFilePathFilter>()
                .Which.SearchPattern.Should().Be("*.html");

            context.ResourcesSources[2].DirectoryPath.Should().Be("DirectoryPath3");
            context.ResourcesSources[2].PathFilter.Should().BeOfType<SearchPatternFilePathFilter>()
                .Which.SearchPattern.Should().Be("*");
        }
    }
}
