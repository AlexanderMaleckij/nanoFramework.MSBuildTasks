using nanoFramework.MSBuildTasks.Models;
using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Factories
{
    public interface IResourcesLocationProcessorFactory
    {
        IResourcesSourceProcessor Create(ResourcesSourceProcessorOptions options);
    }
}
