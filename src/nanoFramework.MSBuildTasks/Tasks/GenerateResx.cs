using System.IO.Abstractions;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;

using nanoFramework.MSBuildTasks.Factories;
using nanoFramework.MSBuildTasks.Mappers;
using nanoFramework.MSBuildTasks.Models;
using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Tasks
{
    public sealed class GenerateResx : MSBuildTaskBase
    {
        [Required]
        public string ProjectDirectory { get; set; }

        [Required]
        public string ResxFileName { get; set; }

        [Required]
        public ITaskItem[] TaskItems { get; set; }

        public override void ConfigureServices(IServiceCollection collection)
        {
            collection
                .AddSingleton<ITaskItemMapper<ResourcesSource>, ResourcesSourceMapper>()
                .AddSingleton<INanoResXResourceWriterFactory, NanoResXResourceWriterFactory>()
                .AddSingleton<IResourcesLocationProcessorFactory, ResourcesLocationProcessorFactory>()
                .AddSingleton<IFileSystemService, FileSystemService>()
                .AddSingleton<IFileSystem>(new FileSystem());
        }

        public bool ExecuteTask(
            ITaskItemMapper<ResourcesSource> resourcesSourceMapper,
            INanoResXResourceWriterFactory nanoResxWriterFactory,
            IResourcesLocationProcessorFactory resourcesSourceProcessorFactory)
        {
            var resourcesLocations = TaskItems.Select(resourcesSourceMapper.Map);
            var resXFileWriter = nanoResxWriterFactory.Create(ResxFileName);

            using (var resourcesLocationProcessor = resourcesSourceProcessorFactory.Create(resXFileWriter))
            {
                foreach (var resourcesLocation in resourcesLocations)
                {
                    resourcesLocationProcessor.Process(resourcesLocation, ProjectDirectory);
                }

                resXFileWriter.Generate();
            }

            return true;
        }
    }
}
