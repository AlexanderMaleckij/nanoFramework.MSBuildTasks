using System;

using nanoFramework.MSBuildTasks.Models;

namespace nanoFramework.MSBuildTasks.Services
{
    public interface IResourcesSourceProcessor : IDisposable
    {
        void Process(ResourcesSource resourcesLocation, string projectDirectory);
    }
}
