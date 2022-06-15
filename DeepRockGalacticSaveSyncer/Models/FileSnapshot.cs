using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace DeepRockGalacticSaveSyncer.Models
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    internal class FileSnapshot : IComparable<FileSnapshot>
    {
        // Immutable container for simple File metadata, kind of like a baby FileInfo
        // Exposes modification time based comparison methods

        public string Path { get; }
        public string Name { get; }
        public DateTime LastModifiedTime { get; }

        public FileSnapshot(string path)
        {
            Path = path;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File located at {path} doesn't exist.");
            }

            Name = System.IO.Path.GetFileName(Path);

            LastModifiedTime = File.GetLastWriteTimeUtc(Path);
        }

        public int CompareTo(FileSnapshot? other)
        {
            if (other == null)
            {
                return 1;
            }

            return LastModifiedTime.CompareTo(other.LastModifiedTime);
        }

        public static bool operator >(FileSnapshot one, FileSnapshot two)
        {
            return one.CompareTo(two) > 0;
        }

        public static bool operator <(FileSnapshot one, FileSnapshot two)
        {
            return one.CompareTo(two) < 0;
        }

        public static bool operator >=(FileSnapshot one, FileSnapshot two)
        {
            return one.CompareTo(two) >= 0;
        }

        public static bool operator <=(FileSnapshot one, FileSnapshot two)
        {
            return one.CompareTo(two) <= 0;
        }
    }
}
