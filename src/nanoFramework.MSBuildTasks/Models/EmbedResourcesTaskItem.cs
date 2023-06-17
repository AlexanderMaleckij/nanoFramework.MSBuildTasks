using System;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Build.Framework;

namespace nanoFramework.MSBuildTasks.Models
{
    internal sealed class EmbedResourcesTaskItem
    {
        private const string REGEX_FILTER_METADATA_NAME = "RegexFilter";
        private const string MetadataRequiredTemplate = "\"{0}\" metadata is required.";
        private const string DirectoryDoesNotExistTemplate = "Directory \"{0}\" does not exist.";

        public string AbsoluteFolderPath { get; }

        public Regex RegexFilter { get; }

        public EmbedResourcesTaskItem(string projectDirectory, ITaskItem taskItem)
        {
            if (taskItem is null)
            {
                throw new ArgumentNullException(nameof(taskItem));
            }

            var folderPath = taskItem.ItemSpec;
            var pattern = taskItem.GetMetadata(REGEX_FILTER_METADATA_NAME);

            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentException(
                    string.Format(MetadataRequiredTemplate, REGEX_FILTER_METADATA_NAME));
            }

            var absoluteFolderPath = GetAbsolutePath(folderPath, projectDirectory);

            if (!Directory.Exists(absoluteFolderPath))
            {
                throw new ArgumentException(
                    string.Format(DirectoryDoesNotExistTemplate, folderPath));
            }

            AbsoluteFolderPath = absoluteFolderPath;
            RegexFilter = new Regex(pattern);
        }

        static string GetAbsolutePath(string path, string basePath)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                return Path.GetFullPath(Path.Combine(basePath, path));
            }
        }
    }
}
