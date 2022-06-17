using DeepRockGalacticSaveSyncer.DwarfManager;
using DeepRockGalacticSaveSyncer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepRockGalacticSaveSyncer.Models
{
    internal class SaveFile : IComparable<SaveFile>
    {
        // Immutable container for Save File data and metadata

        public string Path { get; }
        public string Name { get; }
        public DateTime LastModifiedTime { get; }
        public Dwarf Engineer { get; }
        public Dwarf Scout { get; }
        public Dwarf Driller { get; }
        public Dwarf Gunner { get; }

        public SaveFile(string path)
        {
            Path = path;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File located at {path} doesn't exist.");
            }

            Name = System.IO.Path.GetFileName(Path);
            LastModifiedTime = File.GetLastWriteTimeUtc(Path);

            var dwarves = DwarfFactory.CreateDwarvesFromSaveFile(this);
            Engineer = dwarves[DwarfName.Engineer];
            Scout = dwarves[DwarfName.Scout];
            Driller = dwarves[DwarfName.Driller];
            Gunner = dwarves[DwarfName.Gunner];
        }

        // Comparison Methods

        public int CompareTo(SaveFile? other)
        {
            if (other == null)
            {
                return 1;
            }

            var engineerComparison = Engineer.CompareTo(other.Engineer);
            var scoutComparison = Scout.CompareTo(other.Scout);
            var drillerComparison = Driller.CompareTo(other.Driller);
            var gunnerComparison = Gunner.CompareTo(other.Gunner);

            // Strict dwarf comparison, all of the dwarves in a save must be >= or <= the other save's dwarves.
            if (
                engineerComparison == 0 &&
                scoutComparison == 0 &&
                drillerComparison == 0 &&
                gunnerComparison == 0
            )
            {
                return LastModifiedTime.CompareTo(other.LastModifiedTime);
            }
            else if (
                engineerComparison >= 0 &&
                scoutComparison >= 0 &&
                drillerComparison >= 0 &&
                gunnerComparison >= 0
            )
            {
                return 1;
            }
            else if (
                engineerComparison <= 0 &&
                scoutComparison <= 0 &&
                drillerComparison <= 0 &&
                gunnerComparison <= 0
            )
            {
                return -1;
            }

            // Failing that, just default to whichever file is newer
            return LastModifiedTime.CompareTo(other.LastModifiedTime);
        }

        public static bool operator >(SaveFile one, SaveFile two)
        {
            return one.CompareTo(two) > 0;
        }

        public static bool operator <(SaveFile one, SaveFile two)
        {
            return one.CompareTo(two) < 0;
        }

        public static bool operator >=(SaveFile one, SaveFile two)
        {
            return one.CompareTo(two) >= 0;
        }

        public static bool operator <=(SaveFile one, SaveFile two)
        {
            return one.CompareTo(two) <= 0;
        }
    }
}
