using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepRockGalacticSaveSyncer.Models
{
    internal class Dwarf
    {
        public int Promotions { get; }
        public int Experience { get; }

        public Dwarf(int promotions, int experience)
        {
            Promotions = promotions;
            Experience = experience;
        }

        // Comparison Methods

        public int CompareTo(Dwarf? other)
        {
            if (other == null)
            {
                return 1;
            }

            // If this dwarf is has the same amount of promotions as the other, then check for experience differences
            var promotionsComparison = Promotions.CompareTo(other.Promotions);
            if (promotionsComparison == 0)
            {
                return Experience.CompareTo(other.Experience);
            }

            // Otherwise, this will determine precedence just fine
            return promotionsComparison;
        }

        public static bool operator >(Dwarf one, Dwarf two)
        {
            return one.CompareTo(two) > 0;
        }

        public static bool operator <(Dwarf one, Dwarf two)
        {
            return one.CompareTo(two) < 0;
        }

        public static bool operator >=(Dwarf one, Dwarf two)
        {
            return one.CompareTo(two) >= 0;
        }

        public static bool operator <=(Dwarf one, Dwarf two)
        {
            return one.CompareTo(two) <= 0;
        }
    }
}
