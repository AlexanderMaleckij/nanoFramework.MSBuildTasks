using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Steps
{
    internal sealed class ParseInputStep : IPipelineStep<ResXGenerationContext>
    {
        private readonly TaskLoggingHelper _taskLoggingHelper;

        public ParseInputStep(TaskLoggingHelper taskLoggingHelper)
        {
            _taskLoggingHelper = ParamChecker.Check(taskLoggingHelper, nameof(taskLoggingHelper));
        }

        public void Handle(ResXGenerationContext context)
        {
            _taskLoggingHelper.LogMessage("Parsing input");

            context.ResourcesSourceInputs = context.TaskInput.TaskItems.Select(Map).ToArray();
        }

        private static ResourcesSourceParsedInput Map(ITaskItem taskItem) =>
            new ResourcesSourceParsedInput
            {
                DirectoryPath = taskItem.ItemSpec,
                RegexFilter = taskItem.GetMetadata(nameof(ResourcesSourceParsedInput.RegexFilter)),
                SearchPattern = taskItem.GetMetadata(nameof(ResourcesSourceParsedInput.SearchPattern)),
            };
    }
}
