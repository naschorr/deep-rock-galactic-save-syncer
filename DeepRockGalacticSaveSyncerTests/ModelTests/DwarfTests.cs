using DeepRockGalacticSaveSyncer.Models;

namespace DeepRockGalacticSaveSyncer.Tests.ModelTests
{
    public class DwarfTests
    {
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
