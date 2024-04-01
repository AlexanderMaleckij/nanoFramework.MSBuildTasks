using System.ComponentModel.DataAnnotations;
using System.IO.Abstractions;

using Microsoft.Build.Utilities;

using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Steps
{
    internal sealed class ValidateInputStep : IPipelineStep<ResXGenerationContext>
    {
        private readonly IFileSystem _fileSystem;
        private readonly TaskLoggingHelper _taskLoggingHelper;

        public ValidateInputStep(IFileSystem fileSystem, TaskLoggingHelper taskLoggingHelper)
        {
            _fileSystem = ParamChecker.Check(fileSystem, nameof(fileSystem));
            _taskLoggingHelper = ParamChecker.Check(taskLoggingHelper, nameof(taskLoggingHelper));
        }

        public void Handle(ResXGenerationContext context)
        {
            _taskLoggingHelper.LogMessage("Validating input");

            foreach (var resourcesSource in context.ResourcesSourceInputs)
            {
                var validationContext = new ValidationContext(resourcesSource);

                if (!_fileSystem.Directory.Exists(resourcesSource.DirectoryPath))
                {
                    _taskLoggingHelper.LogError(
                        "validation",
                        "RG0001",
                        null,
                        context.TaskInput.ProjectFullPath,
                        0,
                        0,
                        0,
                        0,
                        "Directory specified in the ResourcesSource node does not exist. Directory: {0}",
                        resourcesSource.DirectoryPath);
                }

                try
                {
                    Validator.ValidateObject(resourcesSource, validationContext, true);
                }
                catch (ValidationException vex)
                {
                    _taskLoggingHelper.LogError(
                        "validation",
                        "RG0001",
                        null,
                        context.TaskInput.ProjectFullPath,
                        0,
                        0,
                        0,
                        0,
                        vex.Value != null
                            ? $"{vex.Message} Value: \"{vex.Value}\"."
                            : vex.Message);
                }
            }
        }
    }
}
