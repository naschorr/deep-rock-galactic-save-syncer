﻿using DeepRockGalacticSaveSyncer.DwarfManager;
using DeepRockGalacticSaveSyncer.Enums;

namespace DeepRockGalacticSaveSyncer.Models
{
    internal class SaveFile : ImmutableFile, IComparable<SaveFile>
    {
        // Immutable container for Save File data and metadata

        public Dwarf Engineer { get; }
        public Dwarf Scout { get; }
        public Dwarf Driller { get; }
        public Dwarf Gunner { get; }

        // Constructors

        public SaveFile(string path) : base(path)
        {
            var dwarves = DwarfFactory.CreateDwarvesFromSaveFile(this);
            Engineer = dwarves[DwarfName.Engineer];
            Scout = dwarves[DwarfName.Scout];
            Driller = dwarves[DwarfName.Driller];
            Gunner = dwarves[DwarfName.Gunner];
        }

        public SaveFile(ImmutableFile file, Dictionary<DwarfName, Dwarf> dwarves) : base(file.Path, file.Name, file.LastModifiedTime)
        {
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
