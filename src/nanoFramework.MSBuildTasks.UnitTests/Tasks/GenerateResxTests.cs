using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Pipelines;
using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Tasks;

namespace nanoFramework.MSBuildTasks.UnitTests.Tasks
{
    [TestClass]
    public class GenerateResxTests
    {
        [TestMethod]
        public void GivenExecuteTask_WhenCalled_ThenShouldRunPipelineWithExpectedContext()
        {
            // Arrange
            var resXFileName = "Resources.resx";
            var projectDirectory = "test-project-directory";
            var taskItems = new[]
            {
                Mock.Of<ITaskItem>(),
            };

            var task = new GenerateResx
            {
                TaskItems = taskItems,
                ResXFileName = resXFileName,
                ProjectDirectory = projectDirectory
            };

            var pipelineRunnerMock = new Mock<IPipelineRunner<ResXGenerationContext>>();

            pipelineRunnerMock
                .Setup(x => x.Run(It.IsAny<ResXGenerationContext>()))
                .Returns(true);

            var serviceProvider = BuildServiceProvider(pipelineRunnerMock.Object);

            // Act
            _ = task.InvokeExecute(serviceProvider);

            // Assert
            pipelineRunnerMock.Verify(x => x.Run(
                    It.Is<ResXGenerationContext>(ctx =>
                           ctx.TaskInput.ProjectDirectory == projectDirectory
                        && ctx.TaskInput.ResXFileName == resXFileName
                        && ctx.TaskInput.TaskItems == taskItems)
                ), Times.Once);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void GivenExecuteTask_WhenCalled_ThenShouldReturnExpectedResult(bool pipelineRunResult)
        {
            // Arrange
            var taskItems = new[]
            {
                Mock.Of<ITaskItem>()
            };

            var task = new GenerateResx
            {
                TaskItems = taskItems
            };

            var pipelineRunnerMock = new Mock<IPipelineRunner<ResXGenerationContext>>();

            pipelineRunnerMock
                .Setup(x => x.Run(It.IsAny<ResXGenerationContext>()))
                .Returns(pipelineRunResult);

            var serviceProvider = BuildServiceProvider(pipelineRunnerMock.Object);

            // Act
            var result = task.InvokeExecute(serviceProvider);

            // Assert
            result.Should().Be(pipelineRunResult);
        }

        public static ServiceProvider BuildServiceProvider(IPipelineRunner<ResXGenerationContext> pipelineRunner)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(pipelineRunner);

            return serviceCollection.BuildServiceProvider();
        }
    }
}