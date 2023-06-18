using Microsoft.Build.Framework;

namespace nanoFramework.MSBuildTasks.Mappers
{
    public interface ITaskItemMapper<out TDst>
    {
        TDst Map(ITaskItem taskItem);
    }
}
