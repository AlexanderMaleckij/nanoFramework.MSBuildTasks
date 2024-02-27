using System;
using System.Collections.Generic;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Pipelines;

namespace nanoFramework.MSBuildTasks.UnitTests.Pipelines
{
    [TestClass]
    public class PipelineRunnerTests
    {
        public class TestContext
        {
        }

        #region Constructor Tests

        public static IEnumerable<object[]> ConstructorTestData
        {
            get
            {
                var pipelineSteps = Array.Empty<IPipelineStep<TestContext>>();
                var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<ITask>());

                var nullPipelineSteps = null as IPipelineStep<TestContext>;
                var nullTaskLoggingHelper = null as TaskLoggingHelper;

                return new[]
                {
                    new object[] { nullPipelineSteps, taskLoggingHelper, "pipelineSteps" },
                    new object[] { pipelineSteps, nullTaskLoggingHelper, "taskLoggingHelper" },
                };
            }
        }

        [TestMethod]
        [DynamicData(nameof(ConstructorTestData))]
        public void GivenConstructor_WhenParameterIsNull_ThenShouldThrowArgumentNullException(
            IEnumerable<IPipelineStep<TestContext>> pipelineSteps,
            TaskLoggingHelper taskLoggingHelper,
            string nullParamName)
        {
            // Act
            Action act = () => _ = new PipelineRunner<TestContext>(pipelineSteps, taskLoggingHelper);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName(nullParamName);
        }

        #endregion

        #region Run Tests

        [TestMethod]
        public void GivenRun_WhenStepsDoNotLogErrors_ThenShouldCallAllStepsWithTheSameContext()
        {
            // Arrange
            var step1Mock = new Mock<IPipelineStep<TestContext>>();
            var step2Mock = new Mock<IPipelineStep<TestContext>>();
            var steps = new[] { step1Mock.Object, step2Mock.Object };
            var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<IBuildEngine>(), "TestTask");
            var context = new TestContext();

            var runner = new PipelineRunner<TestContext>(steps, taskLoggingHelper);

            // Act
            _ = runner.Run(context);

            // Assert
            step1Mock.Verify(x => x.Handle(context), Times.Once);
            step2Mock.Verify(x => x.Handle(context), Times.Once);
            step1Mock.VerifyNoOtherCalls();
            step2Mock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GivenRun_WhenStepsDoNotLogErrors_ThenShouldReturnTrue()
        {
            // Arrange
            var steps = new[] { Mock.Of<IPipelineStep<TestContext>>() };
            var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<IBuildEngine>(), "TestTask");
            var context = new TestContext();

            var runner = new PipelineRunner<TestContext>(steps, taskLoggingHelper);

            // Act
            var result = runner.Run(context);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void GivenRun_WhenStepLoggedError_ThenShouldNotCallSubsequentSteps()
        {
            // Arrange
            var step1Mock = new Mock<IPipelineStep<TestContext>>();
            var step2Mock = new Mock<IPipelineStep<TestContext>>();
            var steps = new[] { step1Mock.Object, step2Mock.Object };
            var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<IBuildEngine>(), "TestTask");
            var context = new TestContext();

            step1Mock
                .Setup(x => x.Handle(It.IsAny<TestContext>()))
                .Callback(() => taskLoggingHelper.LogError("error"));

            var runner = new PipelineRunner<TestContext>(steps, taskLoggingHelper);

            // Act
            _ = runner.Run(context);

            // Assert
            step1Mock.Verify(x => x.Handle(It.IsAny<TestContext>()), Times.Once);
            step2Mock.Verify(x => x.Handle(It.IsAny<TestContext>()), Times.Never);
        }

        [TestMethod]
        public void GivenRun_WhenLoggerHasLoggedErrors_ThenShouldReturnFalse()
        {
            // Arrange
            var stepMock = new Mock<IPipelineStep<TestContext>>();
            var steps = new[] { stepMock.Object };
            var taskLoggingHelper = new TaskLoggingHelper(Mock.Of<IBuildEngine>(), "TestTask");
            var context = new TestContext();

            stepMock
                .Setup(x => x.Handle(It.IsAny<TestContext>()))
                .Callback(() => taskLoggingHelper.LogError("error"));

            var runner = new PipelineRunner<TestContext>(steps, taskLoggingHelper);

            // Act
            var result = runner.Run(context);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}
