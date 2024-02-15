using nanoFramework.MSBuildTasks.Models;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Services
{
    internal sealed class ResourcesSourceProcessor : IResourcesSourceProcessor
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly ResourcesSourceProcessorOptions _processorOptions;

        public ResourcesSourceProcessor(
            IFileSystemService fileSystemService,
            ResourcesSourceProcessorOptions processorOptions)
        {
            _fileSystemService = ParamChecker.Check(fileSystemService, nameof(fileSystemService));
            _processorOptions = ParamChecker.Check(processorOptions, nameof(processorOptions));
        }

        public void Process(ResourcesSource resourcesLocation)
        {
            ParamChecker.Check(resourcesLocation, nameof(resourcesLocation));

            var absoluteFolderPath = _fileSystemService.GetAbsolutePath(resourcesLocation.FolderPath, _processorOptions.ProjectDirectory);
            var directoryFilesFullPaths = _fileSystemService.GetDirectoryFiles(absoluteFolderPath, resourcesLocation.RegexFilter);

            foreach (var directoryFileFullPath in directoryFilesFullPaths)
            {
                var relativeFilePath = directoryFileFullPath.Substring(absoluteFolderPath.Length + 1);
                var resourceName = relativeFilePath.Replace('\\', '/');

                _processorOptions.NanoResXWriter.Add(resourceName, directoryFileFullPath);
            }
        }
    }
}
