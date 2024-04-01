using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;

using Microsoft.Build.Framework;
using Microsoft.Extensions.DependencyInjection;

using nanoFramework.MSBuildTasks.Pipelines;
using nanoFramework.MSBuildTasks.Pipelines.ResX.Models;
using nanoFramework.MSBuildTasks.Pipelines.ResX.Steps;
using nanoFramework.MSBuildTasks.Services;

namespace nanoFramework.MSBuildTasks.Tasks
{
    public sealed class GenerateResx : MSBuildTaskBase
    {
        [Required]
        public string ProjectDirectory { get; set; }

        [Required]
        public string ProjectFullPath { get; set; }

        [Required]
        public string ResXFileName { get; set; }

        [Required]
        public ITaskItem[] TaskItems { get; set; }

        [ExcludeFromCodeCoverage]
        public override void ConfigureServices(IServiceCollection collection)
        {
            collection.AddSingleton(Log);

            collection
                .AddSingleton<IFileSystemService, FileSystemService>()
                .AddSingleton<IFileSystem>(new FileSystem());

            collection
                .AddSingleton<IPipelineStep<ResXGenerationContext>, ParseInputStep>()
                .AddSingleton<IPipelineStep<ResXGenerationContext>, ValidateInputStep>()
                .AddSingleton<IPipelineStep<ResXGenerationContext>, TransformInputStep>()
                .AddSingleton<IPipelineStep<ResXGenerationContext>, LookupFilesToIncludeStep>()
                .AddSingleton<IPipelineStep<ResXGenerationContext>, GenerateResXFileStep>()
                .AddSingleton<IPipelineRunner<ResXGenerationContext>, PipelineRunner<ResXGenerationContext>>();
        }

        public bool ExecuteTask(IPipelineRunner<ResXGenerationContext> pipelineRunner)
        {
            var context = new ResXGenerationContext
            {
                TaskInput = new GenerateResXTaskInput
                {
                    ProjectDirectory = ProjectDirectory,
                    ProjectFullPath = ProjectFullPath,
                    ResXFileName = ResXFileName,
                    TaskItems = TaskItems,
                }
            };

            return pipelineRunner.Run(context);
        }
    }
}
