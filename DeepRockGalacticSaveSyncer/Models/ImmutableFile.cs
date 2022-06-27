namespace DeepRockGalacticSaveSyncer.Models
{
    internal class ImmutableFile
    {
        // Simple immutable file container

        public string Path { get; }
        public string Name { get; }
        public DateTime LastModifiedTime { get; }

        public ImmutableFile(string path)
        {
            Path = path;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File located at {path} doesn't exist.");
            }

            Name = System.IO.Path.GetFileName(Path);
            LastModifiedTime = File.GetLastWriteTimeUtc(Path);
        }

        internal ImmutableFile(string path, string name, DateTime lastModifiedTime)
        {
            Path = path;
            Name = name;
            LastModifiedTime = lastModifiedTime;
        }
    }
}
