using DeepRockGalacticSaveSyncer.Enums;
using DeepRockGalacticSaveSyncer.Models;
using DeepRockGalacticSaveSyncer.Tests.Generators;
using Moq;

namespace DeepRockGalacticSaveSyncer.Tests.Models
{
    public class SaveFileTests
    {
        // Niche Helpers

        internal ImmutableFile ImmutableFileGenerator()
        {
            return ImmutableFileGenerator(null, null, null);
        }

        internal ImmutableFile ImmutableFileGenerator(string? path, string? name, DateTime? lastModifiedTime)
        {
            path = path != null ? path : @"path\to\fake\test\file";
            name = name != null ? name : "test";
            lastModifiedTime = lastModifiedTime != null ? lastModifiedTime : DateTime.UtcNow;

            return new ImmutableFile(path, name, (DateTime)lastModifiedTime);
        }

        // Construction Testing

        [Theory]
        [ClassData(typeof(SaveFileDataGenerator))]
        public void ConstructorValidHasDwarves(SaveFileTruthPair pair)
        {
            // Note that we're not testing the efficacy of the DwarfFactory generating dwarves from a save file.

            var saveFile = new SaveFile(pair.Path);

            Assert.NotNull(saveFile.Engineer);
            Assert.NotNull(saveFile.Scout);
            Assert.NotNull(saveFile.Driller);
            Assert.NotNull(saveFile.Gunner);
        }

        // Comparison Testing

        [Fact]
        public void AllHighlyPromotedDwarvesComparedToAllLowlyPromotedDwarves()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfName, Dwarf>();
            highlyPromotedDwarves.Add(DwarfName.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfName.Scout, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfName.Driller, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfName.Gunner, new Dwarf(3, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfName, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfName.Engineer, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfName.Scout, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfName.Driller, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfName.Gunner, new Dwarf(0, 0));

            var highlyPromotedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(highlyPromotedDwarfSaveFile > lowlyPromotedDwarfSaveFile);
            Assert.True(highlyPromotedDwarfSaveFile >= lowlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void AllLowPromotedDwarvesComparedToAllHighlyPromotedDwarves()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfName, Dwarf>();
            highlyPromotedDwarves.Add(DwarfName.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfName.Scout, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfName.Driller, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfName.Gunner, new Dwarf(3, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfName, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfName.Engineer, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfName.Scout, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfName.Driller, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfName.Gunner, new Dwarf(0, 0));

            var highlyPromotedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(lowlyPromotedDwarfSaveFile < highlyPromotedDwarfSaveFile);
            Assert.True(lowlyPromotedDwarfSaveFile <= highlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void AllSamePromotedSameExperienceDwarvesCompared()
        {
            // Slightly newer file
            var immutableFile0 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 12, 0, 0, DateTimeKind.Local));
            var dwarves0 = new Dictionary<DwarfName, Dwarf>();
            dwarves0.Add(DwarfName.Engineer, new Dwarf(2, 2000));
            dwarves0.Add(DwarfName.Scout, new Dwarf(1, 204500));
            dwarves0.Add(DwarfName.Driller, new Dwarf(0, 10150));
            dwarves0.Add(DwarfName.Gunner, new Dwarf(1, 97500));

            // Slightly older file
            var immutableFile1 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 10, 0, 0, DateTimeKind.Local));
            var dwarves1 = new Dictionary<DwarfName, Dwarf>();
            dwarves1.Add(DwarfName.Engineer, new Dwarf(2, 2000));
            dwarves1.Add(DwarfName.Scout, new Dwarf(1, 204500));
            dwarves1.Add(DwarfName.Driller, new Dwarf(0, 10150));
            dwarves1.Add(DwarfName.Gunner, new Dwarf(1, 97500));

            var dwarves0SaveFile = new SaveFile(immutableFile0, dwarves0);
            var dwarves1SaveFile = new SaveFile(immutableFile1, dwarves1);

            // With same promotions and experience, the slightly newer file will take precendence
            Assert.True(dwarves0SaveFile > dwarves1SaveFile);
            Assert.True(dwarves0SaveFile >= dwarves1SaveFile);
        }

        [Fact]
        public void MixedDwarvesWithPromotionsGreaterThanEqualToCompared()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfName, Dwarf>();
            highlyPromotedDwarves.Add(DwarfName.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfName.Scout, new Dwarf(1, 0));
            highlyPromotedDwarves.Add(DwarfName.Driller, new Dwarf(2, 0));
            highlyPromotedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfName, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 0));
            lowlyPromotedDwarves.Add(DwarfName.Scout, new Dwarf(1, 0));
            lowlyPromotedDwarves.Add(DwarfName.Driller, new Dwarf(1, 0));
            lowlyPromotedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 0));

            var highlyPromotedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(highlyPromotedDwarfSaveFile > lowlyPromotedDwarfSaveFile);
            Assert.True(highlyPromotedDwarfSaveFile >= lowlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void MixedDwarvesWithPromotionsLessThanEqualCompared()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfName, Dwarf>();
            highlyPromotedDwarves.Add(DwarfName.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfName.Scout, new Dwarf(1, 0));
            highlyPromotedDwarves.Add(DwarfName.Driller, new Dwarf(2, 0));
            highlyPromotedDwarves.Add(DwarfName.Gunner, new Dwarf(2, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfName, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 0));
            lowlyPromotedDwarves.Add(DwarfName.Scout, new Dwarf(1, 0));
            lowlyPromotedDwarves.Add(DwarfName.Driller, new Dwarf(2, 0));
            lowlyPromotedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 0));

            var highlyPromotedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(lowlyPromotedDwarfSaveFile < highlyPromotedDwarfSaveFile);
            Assert.True(lowlyPromotedDwarfSaveFile <= highlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void MixedPromotionDwarvesCompared()
        {
            // Slightly newer file
            var immutableFile0 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 12, 0, 0, DateTimeKind.Local));
            var dwarves0 = new Dictionary<DwarfName, Dwarf>();
            dwarves0.Add(DwarfName.Engineer, new Dwarf(2, 0));
            dwarves0.Add(DwarfName.Scout, new Dwarf(3, 0));
            dwarves0.Add(DwarfName.Driller, new Dwarf(1, 0));
            dwarves0.Add(DwarfName.Gunner, new Dwarf(2, 0));

            // Slightly older file
            var immutableFile1 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 10, 0, 0, DateTimeKind.Local));
            var dwarves1 = new Dictionary<DwarfName, Dwarf>();
            dwarves1.Add(DwarfName.Engineer, new Dwarf(2, 0));
            dwarves1.Add(DwarfName.Scout, new Dwarf(1, 0));
            dwarves1.Add(DwarfName.Driller, new Dwarf(3, 0));
            dwarves1.Add(DwarfName.Gunner, new Dwarf(1, 0));

            var dwarves0SaveFile = new SaveFile(immutableFile0, dwarves0);
            var dwarves1SaveFile = new SaveFile(immutableFile1, dwarves1);

            // Mixed promotions, the slightly newer file will take precendence
            Assert.True(dwarves0SaveFile > dwarves1SaveFile);
            Assert.True(dwarves0SaveFile >= dwarves1SaveFile);
        }

        [Fact]
        public void SamePromotionGreaterThanExperienceDwarvesCompared()
        {
            var highlyExperiencedDwarves = new Dictionary<DwarfName, Dwarf>();
            highlyExperiencedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 2500));
            highlyExperiencedDwarves.Add(DwarfName.Scout, new Dwarf(1, 6025));
            highlyExperiencedDwarves.Add(DwarfName.Driller, new Dwarf(3, 5400));
            highlyExperiencedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 20150));

            var lowlyExperiencedDwarves = new Dictionary<DwarfName, Dwarf>();
            lowlyExperiencedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 1500));
            lowlyExperiencedDwarves.Add(DwarfName.Scout, new Dwarf(1, 2600));
            lowlyExperiencedDwarves.Add(DwarfName.Driller, new Dwarf(3, 2400));
            lowlyExperiencedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 14150));

            var highlyExperiencedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), highlyExperiencedDwarves);
            var lowlyExperiencedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), lowlyExperiencedDwarves);

            Assert.True(highlyExperiencedDwarfSaveFile > lowlyExperiencedDwarfSaveFile);
        }

        [Fact]
        public void SamePromotionGreaterThanEqualToExperienceDwarvesCompared()
        {
            var highlyExperiencedDwarves = new Dictionary<DwarfName, Dwarf>();
            highlyExperiencedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 2500));
            highlyExperiencedDwarves.Add(DwarfName.Scout, new Dwarf(1, 6025));
            highlyExperiencedDwarves.Add(DwarfName.Driller, new Dwarf(3, 5400));
            highlyExperiencedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 20150));

            var lowlyExperiencedDwarves = new Dictionary<DwarfName, Dwarf>();
            lowlyExperiencedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 2500));
            lowlyExperiencedDwarves.Add(DwarfName.Scout, new Dwarf(1, 2600));
            lowlyExperiencedDwarves.Add(DwarfName.Driller, new Dwarf(3, 5400));
            lowlyExperiencedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 14150));

            var highlyExperiencedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), highlyExperiencedDwarves);
            var lowlyExperiencedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), lowlyExperiencedDwarves);

            Assert.True(highlyExperiencedDwarfSaveFile >= lowlyExperiencedDwarfSaveFile);
        }

        [Fact]
        public void SamePromotionLessThanExperienceDwarvesCompared()
        {
            var highlyExperiencedDwarves = new Dictionary<DwarfName, Dwarf>();
            highlyExperiencedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 9500));
            highlyExperiencedDwarves.Add(DwarfName.Scout, new Dwarf(1, 6025));
            highlyExperiencedDwarves.Add(DwarfName.Driller, new Dwarf(3, 5400));
            highlyExperiencedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 20150));

            var lowlyExperiencedDwarves = new Dictionary<DwarfName, Dwarf>();
            lowlyExperiencedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 2500));
            lowlyExperiencedDwarves.Add(DwarfName.Scout, new Dwarf(1, 2600));
            lowlyExperiencedDwarves.Add(DwarfName.Driller, new Dwarf(3, 4400));
            lowlyExperiencedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 14150));

            var highlyExperiencedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), highlyExperiencedDwarves);
            var lowlyExperiencedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), lowlyExperiencedDwarves);

            Assert.True(lowlyExperiencedDwarfSaveFile < highlyExperiencedDwarfSaveFile);
        }

        [Fact]
        public void SamePromotionLessThanEqualToExperienceDwarvesCompared()
        {
            var highlyExperiencedDwarves = new Dictionary<DwarfName, Dwarf>();
            highlyExperiencedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 2500));
            highlyExperiencedDwarves.Add(DwarfName.Scout, new Dwarf(1, 6025));
            highlyExperiencedDwarves.Add(DwarfName.Driller, new Dwarf(3, 4400));
            highlyExperiencedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 20150));

            var lowlyExperiencedDwarves = new Dictionary<DwarfName, Dwarf>();
            lowlyExperiencedDwarves.Add(DwarfName.Engineer, new Dwarf(2, 2500));
            lowlyExperiencedDwarves.Add(DwarfName.Scout, new Dwarf(1, 2600));
            lowlyExperiencedDwarves.Add(DwarfName.Driller, new Dwarf(3, 4400));
            lowlyExperiencedDwarves.Add(DwarfName.Gunner, new Dwarf(1, 14150));

            var highlyExperiencedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), highlyExperiencedDwarves);
            var lowlyExperiencedDwarfSaveFile = new SaveFile(ImmutableFileGenerator(), lowlyExperiencedDwarves);

            Assert.True(lowlyExperiencedDwarfSaveFile <= highlyExperiencedDwarfSaveFile);
        }

        [Fact]
        public void SamePromotionMixedExperienceDwarvesCompared()
        {
            // Slightly newer file
            var immutableFile0 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 12, 0, 0, DateTimeKind.Local));
            var dwarves0 = new Dictionary<DwarfName, Dwarf>();
            dwarves0.Add(DwarfName.Engineer, new Dwarf(2, 2500));
            dwarves0.Add(DwarfName.Scout, new Dwarf(3, 90000));
            dwarves0.Add(DwarfName.Driller, new Dwarf(1, 500));
            dwarves0.Add(DwarfName.Gunner, new Dwarf(2, 165000));

            // Slightly older file
            var immutableFile1 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 10, 0, 0, DateTimeKind.Local));
            var dwarves1 = new Dictionary<DwarfName, Dwarf>();
            dwarves1.Add(DwarfName.Engineer, new Dwarf(2, 1000));
            dwarves1.Add(DwarfName.Scout, new Dwarf(3, 12500));
            dwarves1.Add(DwarfName.Driller, new Dwarf(1, 2000));
            dwarves1.Add(DwarfName.Gunner, new Dwarf(2, 160000));

            var dwarves0SaveFile = new SaveFile(immutableFile0, dwarves0);
            var dwarves1SaveFile = new SaveFile(immutableFile1, dwarves1);

            // Mixed promotions, the slightly newer file will take precendence
            Assert.True(dwarves0SaveFile > dwarves1SaveFile);
            Assert.True(dwarves0SaveFile >= dwarves1SaveFile);
        }
    }
}