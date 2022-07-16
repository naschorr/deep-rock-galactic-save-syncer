using Core.Dwarves;

namespace Core.Tests.DwarvesTests
{
    public class DwarfTests
    {
        // Method Tests

        [Fact]
        public void SameDwarfCompared()
        {
            var dwarf = new Dwarf(0, 0);

            Assert.True(dwarf == dwarf);
        }

        [Fact]
        public void SameDwarvesCompared()
        {
            var dwarf0 = new Dwarf(0, 0);
            var dwarf1 = new Dwarf(0, 0);

            Assert.True(dwarf0 == dwarf1);
        }

        [Fact]
        public void NullCompared()
        {
            var dwarf = new Dwarf(0, 0);

            Assert.False(dwarf == null);
            Assert.False(null == dwarf);
        }

        [Fact]
        public void InequalDwarvesCompared()
        {
            var dwarf0 = new Dwarf(0, 0);
            var dwarf1 = new Dwarf(1, 2000);

            Assert.True(dwarf0 != dwarf1);
        }

        [Fact]
        public void Level0Dwarf()
        {
            var dwarf = new Dwarf(0, 0);

            Assert.Equal(1, dwarf.Level);
        }

        [Fact]
        public void Level4DwarfUpperEdge()
        {
            var dwarf = new Dwarf(0, 17999);

            Assert.Equal(4, dwarf.Level);
        }

        [Fact]
        public void Level5DwarfLowerEdge()
        {
            var dwarf = new Dwarf(0, 18000);

            Assert.Equal(5, dwarf.Level);
        }

        [Fact]
        public void Level5DwarfMiddle()
        {
            var dwarf = new Dwarf(0, 21000);

            Assert.Equal(5, dwarf.Level);
        }

        [Fact]
        public void Level5DwarfUpperEdge()
        {
            var dwarf = new Dwarf(0, 24999);

            Assert.Equal(5, dwarf.Level);
        }

        [Fact]
        public void Level13DwarfUpperEdge()
        {
            var dwarf = new Dwarf(0, 116999);

            Assert.Equal(13, dwarf.Level);
        }

        [Fact]
        public void Level14DwarfLowerEdge()
        {
            var dwarf = new Dwarf(0, 117000);

            Assert.Equal(14, dwarf.Level);
        }

        [Fact]
        public void Level19DwarfLowerEdge()
        {
            var dwarf = new Dwarf(0, 199500);

            Assert.Equal(19, dwarf.Level);
        }

        [Fact]
        public void Level19DwarfMiddle()
        {
            var dwarf = new Dwarf(0, 205000);

            Assert.Equal(19, dwarf.Level);
        }

        [Fact]
        public void Level19DwarfUpperEdge()
        {
            var dwarf = new Dwarf(0, 217499);

            Assert.Equal(19, dwarf.Level);
        }

        [Fact]
        public void Level25Dwarf()
        {
            var dwarf = new Dwarf(0, 315000);

            Assert.Equal(25, dwarf.Level);
        }

        [Fact]
        public void TooMuchExperience()
        {
            var dwarf = new Dwarf(0, 1000000);

            Assert.Equal(25, dwarf.Level);
        }

        // Comparison Tests

        [Fact]
        public void PromotedDwarfEqualToPromotedDwarf()
        {
            var dwarf0 = new Dwarf(1, 0);
            var dwarf1 = new Dwarf(1, 0);

            Assert.Equal(0, dwarf0.CompareTo(dwarf1));
        }

        [Fact]
        public void HighlyPromotedDwarfGreaterThanLowPromotedDwarf()
        {
            var dwarf0 = new Dwarf(2, 0);
            var dwarf1 = new Dwarf(0, 0);

            Assert.True(dwarf0 > dwarf1);
        }

        [Fact]
        public void HighlyPromotedDwarfGreaterThanEqualToLowPromotedDwarf()
        {
            var dwarf0 = new Dwarf(2, 0);
            var dwarf1 = new Dwarf(0, 0);

            Assert.True(dwarf0 >= dwarf1);
        }

        [Fact]
        public void LowPromotedDwarfLessThanHighlyPromotedDwarf()
        {
            var dwarf0 = new Dwarf(2, 0);
            var dwarf1 = new Dwarf(0, 0);

            Assert.True(dwarf1 < dwarf0);
        }

        [Fact]
        public void LowPromotedDwarfLessThanEqualToHighlyPromotedDwarf()
        {
            var dwarf0 = new Dwarf(2, 0);
            var dwarf1 = new Dwarf(0, 0);

            Assert.True(dwarf1 <= dwarf0);
        }

        [Fact]
        public void ExperiencedDwarfEqualToExperiencedDwarf()
        {
            var dwarf0 = new Dwarf(1, 2000);
            var dwarf1 = new Dwarf(1, 2000);

            Assert.Equal(0, dwarf0.CompareTo(dwarf1));
        }

        [Fact]
        public void HighlyExperiencedDwarfGreaterThanLowExperiencedDwarf()
        {
            var dwarf0 = new Dwarf(0, 5000);
            var dwarf1 = new Dwarf(0, 2000);

            Assert.True(dwarf0 > dwarf1);
        }

        [Fact]
        public void HighlyExperiencedDwarfGreaterThanEqualToLowExperiencedDwarf()
        {
            var dwarf0 = new Dwarf(0, 5000);
            var dwarf1 = new Dwarf(0, 2000);

            Assert.True(dwarf0 >= dwarf1);
        }

        [Fact]
        public void LowExperiencedDwarfLessThanHighlyExperiencedDwarf()
        {
            var dwarf0 = new Dwarf(0, 5000);
            var dwarf1 = new Dwarf(0, 2000);

            Assert.True(dwarf1 < dwarf0);
        }

        [Fact]
        public void LowExperiencedDwarfLessThanEqualToHighlyExperiencedDwarf()
        {
            var dwarf0 = new Dwarf(0, 5000);
            var dwarf1 = new Dwarf(0, 2000);

            Assert.True(dwarf1 <= dwarf0);
        }
    }
}
