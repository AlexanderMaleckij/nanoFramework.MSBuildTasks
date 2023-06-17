using System;
using System.IO;
using System.Linq;
using System.Resources;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using nanoFramework.MSBuildTasks.Models;

namespace nanoFramework.MSBuildTasks.Tasks
{
    public sealed class GenerateResx : Task
    {
        [Required]
        public string ProjectDirectory { get; set; }

        [Required]
        public string ResxFileName { get; set; }

        [Required]
        public bool ResxUseForwardSlashes { get; set; }

        [Required]
        public ITaskItem[] Sources { get; set; }

        public override bool Execute()
        {
            EmbedResourcesTaskItem[] sources = null;

            try
            {
                sources = Sources.Select(x => new EmbedResourcesTaskItem(ProjectDirectory, x)).ToArray();
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);

                return false;
            }

            var resourceWriter = new ResXResourceWriter(ResxFileName);

            foreach (var source in sources)
            {
                var fullFilesPathsToInclude = Directory.GetFiles(source.AbsoluteFolderPath, "*", SearchOption.AllDirectories)
                    .Where(path => source.RegexFilter.IsMatch(path))
                    .ToArray();

                foreach (var fullFilePathToInclude in fullFilesPathsToInclude)
                {
                    var relativeFilePath = fullFilePathToInclude.Substring(source.AbsoluteFolderPath.Length + 1);

                    var resourceName = ResxUseForwardSlashes
                        ? relativeFilePath.Replace('\\', '/')
                        : relativeFilePath;

                    var fileRef = new ResXFileRef(fullFilePathToInclude, typeof(byte[]).AssemblyQualifiedName);
                    var dataNode = new ResXDataNode(resourceName, fileRef);

                    resourceWriter.AddResource(dataNode);
                }
            }

            resourceWriter.Generate();
            resourceWriter.Close();

            return true;
        }
    }
}
