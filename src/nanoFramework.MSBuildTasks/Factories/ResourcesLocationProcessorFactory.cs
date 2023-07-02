using System;

using nanoFramework.MSBuildTasks.Models;
using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Factories
{
    internal sealed class ResourcesLocationProcessorFactory : IResourcesLocationProcessorFactory
    {
        private readonly IFileSystemService _fileSystemService;

        public ResourcesLocationProcessorFactory(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService ?? throw new ArgumentNullException(nameof(fileSystemService));
        }

        public IResourcesSourceProcessor Create(ResourcesSourceProcessorOptions options)
        {
            return new ResourcesSourceProcessor(_fileSystemService, options);
        }
    }
}
