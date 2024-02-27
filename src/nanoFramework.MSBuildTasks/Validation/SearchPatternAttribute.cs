using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace nanoFramework.MSBuildTasks.Validation
{
    internal sealed class SearchPatternAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is string searchPattern)
            {
                if (searchPattern == string.Empty)
                {
                    return true;
                }

                return IsSearchPatternValid(searchPattern);
            }

            return false;
        }

        // https://referencesource.microsoft.com/#mscorlib/system/io/path.cs,b949d580e3b5631a,references
        [ExcludeFromCodeCoverage]
        private static bool IsSearchPatternValid(string searchPattern)
        {
            int index;
            while ((index = searchPattern.IndexOf("..", StringComparison.Ordinal)) != -1)
            {
                // Terminal ".." . Files names cannot end in ".."
                if (index + 2 == searchPattern.Length)
                {
                    return false;
                }

                if ((searchPattern[index + 2] == Path.DirectorySeparatorChar)
                   || (searchPattern[index + 2] == Path.AltDirectorySeparatorChar))
                {
                    return false;
                }

                searchPattern = searchPattern.Substring(index + 2);
            }

            return true;
        }
    }
}
