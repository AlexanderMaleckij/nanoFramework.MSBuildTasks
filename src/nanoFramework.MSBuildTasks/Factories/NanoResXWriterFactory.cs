using System;
using System.IO;
using System.IO.Abstractions;

using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Factories
{
    internal sealed class NanoResxWriterFactory : INanoResXWriterFactory
    {
        private readonly IFileSystem _fileSystem;

        public NanoResxWriterFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public INanoResXWriter Create(string resxFileName)
        {
            var fileStream = _fileSystem.FileStream.New(resxFileName, FileMode.Create, FileAccess.Write);

            return new NanoResXWriter(fileStream);
        }
    }
}
