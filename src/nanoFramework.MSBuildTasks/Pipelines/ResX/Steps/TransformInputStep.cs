using System.IO.Abstractions;
using System.Linq;

using Microsoft.Build.Utilities;

using nanoFramework.MSBuildTasks.Pipelines.ResX.FilePathFilters;
using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Steps
{
    internal sealed class TransformInputStep : IPipelineStep<ResXGenerationContext>
    {
        private readonly TaskLoggingHelper _taskLoggingHelper;
        private readonly IFileSystem _fileSystem;

        public TransformInputStep(TaskLoggingHelper taskLoggingHelper, IFileSystem fileSystem)
        {
            _taskLoggingHelper = ParamChecker.Check(taskLoggingHelper, nameof(taskLoggingHelper));
            _fileSystem = ParamChecker.Check(fileSystem, nameof(fileSystem));
        }

        public void Handle(ResXGenerationContext context)
        {
            _taskLoggingHelper.LogMessage("Transforming input");

            context.ResourcesSources = context.ResourcesSourceInputs.Select(Transform).ToArray();
        }

        private ResourcesSource Transform(ResourcesSourceParsedInput input)
        {
            IFilePathFilter filter = null;

            if (!string.IsNullOrEmpty(input.SearchPattern))
            {
                filter = new SearchPatternFilePathFilter(input.SearchPattern, _fileSystem);
            }

            if (!string.IsNullOrEmpty(input.RegexFilter))
            {
                filter = new RegexFilePathFilter(input.RegexFilter, _fileSystem);
            }

            filter = filter ?? new SearchPatternFilePathFilter("*", _fileSystem);

            return new ResourcesSource(input.DirectoryPath, filter);
        }
    }
}
