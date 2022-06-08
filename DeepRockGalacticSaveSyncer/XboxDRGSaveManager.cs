using GlobExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeepRockGalacticSaveSwapper
{
    [SupportedOSPlatform("windows")]
    internal class XboxDRGSaveManager : SaveManager
    {
        private string _saveDirectoryPath;
        private Regex _saveFileRegex = new Regex("^[A-F0-9]+$");

        const string XBOX_DRG_GLOBBED_SAVE_DIRECTORY_PATH = @"Packages\*DeepRockGalactic*\SystemAppData\wgs\*_*\*[!.]*";

        public XboxDRGSaveManager()
        {
            _saveDirectoryPath = findSaveDirectoryPathOnFileSystem();
        }

        public XboxDRGSaveManager(string saveDirectoryPath)
        {
            _saveDirectoryPath = saveDirectoryPath;
        }

        private string findSaveDirectoryPathOnFileSystem()
        {
            // Perform the traversal, starting from the safe local AppData folder
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var saveDirectory = Glob.Directories(localAppDataPath, XBOX_DRG_GLOBBED_SAVE_DIRECTORY_PATH).ToArray()[0];

            // The Glob.Directories doesn't prepend the root directory to its results, so do that before returning the result directory
            return Path.Combine(localAppDataPath, saveDirectory);
        }

        private bool filterCandidateSaveFilePath(string path)
        {
            FileInfo file = new FileInfo(path);

            return _saveFileRegex.IsMatch(file.Name);
        }

        public override FileSnapshot getNewestSaveFileSnapshot()
        {
            // Get a list of file in the save directory
            List<string> files = Directory.EnumerateFiles(_saveDirectoryPath).ToList().FindAll(path => filterCandidateSaveFilePath(path));

            // No files? Something went wrong!
            if (files.Count == 0)
            {
                throw new IOException($"Unable to find save file in directory {_saveDirectoryPath}");
            }

            // Find the newest save, and return some meta data about it
            var newestSaveFileSnapshot = new FileSnapshot(files[0]);
            foreach (string file in files.Skip(1))
            {
                var fileSnapshot = new FileSnapshot(file);

                if (fileSnapshot > newestSaveFileSnapshot)
                {
                    newestSaveFileSnapshot = fileSnapshot;
                }
            }

            return newestSaveFileSnapshot;
        }
    }
}
