using DeepRockGalacticSaveSyncer.DwarfManager;
using DeepRockGalacticSaveSyncer.Enums;
using DeepRockGalacticSaveSyncer.Models;
using DeepRockGalacticSaveSyncer.Tests.Generators;
using DeepRockGalacticSaveSyncer.Tests.Models;

namespace DeepRockGalacticSaveSyncer.Tests.DwarfManagerTests
{
    public class DwarfManagerTests
    {
        [Theory]
        [ClassData(typeof(SaveFileDataGenerator))]
        public void CreateDwarvesFromSaveFileTest(SaveFileTruthPair pair)
        {
            // Note that we're not testing the efficacy of the DwarfFactory generating dwarves from a save file.

            var file = new ImmutableFile(pair.Path);
            var truthFile = pair.Truth;

            var dwarves = DwarfFactory.CreateDwarvesFromSaveFile(file);

            var engineer = dwarves[DwarfName.Engineer];
            var scout = dwarves[DwarfName.Scout];
            var gunner = dwarves[DwarfName.Gunner];
            var driller = dwarves[DwarfName.Driller];

            // Engineer checks
            Assert.Equal(
                truthFile.Dwarves?[DwarfName.Engineer].Promotions,
                engineer.Promotions
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfName.Engineer].Experience,
                engineer.Experience
            );

            // Scout checks
            Assert.Equal(
                truthFile.Dwarves?[DwarfName.Scout].Promotions,
                scout.Promotions
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfName.Scout].Experience,
                scout.Experience
            );

            // Gunner checks
            Assert.Equal(
                truthFile.Dwarves?[DwarfName.Gunner].Promotions,
                gunner.Promotions
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfName.Gunner].Experience,
                gunner.Experience
            );

            // Driller checks
            Assert.Equal(
                truthFile.Dwarves?[DwarfName.Driller].Promotions,
                driller.Promotions
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfName.Driller].Experience,
                driller.Experience
            );
        }
    }
}
