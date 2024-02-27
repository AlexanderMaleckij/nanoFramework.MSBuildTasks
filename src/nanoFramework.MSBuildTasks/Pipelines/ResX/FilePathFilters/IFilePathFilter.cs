namespace nanoFramework.MSBuildTasks.Pipelines.ResX.FilePathFilters
{
    public interface IFilePathFilter
    {
        string[] GetMatchingFilePaths(string basePath);
    }
}
