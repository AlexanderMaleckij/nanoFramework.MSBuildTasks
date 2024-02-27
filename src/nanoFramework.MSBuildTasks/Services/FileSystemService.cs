using System.IO.Abstractions;

using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Services
{
    internal sealed class FileSystemService : IFileSystemService
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemService(IFileSystem fileSystem)
        {
            _fileSystem = ParamChecker.Check(fileSystem, nameof(fileSystem));
        }

        public string GetAbsolutePath(string path, string basePath)
        {
            if (_fileSystem.Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                return _fileSystem.Path.GetFullPath(
                    _fileSystem.Path.Combine(basePath, path));
            }
        }
    }
}
