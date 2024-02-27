using System;
using System.Diagnostics;

namespace nanoFramework.MSBuildTasks.Utils
{
    internal static class ParamChecker
    {
        [DebuggerHidden]
        public static T Check<T>(T parameter, string parameterName)
            where T : class
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return parameter;
        }
    }
}
