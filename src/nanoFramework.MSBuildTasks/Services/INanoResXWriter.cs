using System;

namespace nanoFramework.MSBuildTasks.Services
{
    public interface INanoResXWriter : IDisposable
    {
        void Add(string resourceName, string resourcePath);

        void Generate();
    }
}
