using System.IO;
using System.IO.Abstractions;

using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.FilePathFilters
{
    public sealed class SearchPatternFilePathFilter : IFilePathFilter
    {
        private readonly IFileSystem _fileSystem;

        public SearchPatternFilePathFilter(string searchPattern, SearchOption searchOption, IFileSystem fileSystem)
        {
            SearchPattern = ParamChecker.Check(searchPattern, nameof(searchPattern));
            _fileSystem = ParamChecker.Check(fileSystem, nameof(fileSystem));
            SearchOption = searchOption;
        }

        public SearchOption SearchOption { get; }

        public string SearchPattern { get; }

        public string[] GetMatchingFilePaths(string basePath)
        {
            ParamChecker.Check(basePath, nameof(basePath));

            return _fileSystem.Directory.GetFiles(basePath, SearchPattern, SearchOption);
        }
    }
}
