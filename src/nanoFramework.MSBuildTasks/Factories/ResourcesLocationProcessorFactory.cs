using nanoFramework.MSBuildTasks.Services;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Factories
{
    internal sealed class ResourcesLocationProcessorFactory : IResourcesLocationProcessorFactory
    {
        private readonly IFileSystemService _fileSystemService;

        public ResourcesLocationProcessorFactory(IFileSystemService fileSystemService)
        {
            _fileSystemService = ParamChecker.Check(fileSystemService, nameof(fileSystemService));
        }

        public IResourcesSourceProcessor Create(INanoResXResourceWriter nanoResXResourceWriter) =>
            new ResourcesSourceProcessor(
                _fileSystemService,
                ParamChecker.Check(nanoResXResourceWriter, nameof(nanoResXResourceWriter)));
    }
}
