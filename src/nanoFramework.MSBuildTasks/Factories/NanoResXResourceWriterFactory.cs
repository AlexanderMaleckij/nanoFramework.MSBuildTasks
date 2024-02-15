using nanoFramework.MSBuildTasks.Services;
using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Factories
{
    internal sealed class NanoResXResourceWriterFactory : INanoResXResourceWriterFactory
    {
        public INanoResXResourceWriter Create(string fileName) =>
            new NanoResXResourceWriter(ParamChecker.Check(fileName, nameof(fileName)));
    }
}
