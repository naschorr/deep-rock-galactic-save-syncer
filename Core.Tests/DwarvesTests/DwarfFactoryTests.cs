using Core.Dwarves;
using Core.Enums;
using Core.Models;
using DeepRockGalacticSaveSyncer.Tests.Generators;
using DeepRockGalacticSaveSyncer.Tests.Models;

namespace Core.Tests.DwarvesTests
{
    public class DwarfFactoryTests
    {
        [Theory]
        [ClassData(typeof(SaveFileDataGenerator))]
        public void CreateDwarvesFromSaveFileTest(SaveFileTruthPair pair)
        {
            // Note that we're not testing the efficacy of the DwarfFactory generating dwarves from a save file.

            var file = new ImmutableFile(pair.Path);
            var truthFile = pair.Truth;

            var dwarves = DwarfFactory.CreateDwarvesFromSaveFile(file);

            var engineer = dwarves[DwarfType.Engineer];
            var scout = dwarves[DwarfType.Scout];
            var gunner = dwarves[DwarfType.Gunner];
            var driller = dwarves[DwarfType.Driller];

            // Engineer checks
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Engineer].Promotions,
                engineer.Promotions
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Engineer].Promotion.ToString(),
                engineer.Promotion.ToString()
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Engineer].Experience,
                engineer.Experience
            );

            // Scout checks
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Scout].Promotions,
                scout.Promotions
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Scout].Promotion.ToString(),
                scout.Promotion.ToString()
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Scout].Experience,
                scout.Experience
            );

            // Gunner checks
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Gunner].Promotions,
                gunner.Promotions
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Gunner].Promotion.ToString(),
                gunner.Promotion.ToString()
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Gunner].Experience,
                gunner.Experience
            );

            // Driller checks
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Driller].Promotions,
                driller.Promotions
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Driller].Promotion.ToString(),
                driller.Promotion.ToString()
            );
            Assert.Equal(
                truthFile.Dwarves?[DwarfType.Driller].Experience,
                driller.Experience
            );
        }
    }
}
