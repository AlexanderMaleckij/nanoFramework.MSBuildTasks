using System;

using nanoFramework.MSBuildTasks.Models;

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
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
            _processorOptions = processorOptions ?? throw new ArgumentNullException(nameof(processorOptions));
        }

        public void Process(ResourcesSource resourcesLocation)
        {
            if (resourcesLocation is null)
            {
                throw new ArgumentNullException(nameof(resourcesLocation));
            }

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
