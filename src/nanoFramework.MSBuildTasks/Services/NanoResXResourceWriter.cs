using System.IO;
using System.Resources;

using nanoFramework.MSBuildTasks.Utils;

namespace nanoFramework.MSBuildTasks.Services
{
    internal sealed class NanoResXResourceWriter : INanoResXResourceWriter
    {
        private readonly ResXResourceWriter _resXResourceWriter;

        public NanoResXResourceWriter(Stream stream)
        {
            ParamChecker.Check(stream, nameof(stream));

            _resXResourceWriter = new ResXResourceWriter(stream);
        }

        public void Add(string resourceName, string resourcePath)
        {
            var fileRef = new ResXFileRef(resourcePath, typeof(byte[]).AssemblyQualifiedName);
            var dataNode = new ResXDataNode(resourceName, fileRef);

            _resXResourceWriter.AddResource(dataNode);
        }

        public void Generate() => _resXResourceWriter.Generate();

        public void Dispose() => _resXResourceWriter.Dispose();
    }
}
