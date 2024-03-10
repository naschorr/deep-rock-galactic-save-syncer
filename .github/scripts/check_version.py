import sys
import re
from pathlib import Path
from dataclasses import dataclass


@dataclass
class SemanticVersion:
    major: int
    minor: int
    patch: int


    def __str__(self) -> str:
        return f"{self.major}.{self.minor}.{self.patch}"


    def __eq__(self, other: "SemanticVersion") -> bool: # type: ignore (dunno why it's complaining about the SemanticVersion param type)
        return self.major == other.major and self.minor == other.minor and self.patch == other.patch


    def __le__(self, other: "SemanticVersion") -> bool:
        if (self.major < other.major):
            return True
        elif (self.major > other.major):
            return False

        if (self.minor < other.minor):
            return True
        elif (self.minor > other.minor):
            return False

        return self.patch <= other.patch


class VersionChecker:
    CSPROJ_FILE_PATHS = [
        Path("GUI/GUI.csproj"),
        Path("Core/Core.csproj"),
    ]
    CSPROJ_VERSION_REGEX = re.compile(r"<Version>((?:\d\.){2}\d)<\/Version>", re.MULTILINE | re.IGNORECASE)

    ELECTRON_MANIFEST_PATH = Path("GUI/electron.manifest.json")
    ELECTRON_MANIFEST_VERSION_REGEX = re.compile(r'"buildVersion":\s?"((?:\d\.){2}\d)"', re.MULTILINE | re.IGNORECASE)


    def __init__(self, root_path: Path):
        self.root_path = root_path


    def _parse_semantic_version(self, version_string: str) -> SemanticVersion:
        ## Lazy preprocessing
        version_string = version_string.strip().removeprefix("v")

        ## My SemVers will always use three digits split by dots
        components = version_string.split(".")
        if (len(components) != 3):
            raise ValueError(f"Invalid version string {version_string}")
        components = [int(component) for component in components]

        return SemanticVersion(major=components[0], minor=components[1], patch=components[2])


    def _get_version_from_file_with_regex(self, file_path: Path, regex: re.Pattern) -> SemanticVersion:
        match = regex.search(file_path.read_text())
        if (match is None):
            raise ValueError(f"Failed to find version in {file_path}")
        else:
            return self._parse_semantic_version(match.group(1))


    def _get_csproj_version(self, file_path: Path) -> SemanticVersion:
        return self._get_version_from_file_with_regex(file_path, self.CSPROJ_VERSION_REGEX)


    def _get_electron_manifest_version(self, file_path: Path) -> SemanticVersion:
        return self._get_version_from_file_with_regex(file_path, self.ELECTRON_MANIFEST_VERSION_REGEX)

    
    def check_versions(self, latest_released_version: str) -> bool:
        path_version_map = {}

        ## Get the Electon Manifest version
        electron_manifest_path = self.root_path / self.ELECTRON_MANIFEST_PATH
        electron_manifest_version = self._get_electron_manifest_version(electron_manifest_path)
        if (electron_manifest_version is None):
            print(f"Failed to get version from {electron_manifest_path}")
            return False
        path_version_map[electron_manifest_path] = electron_manifest_version

        ## Get the csproj version
        for file_path in self.CSPROJ_FILE_PATHS:
            full_path = self.root_path / file_path
            csproj_version = self._get_csproj_version(full_path)
            if (csproj_version is None):
                print(f"Failed to get version from {full_path}")
                return False
            path_version_map[full_path] = csproj_version

        ## Check if all versions are the same
        for path, version in path_version_map.items():
            if version != electron_manifest_version:
                print(f"Version mismatch: {path} has version {version}, expected {electron_manifest_version}")
                return False

        ## Make sure the version is greater than the latest released version
        latest_released_semantic_version = self._parse_semantic_version(latest_released_version)
        if (electron_manifest_version <= latest_released_semantic_version):
            print(f"Version {electron_manifest_version} is not greater than latest released version {latest_released_semantic_version}")
            return False

        ## Provide some useful output in the log if things are successful
        print(f"Configured versions are all the same {electron_manifest_version}, and greater than the latest released version {latest_released_semantic_version}")
        for path, version in path_version_map.items():
            print(f"{path} has version {version}")

        return True


## Super lazy CLI
arguments = sys.argv[1:]
if (len(arguments) != 2):
    print("Usage: check_version.py <root_path> <latest_released_version>")
    sys.exit(1)

if (VersionChecker(Path(arguments[0])).check_versions(arguments[1])):
    sys.exit(0)

sys.exit(1)
