using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;

namespace nanoFramework.MSBuildTasks.Services
{
    internal sealed class FileSystemService : IFileSystemService
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem
                ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public string[] GetDirectoryFiles(string path, string regexFilter)
        {
            var filter = new Regex(regexFilter);
            var matchingDirectoryFiles = _fileSystem.Directory
                .GetFiles(path, "*", SearchOption.AllDirectories)
                .Where(filePath => filter.IsMatch(filePath))
                .ToArray();

            return matchingDirectoryFiles;
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
