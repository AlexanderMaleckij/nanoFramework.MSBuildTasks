using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using nanoFramework.MSBuildTasks.Factories;
using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.UnitTests.Factories
{
    [TestClass]
    public class NanoResxResourceWriterFactoryTests
    {
        [TestMethod]
        public void GivenConstructor_WhenFileSystemIsNull_ThemShouldThrowArgumentNullException()
        {
            // Arrange
            var fileSystem = null as IFileSystem;

            // Act
            Action act = () => new NanoResXResourceWriterFactory(fileSystem);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>()
                .WithParameterName("fileSystem");
        }

        [TestMethod]
        public void GivenCreate_WhenCalled_ThenShouldReturnNotNullInstanceOfNanoResXWriter()
        {
            // Arrange
            var resxFileName = "test.resx";
            var fileSystem = new MockFileSystem();
            var factory = new NanoResXResourceWriterFactory(fileSystem);

            // Act
            var writer = factory.Create(resxFileName);

            // Assert
            writer.Should().NotBeNull().And.BeOfType<NanoResXResourceWriter>();
        }
    }
}
