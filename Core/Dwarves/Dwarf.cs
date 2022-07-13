namespace Core.Dwarves
{
    public class Dwarf
    {
        public int Promotions { get; }
        public int Experience { get; }
        public int Level {
            get
            {
                return CalculateLevel();
            }
        }

        public Dwarf(int promotions, int experience)
        {
            Promotions = promotions;
            Experience = experience;
        }

        // Methods

        private int CalculateLevel()
        {
            // See: https://deeprockgalactic.fandom.com/wiki/Experience

            var level = 1;
            var levelFound = false;
            var experienceAtCurrentLevel = 0;
            var experienceToNextLevel = 3000;
            while (!levelFound && level < 25)
            {
                if (experienceAtCurrentLevel <= Experience && experienceAtCurrentLevel + experienceToNextLevel > Experience)
                {
                    levelFound = true;
                }
                else
                {
                    level += 1;
                    experienceAtCurrentLevel += experienceToNextLevel;

                    if (level < 14)
                    {
                        experienceToNextLevel += 1000;
                    }
                    else
                    {
                        experienceToNextLevel += 500;
                    }
                }
            }

            return level;
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

        public static bool operator ==(Dwarf? one, Dwarf? two)
        {
            if (ReferenceEquals(one, null))
            {
                return ReferenceEquals(two, null);
            }

            return one.CompareTo(two) == 0;
        }

        public static bool operator !=(Dwarf? one, Dwarf? two)
        {
            if (ReferenceEquals(one, null))
            {
                return !ReferenceEquals(two, null);
            }

            return one.CompareTo(two) != 0;
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
