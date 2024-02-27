namespace nanoFramework.MSBuildTasks.Pipelines
{
    public interface IPipelineRunner<TContext> where TContext : class
    {
        bool Run(TContext context);
    }
}
