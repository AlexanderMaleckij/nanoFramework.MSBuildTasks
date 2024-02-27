namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Models
{
    public sealed class ResXGenerationContext
    {
        public GenerateResXTaskInput TaskInput { get; set; }

        public ResourcesSourceParsedInput[] ResourcesSourceInputs { get; set; }

        public ResourcesSource[] ResourcesSources { get; set; }

        public FileResourceInfo[] FileResourceInfos { get; set; }
    }
}
