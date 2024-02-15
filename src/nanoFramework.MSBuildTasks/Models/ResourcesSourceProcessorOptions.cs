using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Models
{
    public class ResourcesSourceProcessorOptions
    {
        public string ProjectDirectory { get; set; }

        public INanoResXResourceWriter NanoResXWriter { get; set; }
    }
}
