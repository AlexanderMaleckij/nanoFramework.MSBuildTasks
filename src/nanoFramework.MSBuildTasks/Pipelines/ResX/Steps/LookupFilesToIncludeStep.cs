using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Utilities;

using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Services;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Steps
{
    internal sealed class LookupFilesToIncludeStep : IPipelineStep<ResXGenerationContext>
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly TaskLoggingHelper _taskLoggingHelper;

        public LookupFilesToIncludeStep(
            IFileSystemService fileSystemService,
            TaskLoggingHelper taskLoggingHelper)
        {
            _fileSystemService = ParamChecker.Check(fileSystemService, nameof(fileSystemService));
            _taskLoggingHelper = ParamChecker.Check(taskLoggingHelper, nameof(taskLoggingHelper));
        }

        public void Handle(ResXGenerationContext context)
        {
            _taskLoggingHelper.LogMessage("Looking up files to include into resource file");

            var fileResourceInfos = new HashSet<FileResourceInfo>();

            foreach (var resourcesSource in context.ResourcesSources)
            {
                var absoluteDirectoryPath = _fileSystemService.GetAbsolutePath(resourcesSource.DirectoryPath, context.TaskInput.ProjectDirectory);
                var resourceFilesFullPaths = resourcesSource.PathFilter.GetMatchingFilePaths(absoluteDirectoryPath);

                foreach (var resourceFileFullPath in resourceFilesFullPaths)
                {
                    var relativeFilePath = resourceFileFullPath.Substring(absoluteDirectoryPath.Length + 1);
                    var resourceName = relativeFilePath.Replace('\\', '/');

                    fileResourceInfos.Add(
                        new FileResourceInfo
                        {
                            Name = resourceName,
                            Path = resourceFileFullPath,
                        });
                }
            }

            context.FileResourceInfos = fileResourceInfos.ToArray();
        }
    }
}
