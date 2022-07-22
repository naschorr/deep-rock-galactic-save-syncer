using System.Text.RegularExpressions;

namespace Core.Models
{
    public class SemanticVersion : IComparable<SemanticVersion>
    {
        private Regex _SemanticVersionRegex = new Regex(@"\D*(\d+)\.(\d+)\.(\d+)-?(\S*)");

        public readonly int Major;
        public readonly int Minor;
        public readonly int Patch;
        public readonly string? Extension;

        // Constructors

        public SemanticVersion(int major, int minor, int patch, string? extension)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Extension = extension == "" ? null : extension;
        }

        public SemanticVersion(string version)
        {
            // This isn't super robust, however since I control the semver tags, this is fine.
            var matches = _SemanticVersionRegex.Match(version);

            if (matches.Success)
            {
                Major = int.Parse(matches.Groups[1].Value);
                Minor = int.Parse(matches.Groups[2].Value);
                Patch = int.Parse(matches.Groups[3].Value);

                var extension = matches.Groups[4].Value;
                Extension = extension == "" ? null : extension;
            }
            else
            {
                throw new ArgumentException("Invalid version string provided, can't extract semantic versiond data from it.");
            }    
        }

        // Methods

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}{((Extension != null && Extension.Length > 0) ? $"-{Extension}" : "")}";
        }

        // Comparison

        public int CompareTo(SemanticVersion? other)
        {
            if (ReferenceEquals(other, null))
            {
                return 1;
            }

            if (Major > other.Major)
            {
                return 1;
            }
            else if (Major < other.Major)
            {
                return -1;
            }

            if (Minor > other.Minor)
            {
                return 1;
            }
            else if (Minor < other.Minor)
            {
                return -1;
            }

            if (Patch > other.Patch)
            {
                return 1;
            }
            else if (Patch < other.Patch)
            {
                return -1;
            }

            bool extensionIsFalsy = Extension == null || Extension.Length == 0;
            bool otherExtensionIsFalsy = other.Extension == null || other.Extension.Length == 0;
            if (extensionIsFalsy && !otherExtensionIsFalsy)
            {
                return 1;
            }
            else if (extensionIsFalsy && otherExtensionIsFalsy || !extensionIsFalsy && !otherExtensionIsFalsy)
            {
                return 0;
            }
            else if (!extensionIsFalsy && otherExtensionIsFalsy)
            {
                return -1;
            }

            return 0;
        }

        public static bool operator ==(SemanticVersion? one, SemanticVersion? two)
        {
            if (ReferenceEquals(one, null))
            {
                return ReferenceEquals(two, null);
            }

            return one.CompareTo(two) == 0;
        }

        public static bool operator !=(SemanticVersion? one, SemanticVersion? two)
        {
            if (ReferenceEquals(one, null))
            {
                return !ReferenceEquals(two, null);
            }

            return one.CompareTo(two) != 0;
        }

        public static bool operator >(SemanticVersion one, SemanticVersion two)
        {
            return one.CompareTo(two) > 0;
        }

        public static bool operator <(SemanticVersion one, SemanticVersion two)
        {
            return one.CompareTo(two) < 0;
        }

        public static bool operator >=(SemanticVersion one, SemanticVersion two)
        {
            return one.CompareTo(two) >= 0;
        }

        public static bool operator <=(SemanticVersion one, SemanticVersion two)
        {
            return one.CompareTo(two) <= 0;
        }
    }
}
