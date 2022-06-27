using DeepRockGalacticSaveSyncer.Models;
using GlobExpressions;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace DeepRockGalacticSaveSyncer.SaveManager
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    internal class XboxSaveManager : SaveManager
    {
        private string _saveDirectoryPath;
        private Regex _saveFileRegex = new Regex("^[A-F0-9]+$");

        const string XBOX_DRG_GLOBBED_SAVE_DIRECTORY_PATH = @"Packages\*DeepRockGalactic*\SystemAppData\wgs\*_*\*[!.]*";

        public XboxSaveManager()
        {
            _saveDirectoryPath = FindSaveDirectoryPathOnFileSystem();
        }

        public XboxSaveManager(string saveDirectoryPath)
        {
            _saveDirectoryPath = saveDirectoryPath;
        }

        private string FindSaveDirectoryPathOnFileSystem()
        {
            // Perform the traversal, starting from the safe local AppData folder
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var saveDirectory = Glob.Directories(localAppDataPath, XBOX_DRG_GLOBBED_SAVE_DIRECTORY_PATH).ToArray()[0];

            // The Glob.Directories doesn't prepend the root directory to its results, so do that before returning the result directory
            return Path.Combine(localAppDataPath, saveDirectory);
        }

        private bool FilterCandidateSaveFilePath(string path)
        {
            FileInfo file = new FileInfo(path);

            return _saveFileRegex.IsMatch(file.Name);
        }

        public override SaveFile GetNewestSaveFile()
        {
            // Get a list of file in the save directory
            List<string> files = Directory.EnumerateFiles(_saveDirectoryPath).ToList().FindAll(path => FilterCandidateSaveFilePath(path));

            // No files? Something went wrong!
            if (files.Count == 0)
            {
                throw new IOException($"Unable to find save file in directory {_saveDirectoryPath}");
            }

            // Find the newest save, and return some meta data about it
            var newestSaveFile = new SaveFile(files[0]);
            foreach (string file in files.Skip(1))
            {
                var saveFile = new SaveFile(file);

                if (saveFile > newestSaveFile)
                {
                    newestSaveFile = saveFile;
                }
            }

            return newestSaveFile;
        }
    }
}
