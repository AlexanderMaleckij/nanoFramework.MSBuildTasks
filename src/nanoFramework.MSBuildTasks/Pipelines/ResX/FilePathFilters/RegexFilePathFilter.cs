using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;

using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.FilePathFilters
{
    public sealed class RegexFilePathFilter : IFilePathFilter
    {
        private readonly Regex _regex;
        private readonly IFileSystem _fileSystem;

        public RegexFilePathFilter(string pattern, IFileSystem fileSystem)
        {
            _regex = new Regex(pattern);
            _fileSystem = ParamChecker.Check(fileSystem, nameof(fileSystem));
        }

        public string Pattern => _regex.ToString();

        public string[] GetMatchingFilePaths(string basePath)
        {
            ParamChecker.Check(basePath, nameof(basePath));

            return _fileSystem.Directory
                .GetFiles(basePath, "*", SearchOption.AllDirectories)
                .Where(filePath => _regex.IsMatch(filePath))
                .ToArray();
        }
    }
}
