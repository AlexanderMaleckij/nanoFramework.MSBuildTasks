using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Factories
{
    public interface INanoResXResourceWriterFactory
    {
        INanoResXResourceWriter Create(string fileName);
    }
}
