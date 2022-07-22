using Core.Models;

namespace Core.Tests.ModelsTests
{
    public class SemanticVersionTests
    {
        // Construction Tests

        [Fact]
        public void SemanticVersion_SimpleVersionString_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;

            var semanticVersion = new SemanticVersion($"{major}.{minor}.{patch}");

            Assert.Equal(major, semanticVersion.Major);
            Assert.Equal(minor, semanticVersion.Minor);
            Assert.Equal(patch, semanticVersion.Patch);
            Assert.Null(semanticVersion.Extension);
        }

        [Fact]
        public void SemanticVersion_VersionStringPrependV_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;

            var semanticVersion = new SemanticVersion($"v{major}.{minor}.{patch}");

            Assert.Equal(major, semanticVersion.Major);
            Assert.Equal(minor, semanticVersion.Minor);
            Assert.Equal(patch, semanticVersion.Patch);
            Assert.Null(semanticVersion.Extension);
        }

        [Fact]
        public void SemanticVersion_VersionStringPrependString_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;

            var semanticVersion = new SemanticVersion($"version: {major}.{minor}.{patch}");

            Assert.Equal(major, semanticVersion.Major);
            Assert.Equal(minor, semanticVersion.Minor);
            Assert.Equal(patch, semanticVersion.Patch);
            Assert.Null(semanticVersion.Extension);
        }

        [Fact]
        public void SemanticVersion_VersionStringPrependVSpace_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;

            var semanticVersion = new SemanticVersion($"v {major}.{minor}.{patch}");

            Assert.Equal(major, semanticVersion.Major);
            Assert.Equal(minor, semanticVersion.Minor);
            Assert.Equal(patch, semanticVersion.Patch);
            Assert.Null(semanticVersion.Extension);
        }

        [Fact]
        public void SemanticVersion_VersionWithReleaseExtension_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;
            var extension = "rc1";

            var semanticVersion = new SemanticVersion($"v{major}.{minor}.{patch}-{extension}");

            Assert.Equal(major, semanticVersion.Major);
            Assert.Equal(minor, semanticVersion.Minor);
            Assert.Equal(patch, semanticVersion.Patch);
            Assert.Equal(extension, semanticVersion.Extension);
        }

        [Fact]
        public void SemanticVersion_VersionWithReleaseExtensionNoDash_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;
            var extension = "rc1";

            var semanticVersion = new SemanticVersion($"{major}.{minor}.{patch}{extension}");

            Assert.Equal(major, semanticVersion.Major);
            Assert.Equal(minor, semanticVersion.Minor);
            Assert.Equal(patch, semanticVersion.Patch);
            Assert.Equal(extension, semanticVersion.Extension);
        }

        [Fact]
        public void SemanticVersion_VersionWithSurroundingWhitespace_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;

            var semanticVersion = new SemanticVersion($"     {major}.{minor}.{patch}   ");

            Assert.Equal(major, semanticVersion.Major);
            Assert.Equal(minor, semanticVersion.Minor);
            Assert.Equal(patch, semanticVersion.Patch);
            Assert.Null(semanticVersion.Extension);
        }

        [Fact]
        public void SemanticVersion_ToString_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;
            var extension = "alpha";

            var semanticVersion = new SemanticVersion($"{major}.{minor}.{patch}-{extension}");

            Assert.Equal($"{major}.{minor}.{patch}-{extension}", semanticVersion.ToString());
        }

        [Fact]
        public void SemanticVersion_MissingPatch_Test()
        {
            var major = 1;
            var minor = 0;

            Assert.Throws<ArgumentException>(() => new SemanticVersion($"v{major}.{minor}"));
        }

        [Fact]
        public void SemanticVersion_MissingPeriods_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = 0;

            Assert.Throws<ArgumentException>(() => new SemanticVersion($"{major} {minor} {patch}"));
        }

        [Fact]
        public void SemanticVersion_NonNumericMajor_Test()
        {
            var major = "one";
            var minor = 0;
            var patch = 0;

            Assert.Throws<ArgumentException>(() => new SemanticVersion($"{major}.{minor}.{patch}"));
        }

        [Fact]
        public void SemanticVersion_NonNumericPatch_Test()
        {
            var major = 1;
            var minor = 0;
            var patch = "zero";

            Assert.Throws<ArgumentException>(() => new SemanticVersion($"{major}.{minor}.{patch}"));
        }

        // Comparison Tests

        [Fact]
        public void SemanticVersion_MajorComparison_Test()
        {
            var semanticVersion0 = new SemanticVersion(1, 0, 0, null);
            var semanticVersion1 = new SemanticVersion(0, 0, 0, null);

            Assert.True(semanticVersion0 > semanticVersion1);
        }

        [Fact]
        public void SemanticVersion_MinorComparison_Test()
        {
            var semanticVersion0 = new SemanticVersion(1, 1, 0, null);
            var semanticVersion1 = new SemanticVersion(1, 0, 0, null);

            Assert.True(semanticVersion0 > semanticVersion1);
        }

        [Fact]
        public void SemanticVersion_PatchComparison_Test()
        {
            var semanticVersion0 = new SemanticVersion(1, 0, 1, null);
            var semanticVersion1 = new SemanticVersion(1, 0, 0, null);

            Assert.True(semanticVersion0 > semanticVersion1);
        }

        [Fact]
        public void SemanticVersion_ExtensionComparisonNullAndString_Test()
        {
            var semanticVersion0 = new SemanticVersion(1, 0, 0, null);
            var semanticVersion1 = new SemanticVersion(1, 0, 0, "alpha");

            Assert.True(semanticVersion0 > semanticVersion1);
        }

        [Fact]
        public void SemanticVersion_ExtensionComparisonNullAndNull_Test()
        {
            var semanticVersion0 = new SemanticVersion(1, 0, 0, null);
            var semanticVersion1 = new SemanticVersion(1, 0, 0, null);

            Assert.Equal(semanticVersion0, semanticVersion1);
        }

        [Fact]
        public void SemanticVersion_ExtensionComparisonStringAndNull_Test()
        {
            var semanticVersion0 = new SemanticVersion(1, 0, 0, "alpha");
            var semanticVersion1 = new SemanticVersion(1, 0, 0, null);

            Assert.True(semanticVersion0 < semanticVersion1);
        }

        [Fact]
        public void SemanticVersion_ExtensionComparisonStringAndString_Test()
        {
            // Granular extension comparisons aren't performed, just their existence or lack thereof

            var semanticVersion0 = new SemanticVersion(1, 0, 0, "rc1");
            var semanticVersion1 = new SemanticVersion(1, 0, 0, "rc2");

            Assert.Equal(semanticVersion0, semanticVersion1);
        }
    }
}
