using Core.Enums;

namespace Core.Dwarves
{
    public class Dwarf
    {
        public int Promotions { get; }
        public Promotion? Promotion { get; }
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
            Promotion = GetPromotionEnumFromPromotions(Promotions);
            Experience = experience;
        }

        // Methods

        private Promotion? GetPromotionEnumFromPromotions(int promotions)
        {
            // Handle no promotions safely
            if (promotions <= 0)
            {
                return null;
            }

            foreach (int promotionLevel in Enum.GetValues(typeof(Enums.Promotion)))
            {
                String promotionName = Enum.GetName(typeof(Enums.Promotion), promotionLevel);

                /*
                 * DRG promotions work in triplets, so checking if the promotion is at a specific level or the two
                 * previous levels will confirm the promotion's tier (Bronze, Silver, etc)
                 */
                if (promotions <= promotionLevel && promotions > promotionLevel - 3)
                {
                    return (Enums.Promotion)Enum.Parse(typeof(Enums.Promotion), promotionName);
                }
            }

            // Edge case handling for the (unlikely) future where more promotions could be added?
            return Enums.Promotion.Legendary;
        }

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
