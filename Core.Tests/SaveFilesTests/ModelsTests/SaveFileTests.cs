using Core.Dwarves;
using Core.Enums;
using Core.Exceptions;
using Core.Models;
using Core.SaveFiles.Models;
using DeepRockGalacticSaveSyncer.Tests.Generators;
using DeepRockGalacticSaveSyncer.Tests.Models;

namespace Core.Tests.SaveFilesTests.ModelsTests
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

            var saveFile = new SteamSaveFile(pair.Path);

            Assert.NotNull(saveFile.Engineer);
            Assert.NotNull(saveFile.Scout);
            Assert.NotNull(saveFile.Driller);
            Assert.NotNull(saveFile.Gunner);
        }

        // Comparison Testing

        [Fact]
        public void SameSaveFileCompared()
        {
            var dwarves = new Dictionary<DwarfType, Dwarf>();
            dwarves.Add(DwarfType.Engineer, new Dwarf(1, 2500));
            dwarves.Add(DwarfType.Scout, new Dwarf(2, 80000));
            dwarves.Add(DwarfType.Driller, new Dwarf(0, 210000));
            dwarves.Add(DwarfType.Gunner, new Dwarf(1, 25000));

            var immutableFile = ImmutableFileGenerator();

            var dwarves0SaveFile = new SteamSaveFile(immutableFile, dwarves);
            var dwarves1SaveFile = new SteamSaveFile(immutableFile, dwarves);

            /*
             * Note that generating a new immutable file for each, and using the same dwarves would be inequal, as the
             * immutable files generated would have very slightly different creation times.
             */

            Assert.True(dwarves0SaveFile == dwarves1SaveFile);
        }

        [Fact]
        public void NullCompared()
        {
            var dwarves = new Dictionary<DwarfType, Dwarf>();
            dwarves.Add(DwarfType.Engineer, new Dwarf(1, 2500));
            dwarves.Add(DwarfType.Scout, new Dwarf(2, 80000));
            dwarves.Add(DwarfType.Driller, new Dwarf(0, 210000));
            dwarves.Add(DwarfType.Gunner, new Dwarf(1, 25000));

            var dwarvesSaveFile = new SteamSaveFile(ImmutableFileGenerator(), dwarves);

            Assert.False(dwarvesSaveFile == null);
            Assert.False(null == dwarvesSaveFile);
        }

        [Fact]
        public void InequalDwarvesCompared()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(3, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(0, 0));

            var highlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(highlyPromotedDwarfSaveFile != lowlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void AllHighlyPromotedDwarvesComparedToAllLowlyPromotedDwarves()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(3, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(0, 0));

            var highlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(highlyPromotedDwarfSaveFile > lowlyPromotedDwarfSaveFile);
            Assert.True(highlyPromotedDwarfSaveFile >= lowlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void AllLowPromotedDwarvesComparedToAllHighlyPromotedDwarves()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(3, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(0, 0));
            lowlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(0, 0));

            var highlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(lowlyPromotedDwarfSaveFile < highlyPromotedDwarfSaveFile);
            Assert.True(lowlyPromotedDwarfSaveFile <= highlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void AllSamePromotedSameExperienceDwarvesCompared()
        {
            // Slightly newer file
            var immutableFile0 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 12, 0, 0, DateTimeKind.Local));
            var dwarves0 = new Dictionary<DwarfType, Dwarf>();
            dwarves0.Add(DwarfType.Engineer, new Dwarf(2, 2000));
            dwarves0.Add(DwarfType.Scout, new Dwarf(1, 204500));
            dwarves0.Add(DwarfType.Driller, new Dwarf(0, 10150));
            dwarves0.Add(DwarfType.Gunner, new Dwarf(1, 97500));

            // Slightly older file
            var immutableFile1 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 10, 0, 0, DateTimeKind.Local));
            var dwarves1 = new Dictionary<DwarfType, Dwarf>();
            dwarves1.Add(DwarfType.Engineer, new Dwarf(2, 2000));
            dwarves1.Add(DwarfType.Scout, new Dwarf(1, 204500));
            dwarves1.Add(DwarfType.Driller, new Dwarf(0, 10150));
            dwarves1.Add(DwarfType.Gunner, new Dwarf(1, 97500));

            var dwarves0SaveFile = new SteamSaveFile(immutableFile0, dwarves0);
            var dwarves1SaveFile = new SteamSaveFile(immutableFile1, dwarves1);

            // With same promotions and experience, the slightly newer file will take precendence
            Assert.True(dwarves0SaveFile > dwarves1SaveFile);
            Assert.True(dwarves0SaveFile >= dwarves1SaveFile);
        }

        [Fact]
        public void MixedDwarvesWithPromotionsGreaterThanEqualToCompared()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(1, 0));
            highlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(2, 0));
            highlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 0));
            lowlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(1, 0));
            lowlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(1, 0));
            lowlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 0));

            var highlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(highlyPromotedDwarfSaveFile > lowlyPromotedDwarfSaveFile);
            Assert.True(highlyPromotedDwarfSaveFile >= lowlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void MixedDwarvesWithPromotionsLessThanEqualCompared()
        {
            var highlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(3, 0));
            highlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(1, 0));
            highlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(2, 0));
            highlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(2, 0));

            var lowlyPromotedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyPromotedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 0));
            lowlyPromotedDwarves.Add(DwarfType.Scout, new Dwarf(1, 0));
            lowlyPromotedDwarves.Add(DwarfType.Driller, new Dwarf(2, 0));
            lowlyPromotedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 0));

            var highlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyPromotedDwarves);
            var lowlyPromotedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyPromotedDwarves);

            Assert.True(lowlyPromotedDwarfSaveFile < highlyPromotedDwarfSaveFile);
            Assert.True(lowlyPromotedDwarfSaveFile <= highlyPromotedDwarfSaveFile);
        }

        [Fact]
        public void MixedPromotionDwarvesCompared()
        {
            // Slightly newer file
            var immutableFile0 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 12, 0, 0, DateTimeKind.Local));
            var dwarves0 = new Dictionary<DwarfType, Dwarf>();
            dwarves0.Add(DwarfType.Engineer, new Dwarf(2, 0));
            dwarves0.Add(DwarfType.Scout, new Dwarf(3, 0));
            dwarves0.Add(DwarfType.Driller, new Dwarf(1, 0));
            dwarves0.Add(DwarfType.Gunner, new Dwarf(2, 0));

            // Slightly older file
            var immutableFile1 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 10, 0, 0, DateTimeKind.Local));
            var dwarves1 = new Dictionary<DwarfType, Dwarf>();
            dwarves1.Add(DwarfType.Engineer, new Dwarf(2, 0));
            dwarves1.Add(DwarfType.Scout, new Dwarf(1, 0));
            dwarves1.Add(DwarfType.Driller, new Dwarf(3, 0));
            dwarves1.Add(DwarfType.Gunner, new Dwarf(1, 0));

            var dwarves0SaveFile = new SteamSaveFile(immutableFile0, dwarves0);
            var dwarves1SaveFile = new SteamSaveFile(immutableFile1, dwarves1);

            Assert.Throws<DivergentSaveFileException>(() => dwarves0SaveFile.CompareTo(dwarves1SaveFile));
            Assert.Throws<DivergentSaveFileException>(() => dwarves1SaveFile.CompareTo(dwarves0SaveFile));
        }

        [Fact]
        public void SamePromotionGreaterThanExperienceDwarvesCompared()
        {
            var highlyExperiencedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyExperiencedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 2500));
            highlyExperiencedDwarves.Add(DwarfType.Scout, new Dwarf(1, 6025));
            highlyExperiencedDwarves.Add(DwarfType.Driller, new Dwarf(3, 5400));
            highlyExperiencedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 20150));

            var lowlyExperiencedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyExperiencedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 1500));
            lowlyExperiencedDwarves.Add(DwarfType.Scout, new Dwarf(1, 2600));
            lowlyExperiencedDwarves.Add(DwarfType.Driller, new Dwarf(3, 2400));
            lowlyExperiencedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 14150));

            var highlyExperiencedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyExperiencedDwarves);
            var lowlyExperiencedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyExperiencedDwarves);

            Assert.True(highlyExperiencedDwarfSaveFile > lowlyExperiencedDwarfSaveFile);
        }

        [Fact]
        public void SamePromotionGreaterThanEqualToExperienceDwarvesCompared()
        {
            var highlyExperiencedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyExperiencedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 2500));
            highlyExperiencedDwarves.Add(DwarfType.Scout, new Dwarf(1, 6025));
            highlyExperiencedDwarves.Add(DwarfType.Driller, new Dwarf(3, 5400));
            highlyExperiencedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 20150));

            var lowlyExperiencedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyExperiencedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 2500));
            lowlyExperiencedDwarves.Add(DwarfType.Scout, new Dwarf(1, 2600));
            lowlyExperiencedDwarves.Add(DwarfType.Driller, new Dwarf(3, 5400));
            lowlyExperiencedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 14150));

            var highlyExperiencedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyExperiencedDwarves);
            var lowlyExperiencedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyExperiencedDwarves);

            Assert.True(highlyExperiencedDwarfSaveFile >= lowlyExperiencedDwarfSaveFile);
        }

        [Fact]
        public void SamePromotionLessThanExperienceDwarvesCompared()
        {
            var highlyExperiencedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyExperiencedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 9500));
            highlyExperiencedDwarves.Add(DwarfType.Scout, new Dwarf(1, 6025));
            highlyExperiencedDwarves.Add(DwarfType.Driller, new Dwarf(3, 5400));
            highlyExperiencedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 20150));

            var lowlyExperiencedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyExperiencedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 2500));
            lowlyExperiencedDwarves.Add(DwarfType.Scout, new Dwarf(1, 2600));
            lowlyExperiencedDwarves.Add(DwarfType.Driller, new Dwarf(3, 4400));
            lowlyExperiencedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 14150));

            var highlyExperiencedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyExperiencedDwarves);
            var lowlyExperiencedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyExperiencedDwarves);

            Assert.True(lowlyExperiencedDwarfSaveFile < highlyExperiencedDwarfSaveFile);
        }

        [Fact]
        public void SamePromotionLessThanEqualToExperienceDwarvesCompared()
        {
            var highlyExperiencedDwarves = new Dictionary<DwarfType, Dwarf>();
            highlyExperiencedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 2500));
            highlyExperiencedDwarves.Add(DwarfType.Scout, new Dwarf(1, 6025));
            highlyExperiencedDwarves.Add(DwarfType.Driller, new Dwarf(3, 4400));
            highlyExperiencedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 20150));

            var lowlyExperiencedDwarves = new Dictionary<DwarfType, Dwarf>();
            lowlyExperiencedDwarves.Add(DwarfType.Engineer, new Dwarf(2, 2500));
            lowlyExperiencedDwarves.Add(DwarfType.Scout, new Dwarf(1, 2600));
            lowlyExperiencedDwarves.Add(DwarfType.Driller, new Dwarf(3, 4400));
            lowlyExperiencedDwarves.Add(DwarfType.Gunner, new Dwarf(1, 14150));

            var highlyExperiencedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), highlyExperiencedDwarves);
            var lowlyExperiencedDwarfSaveFile = new SteamSaveFile(ImmutableFileGenerator(), lowlyExperiencedDwarves);

            Assert.True(lowlyExperiencedDwarfSaveFile <= highlyExperiencedDwarfSaveFile);
        }

        [Fact]
        public void SamePromotionMixedExperienceDwarvesCompared()
        {
            // Slightly newer file
            var immutableFile0 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 12, 0, 0, DateTimeKind.Local));
            var dwarves0 = new Dictionary<DwarfType, Dwarf>();
            dwarves0.Add(DwarfType.Engineer, new Dwarf(2, 2500));
            dwarves0.Add(DwarfType.Scout, new Dwarf(3, 90000));
            dwarves0.Add(DwarfType.Driller, new Dwarf(1, 500));
            dwarves0.Add(DwarfType.Gunner, new Dwarf(2, 165000));

            // Slightly older file
            var immutableFile1 = ImmutableFileGenerator(null, null, new DateTime(2022, 6, 26, 10, 0, 0, DateTimeKind.Local));
            var dwarves1 = new Dictionary<DwarfType, Dwarf>();
            dwarves1.Add(DwarfType.Engineer, new Dwarf(2, 1000));
            dwarves1.Add(DwarfType.Scout, new Dwarf(3, 12500));
            dwarves1.Add(DwarfType.Driller, new Dwarf(1, 2000));
            dwarves1.Add(DwarfType.Gunner, new Dwarf(2, 160000));

            var dwarves0SaveFile = new SteamSaveFile(immutableFile0, dwarves0);
            var dwarves1SaveFile = new SteamSaveFile(immutableFile1, dwarves1);

            Assert.Throws<DivergentSaveFileException>(() => dwarves0SaveFile.CompareTo(dwarves1SaveFile));
            Assert.Throws<DivergentSaveFileException>(() => dwarves1SaveFile.CompareTo(dwarves0SaveFile));
        }
    }
}