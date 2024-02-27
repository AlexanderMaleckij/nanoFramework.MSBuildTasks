using Microsoft.Build.Framework;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Models
{
    public sealed class GenerateResXTaskInput
    {
        public string ProjectDirectory { get; set; }

        public string ProjectFullPath { get; set; }

        public string ResXFileName { get; set; }

        public ITaskItem[] TaskItems { get; set; }
    }
}
