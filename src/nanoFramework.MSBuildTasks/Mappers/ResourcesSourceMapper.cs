using Microsoft.Build.Framework;

using nanoFramework.MSBuildTasks.Models;

namespace nanoFramework.MSBuildTasks.Mappers
{
    internal class ResourcesSourceMapper : ITaskItemMapper<ResourcesSource>
    {
        private const string REGEX_FILTER_METADATA_NAME = "RegexFilter";

        public ResourcesSource Map(ITaskItem taskItem)
        {
            var folderPath = taskItem.ItemSpec;
            var pattern = taskItem.GetMetadata(REGEX_FILTER_METADATA_NAME);

            return new ResourcesSource
            {
                FolderPath = folderPath,
                RegexFilter = pattern
            };
        }
    }
}
