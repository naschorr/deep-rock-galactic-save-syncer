using Core.Dwarves;
using Core.Enums;
using Core.Models;

namespace Core.SaveFiles.Models
{
    public abstract class SaveFile : ImmutableFile, IComparable<SaveFile>
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
            Engineer = dwarves[DwarfType.Engineer];
            Scout = dwarves[DwarfType.Scout];
            Driller = dwarves[DwarfType.Driller];
            Gunner = dwarves[DwarfType.Gunner];
        }

        internal SaveFile(ImmutableFile file, Dictionary<DwarfType, Dwarf> dwarves) : base(file.Path, file.Name, file.LastModifiedTime)
        {
            Engineer = dwarves[DwarfType.Engineer];
            Scout = dwarves[DwarfType.Scout];
            Driller = dwarves[DwarfType.Driller];
            Gunner = dwarves[DwarfType.Gunner];
        }

        // Comparison Methods

        public int CompareTo(SaveFile? other)
        {
            if (ReferenceEquals(other, null))
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

        public static bool operator ==(SaveFile? one, SaveFile? two)
        {
            if(ReferenceEquals(one, null))
            {
                return ReferenceEquals(two, null);
            }

            return one.CompareTo(two) == 0;
        }

        public static bool operator !=(SaveFile? one, SaveFile? two)
        {
            if (ReferenceEquals(one, null))
            {
                return !ReferenceEquals(two, null);
            }

            return one.CompareTo(two) != 0;
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
