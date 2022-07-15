namespace Core.Models
{
    public class SemanticVersion
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Patch;

        // Constructors

        public SemanticVersion(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public SemanticVersion(IEnumerable<int> version) : this(version.ElementAt(0), version.ElementAt(1), version.ElementAt(2)) { }

        public SemanticVersion(string version) : this(SemanticVersion.ConvertVersionStringToArray(version)) { }

        // Methods

        private static int[] ConvertVersionStringToArray(string version)
        {
            return version.Split('.').Select(e => int.Parse(e)).ToArray();
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }
    }
}
