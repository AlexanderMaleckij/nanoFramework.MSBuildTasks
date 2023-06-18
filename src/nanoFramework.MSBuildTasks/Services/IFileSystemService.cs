namespace nanoFramework.MSBuildTasks.Services
{
    internal interface IFileSystemService
    {
        string[] GetDirectoryFiles(string path, string regexFilter);

        string GetAbsolutePath(string path, string basePath);
    }
}
