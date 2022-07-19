using Core.Models;
using DeepRockGalacticSaveSyncer.Tests.Generators;
using DeepRockGalacticSaveSyncer.Tests.Models;

namespace DeepRockGalacticSaveSyncer.Tests.ModelsTests
{
    public class ImmutableFileTests
    {
        [Fact]
        public void ConstructorInvalidPathException()
        {
            Assert.Throws<FileNotFoundException>(() => new ImmutableFile(""));
        }

        [Theory]
        [ClassData(typeof(SaveFileDataGenerator))]
        public void ConstructorValidHasPath(SaveFileTruthPair pair)
        {
            var immutableFile = new ImmutableFile(pair.Path);

            Assert.Equal(pair.Path, immutableFile.Path);
        }

        [Theory]
        [ClassData(typeof(SaveFileDataGenerator))]
        public void ConstructorValidHasName(SaveFileTruthPair pair)
        {
            var immutableFileName = Path.GetFileName(pair.Path);
            var immutableFile = new ImmutableFile(pair.Path);

            Assert.Equal(immutableFileName, immutableFile.Name);
        }
    }
}
