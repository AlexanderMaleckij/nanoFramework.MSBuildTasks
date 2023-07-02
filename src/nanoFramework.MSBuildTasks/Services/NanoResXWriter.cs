using System;
using System.IO;
using System.Resources;

namespace nanoFramework.MSBuildTasks.Services
{
    internal sealed class NanoResXWriter : INanoResXWriter
    {
        private readonly ResXResourceWriter _resXResourceWriter;

        public NanoResXWriter(Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

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
