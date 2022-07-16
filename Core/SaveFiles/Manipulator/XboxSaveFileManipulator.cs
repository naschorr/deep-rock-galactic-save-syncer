using Core.SaveFiles.Models;
using GlobExpressions;
using System.Text.RegularExpressions;

namespace Core.SaveFiles.Manipulator
{
    public class XboxSaveFileManipulator : SaveFileManipulator
    {
        // todo: singleton?

        private string _SaveDirectoryPath;
        private Regex _SaveFileRegex = new Regex("^[A-F0-9]+$");

        // Note: this is relative to the user's appdata\local directory
        private const string _XBOX_DRG_GLOBBED_SAVE_DIRECTORY_PATH = @"Packages\*DeepRockGalactic*\SystemAppData\wgs\*_*\*[!.]*";

        // Constructors

        public XboxSaveFileManipulator() : this(null) { }

        public XboxSaveFileManipulator(string? saveDirectoryPath)
        {
            if (saveDirectoryPath == null)
            {
                _SaveDirectoryPath = FindSaveDirectoryPathOnFileSystem();
            }
            else
            {
                _SaveDirectoryPath = saveDirectoryPath;
            }
        }

        // Methods

        private string FindSaveDirectoryPathOnFileSystem()
        {
            // Perform the traversal, starting from the safe local AppData folder
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var saveDirectory = Glob.Directories(localAppDataPath, _XBOX_DRG_GLOBBED_SAVE_DIRECTORY_PATH).ToArray()[0];

            // The Glob.Directories doesn't prepend the root directory to its results, so do that before returning the result directory
            return Path.Combine(localAppDataPath, saveDirectory);
        }

        private bool FilterCandidateSaveFilePath(string path)
        {
            FileInfo file = new FileInfo(path);

            return _SaveFileRegex.IsMatch(file.Name);
        }

        public override XboxSaveFile GetNewestSaveFile()
        {
            // Get a list of file in the save directory
            List<string> files = Directory.EnumerateFiles(_SaveDirectoryPath).ToList().FindAll(path => FilterCandidateSaveFilePath(path));

            // No files? Something went wrong!
            if (files.Count == 0)
            {
                throw new IOException($"Unable to find save file in directory {_SaveDirectoryPath}");
            }

            // Find the newest save, and return some meta data about it
            var newestSaveFile = new XboxSaveFile(files[0]);
            foreach (string file in files.Skip(1))
            {
                var saveFile = new XboxSaveFile(file);

                if (saveFile > newestSaveFile)
                {
                    newestSaveFile = saveFile;
                }
            }

            return newestSaveFile;
        }
    }
}
