﻿using Core.SaveFiles.Models;
using GlobExpressions;
using System.Text.RegularExpressions;

namespace Core.SaveFiles.Manipulator
{
    public class XboxSaveFileManipulator : SaveFileManipulator
    {
        // todo: singleton?

        private string _SaveDirectoryPath;
        private Regex _SaveFileNameRegex;

        // Note: this is relative to the user's appdata\local directory
        private const string _XBOX_DRG_GLOBBED_SAVE_DIRECTORY_PATH = @"Packages\*DeepRockGalactic*\SystemAppData\wgs\*_*\*[!.]*";

        // Constructors

        public XboxSaveFileManipulator() : this(null) { }

        public XboxSaveFileManipulator(string? saveDirectoryPath)
        {
            _SaveFileNameRegex = new Regex("^[A-F0-9]+$");

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

        protected override Regex GetSaveFileNameRegex()
        {
            return _SaveFileNameRegex;
        }

        private string FindSaveDirectoryPathOnFileSystem()
        {
            // Perform the traversal, starting from the safe local AppData folder
            var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var saveDirectories = Glob.Directories(localAppDataPath, _XBOX_DRG_GLOBBED_SAVE_DIRECTORY_PATH).ToArray();

            if (saveDirectories.Length == 0)
            {
                throw new DirectoryNotFoundException("Unable to find Xbox save file directory.");
            }

            // The Glob.Directories doesn't prepend the root directory to its results, so do that before returning the result directory
            return Path.Combine(localAppDataPath, saveDirectories[0]);
        }

        public override XboxSaveFile GetNewestSaveFile()
        {
            // Get a list of file in the save directory
            List<string> files = Directory.EnumerateFiles(_SaveDirectoryPath).ToList().FindAll(path => IsValidSaveFilePath(path));

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

        public override string GetSaveFileDirectoryPath()
        {
            return _SaveDirectoryPath;
        }
    }
}
