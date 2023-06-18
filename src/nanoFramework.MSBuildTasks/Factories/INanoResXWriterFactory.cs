using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Factories
{
    public interface INanoResXWriterFactory
    {
        INanoResXWriter Create(string resxFileName);
    }
}
