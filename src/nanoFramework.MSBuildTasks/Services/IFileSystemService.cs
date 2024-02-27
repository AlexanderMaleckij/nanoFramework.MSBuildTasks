namespace nanoFramework.MSBuildTasks.Services
{
    public interface IFileSystemService
    {
        string GetAbsolutePath(string path, string basePath);
    }
}
