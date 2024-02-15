using System;

using nanoFramework.MSBuildTasks.Models;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Services
{
    internal sealed class ResourcesSourceProcessor : IResourcesSourceProcessor
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly INanoResXResourceWriter _nanoResXResourceWriter;

        public ResourcesSourceProcessor(
            IFileSystemService fileSystemService,
            INanoResXResourceWriter nanoResXResourceWriter)
        {
            _fileSystemService = ParamChecker.Check(fileSystemService, nameof(fileSystemService));
            _nanoResXResourceWriter = ParamChecker.Check(nanoResXResourceWriter, nameof(nanoResXResourceWriter));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _nanoResXResourceWriter.Dispose();
        }

        public void Process(ResourcesSource resourcesLocation, string projectDirectory)
        {
            ParamChecker.Check(resourcesLocation, nameof(resourcesLocation));
            ParamChecker.Check(projectDirectory, nameof(projectDirectory));

            var absoluteFolderPath = _fileSystemService.GetAbsolutePath(resourcesLocation.FolderPath, projectDirectory);
            var directoryFilesFullPaths = _fileSystemService.GetDirectoryFiles(absoluteFolderPath, resourcesLocation.RegexFilter);

            foreach (var directoryFileFullPath in directoryFilesFullPaths)
            {
                var relativeFilePath = directoryFileFullPath.Substring(absoluteFolderPath.Length + 1);
                var resourceName = relativeFilePath.Replace('\\', '/');

                _nanoResXResourceWriter.Add(resourceName, directoryFileFullPath);
            }
        }
    }
}
