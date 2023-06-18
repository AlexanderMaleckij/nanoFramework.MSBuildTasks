using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Models
{
    public class ResourcesSourceProcessorOptions
    {
        public string ProjectDirectory { get; set; }

        public INanoResXWriter NanoResXWriter { get; set; }
    }
}
