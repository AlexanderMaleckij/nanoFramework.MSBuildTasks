using System;

namespace nanoFramework.MSBuildTasks.Services
{
    public interface INanoResXResourceWriter : IDisposable
    {
        void Add(string resourceName, string resourcePath);

        void Generate();
    }
}
