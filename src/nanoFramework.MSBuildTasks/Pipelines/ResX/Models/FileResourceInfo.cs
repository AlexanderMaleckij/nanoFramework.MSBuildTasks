namespace nanoFramework.MSBuildTasks.Pipelines.ResX.Models
{
    public sealed class FileResourceInfo
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj is FileResourceInfo other)
            {
                return Name == other.Name && Path == other.Path;
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Name != null ? Name.GetHashCode() : 0);
                hash = hash * 23 + (Path != null ? Path.GetHashCode() : 0);
                return hash;
            }
        }
    }
}
