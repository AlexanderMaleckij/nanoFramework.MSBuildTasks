using System;

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
        public void GivenCreate_WhenCalledWithNullFileName_ThenShouldThrowArgumentNullException()
        {
            // Arrange
            var resxFileName = null as string;
            var factory = new NanoResXResourceWriterFactory();

            // Act
            Action act = () => _ = factory.Create(resxFileName);

            // Assert
            act.Should()
                .ThrowExactly<ArgumentNullException>()
                .WithParameterName("fileName");
        }

        [TestMethod]
        public void GivenCreate_WhenCalled_ThenShouldReturnNotNullInstanceOfNanoResXResourceWriter()
        {
            // Arrange
            var resxFileName = "test.resx";
            var factory = new NanoResXResourceWriterFactory();

            // Act
            var writer = factory.Create(resxFileName);

            // Assert
            writer.Should().NotBeNull().And.BeOfType<NanoResXResourceWriter>();
        }
    }
}
