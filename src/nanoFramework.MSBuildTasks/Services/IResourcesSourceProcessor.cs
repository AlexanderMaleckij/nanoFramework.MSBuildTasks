using nanoFramework.MSBuildTasks.Models;

namespace nanoFramework.MSBuildTasks.Services
{
    public interface IResourcesSourceProcessor
    {
        void Process(ResourcesSource resourcesLocation);
    }
}
