namespace nanoFramework.MSBuildTasks.Services
{
    public interface IFileSystemService
    {
        string[] GetDirectoryFiles(string path, string regexFilter);

        string GetAbsolutePath(string path, string basePath);
    }
}
