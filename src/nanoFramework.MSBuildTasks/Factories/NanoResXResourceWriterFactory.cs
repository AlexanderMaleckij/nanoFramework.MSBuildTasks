using System.IO;
using System.IO.Abstractions;

using nanoFramework.MSBuildTasks.Services;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Factories
{
    internal sealed class NanoResXResourceWriterFactory : INanoResXResourceWriterFactory
    {
        private readonly IFileSystem _fileSystem;

        public NanoResXResourceWriterFactory(IFileSystem fileSystem)
        {
            _fileSystem = ParamChecker.Check(fileSystem, nameof(fileSystem));
        }

        public INanoResXResourceWriter Create(string resxFileName)
        {
            var fileStream = _fileSystem.FileStream.New(resxFileName, FileMode.Create, FileAccess.Write);

            return new NanoResXResourceWriter(fileStream);
        }
    }
}
