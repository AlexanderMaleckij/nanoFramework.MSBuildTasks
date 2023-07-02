using System;
using System.Linq;

using FluentAssertions;

using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using nanoFramework.MSBuildTasks.Factories;
using nanoFramework.MSBuildTasks.Mappers;
using nanoFramework.MSBuildTasks.Models;
using nanoFramework.MSBuildTasks.Services;
using nanoFramework.MSBuildTasks.Tasks;

namespace nanoFramework.MSBuildTasks.UnitTests.Tasks
{
    [TestClass]
    public class GenerateResxTests
    {
        [TestMethod]
        public void GivenExecuteTask_WhenCalled_ThenShouldMapTaskItemsToResourcesSources()
        {
            // Arrange
            var taskItems = new[]
            {
                Mock.Of<ITaskItem>(),
                Mock.Of<ITaskItem>()
            };

            var task = new GenerateResx
            {
                TaskItems = taskItems 
            };

            var mapperMock = new Mock<ITaskItemMapper<ResourcesSource>>();

            mapperMock
                .Setup(x => x.Map(It.IsAny<ITaskItem>()))
                .Returns(new ResourcesSource());

            var serviceProvider = BuildServiceProvider(mapperMock.Object);

            // Act
            _ = task.InvokeExecute(serviceProvider);

            // Assert
            mapperMock.Verify(x => x.Map(taskItems[0]), Times.Once);
            mapperMock.Verify(x => x.Map(taskItems[1]), Times.Once);
            mapperMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GivenExecuteTask_WhenCalled_ThenShouldCreateNanoResXResourceWriter()
        {
            // Arrange
            var writerFactoryMock = new Mock<INanoResXWriterFactory>();

            writerFactoryMock
                .Setup(x => x.Create(It.IsAny<string>()))
                .Returns(Mock.Of<INanoResXWriter>());

            var serviceProvider = BuildServiceProvider(writerFactory: writerFactoryMock.Object);

            var task = new GenerateResx
            {
                TaskItems = Array.Empty<ITaskItem>(),
                ResxFileName = "Resources.resx"
            };

            // Act
            _ = task.InvokeExecute(serviceProvider);

            // Assert
            writerFactoryMock.Verify(x => x.Create(task.ResxFileName), Times.Once);
        }

        [TestMethod]
        public void GivenExecuteTask_WhenCalled_ThenShouldCreateNanoResXResourcesSourceProcessor()
        {
            // Arrange
            var writer = Mock.Of<INanoResXWriter>();
            var writerFactoryMock = new Mock<INanoResXWriterFactory>();
            var processorFactoryMock = new Mock<IResourcesLocationProcessorFactory>();

            writerFactoryMock
                .Setup(x => x.Create(It.IsAny<string>()))
                .Returns(writer);

            processorFactoryMock
                .Setup(x => x.Create(It.IsAny<ResourcesSourceProcessorOptions>()))
                .Returns(Mock.Of<IResourcesSourceProcessor>());

            var serviceProvider = BuildServiceProvider(
                writerFactory: writerFactoryMock.Object,
                processorFactory: processorFactoryMock.Object);

            var task = new GenerateResx
            {
                ProjectDirectory = @"C:\Project",
                TaskItems = Array.Empty<ITaskItem>(),
            };

            // Act
            _ = task.InvokeExecute(serviceProvider);

            // Assert
            processorFactoryMock.Verify(x => x.Create(
                    It.Is<ResourcesSourceProcessorOptions>(o =>
                           o.ProjectDirectory == task.ProjectDirectory
                        && o.NanoResXWriter == writer)),
                    Times.Once);
        }

        [TestMethod]
        public void GivenExecuteTask_WhenCalled_ThenShouldProcessAllSourcesAndCallGenerate()
        {
            // Arrange
            var writerMock = new Mock<INanoResXWriter>();
            var processorMock = new Mock<IResourcesSourceProcessor>();
            var mapperMock = new Mock<ITaskItemMapper<ResourcesSource>>();
            var writerFactoryMock = new Mock<INanoResXWriterFactory>();
            var processorFactoryMock = new Mock<IResourcesLocationProcessorFactory>();

            var taskItems = new[]
            {
                Mock.Of<ITaskItem>(),
                Mock.Of<ITaskItem>()
            };

            var mappedResourcesSources = new[]
            {
                new ResourcesSource(),
                new ResourcesSource()
            };

            mapperMock
                .Setup(x => x.Map(It.IsAny<ITaskItem>()))
                .Returns((ITaskItem t) => mappedResourcesSources.ElementAt(Array.IndexOf(taskItems, t)));

            writerFactoryMock
                .Setup(x => x.Create(It.IsAny<string>()))
                .Returns(writerMock.Object);

            processorFactoryMock
                .Setup(x => x.Create(It.IsAny<ResourcesSourceProcessorOptions>()))
                .Returns(processorMock.Object);

            var serviceProvider = BuildServiceProvider(
                mapper: mapperMock.Object,
                writerFactory: writerFactoryMock.Object,
                processorFactory: processorFactoryMock.Object);

            var task = new GenerateResx
            {
                TaskItems = taskItems,
            };

            // Act
            _ = task.InvokeExecute(serviceProvider);

            // Assert
            writerMock.Verify(x => x.Generate(), Times.Once);
            processorMock.Verify(x => x.Process(mappedResourcesSources[0]), Times.Once);
            processorMock.Verify(x => x.Process(mappedResourcesSources[1]), Times.Once);
            processorMock.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void GivenExecuteTask_WhenCalledAndFinishedWithoutErrors_ThenShouldReturnTrue()
        {
            // Arrange
            var serviceProvider = BuildServiceProvider();

            var task = new GenerateResx()
            {
                TaskItems = Array.Empty<ITaskItem>()
            };

            // Act
            var result = task.InvokeExecute(serviceProvider);

            // Assert
            result.Should().BeTrue();
        }

        public static ServiceProvider BuildServiceProvider(
            ITaskItemMapper<ResourcesSource> mapper = null,
            INanoResXWriterFactory writerFactory = null,
            IResourcesLocationProcessorFactory processorFactory = null,
            IFileSystemService fileSystemService = null)
        {
            var serviceCollection = new ServiceCollection();

            var mapperMock = new Mock<ITaskItemMapper<ResourcesSource>>();
            var writerFactoryMock = new Mock<INanoResXWriterFactory>();
            var processorFactoryMock = new Mock<IResourcesLocationProcessorFactory>();

            mapperMock
                .Setup(x => x.Map(It.IsAny<ITaskItem>()))
                .Returns(new ResourcesSource());

            writerFactoryMock
                .Setup(x => x.Create(It.IsAny<string>()))
                .Returns(Mock.Of<INanoResXWriter>());

            processorFactoryMock
                .Setup(x => x.Create(It.IsAny<ResourcesSourceProcessorOptions>()))
                .Returns(Mock.Of<IResourcesSourceProcessor>());

            serviceCollection
                .AddSingleton(mapper ?? mapperMock.Object)
                .AddSingleton(writerFactory ?? writerFactoryMock.Object)
                .AddSingleton(processorFactory ?? processorFactoryMock.Object)
                .AddSingleton(fileSystemService ?? Mock.Of<IFileSystemService>());

            return serviceCollection.BuildServiceProvider();
        }
    }
}