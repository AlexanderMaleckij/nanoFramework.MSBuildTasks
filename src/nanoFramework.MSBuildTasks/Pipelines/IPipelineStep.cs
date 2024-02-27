namespace nanoFramework.MSBuildTasks.Pipelines
{
    public interface IPipelineStep<TContext>
    {
        void Handle(TContext context);
    }
}
