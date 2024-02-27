using nanoFramework.MSBuildTasks.Pipelines.ResX.FilePathFilters;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Models
{
    public sealed class ResourcesSource
    {
        public ResourcesSource(string directoryPath, IFilePathFilter pathFilter)
        {
            DirectoryPath = ParamChecker.Check(directoryPath, nameof(directoryPath));
            PathFilter = ParamChecker.Check(pathFilter, nameof(pathFilter));
        }

        public string DirectoryPath { get; }

        public IFilePathFilter PathFilter { get; }
    }
}
