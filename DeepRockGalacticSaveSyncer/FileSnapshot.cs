using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace DeepRockGalacticSaveSwapper
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    internal class FileSnapshot : IComparable<FileSnapshot>
    {
        // Immutable container for simple File metadata, kind of like a baby FileInfo
        // Exposes modification time based comparison methods

        public string Path { get; }
        public string Name { get; }
        public DateTime LastModifiedTime { get; }

        public FileSnapshot(String path)
        {
            this.Path = path;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File located at {path} doesn't exist.");
            }

            this.Name = System.IO.Path.GetFileName(this.Path);

            this.LastModifiedTime = File.GetLastWriteTimeUtc(this.Path);
        }

        public int CompareTo(FileSnapshot? other)
        {
            if (other == null)
            {
                return 1;
            }

            return LastModifiedTime.CompareTo(other.LastModifiedTime);
        }

        public static bool operator > (FileSnapshot one, FileSnapshot two)
        {
            return one.CompareTo(two) > 0;
        }

        public static bool operator < (FileSnapshot one, FileSnapshot two)
        {
            return one.CompareTo(two) < 0;
        }

        public static bool operator >= (FileSnapshot one, FileSnapshot two)
        {
            return one.CompareTo(two) >= 0;
        }

        public static bool operator <= (FileSnapshot one, FileSnapshot two)
        {
            return one.CompareTo(two) <= 0;
        }
    }
}
