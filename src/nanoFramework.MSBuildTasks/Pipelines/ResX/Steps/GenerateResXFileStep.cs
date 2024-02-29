using System.IO;
using System.IO.Abstractions;
using System.Resources;

using Microsoft.Build.Utilities;

using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Steps
{
    internal sealed class GenerateResXFileStep : IPipelineStep<ResXGenerationContext>
    {
        private readonly TaskLoggingHelper _taskLoggingHelper;
        private readonly IFileSystem _fileSystem;

        public GenerateResXFileStep(TaskLoggingHelper taskLoggingHelper, IFileSystem fileSystem)
        {
            _taskLoggingHelper = ParamChecker.Check(taskLoggingHelper, nameof(taskLoggingHelper));
            _fileSystem = ParamChecker.Check(fileSystem, nameof(fileSystem));
        }

        public void Handle(ResXGenerationContext context)
        {
            _taskLoggingHelper.LogMessage("Generating resource file. File name = {fileName}", context.TaskInput.ResXFileName);

            using (var fileStream = _fileSystem.File.Open(context.TaskInput.ResXFileName, FileMode.Create))
            {
                using (var writer = new ResXResourceWriter(fileStream))
                {
                    foreach (var resource in context.FileResourceInfos)
                    {
                        var fileRef = new ResXFileRef(resource.Path, typeof(byte[]).AssemblyQualifiedName);
                        var dataNode = new ResXDataNode(resource.Name, fileRef);

                        writer.AddResource(dataNode);
                    }

                    writer.Generate();
                }
            }
        }
    }
}
