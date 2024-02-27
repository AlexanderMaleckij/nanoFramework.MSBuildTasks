using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace nanoFramework.MSBuildTasks.Validation
{
    internal sealed class RegexAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is string regex)
            {
                if (regex == string.Empty)
                {
                    return true;
                }

                try
                {
                    Regex.Match(string.Empty, regex);
                    return true;
                }
                catch (ArgumentException)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
