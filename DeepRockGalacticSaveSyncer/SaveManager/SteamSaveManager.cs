using DeepRockGalacticSaveSyncer.Models;
using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using GlobExpressions;
using Microsoft.Win32;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace DeepRockGalacticSaveSyncer.SaveManager
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    internal class SteamSaveManager : SaveManager
    {
        private string _SaveDirectoryPath;
        private Regex _SaveFileRegex = new Regex("^[0-9]+_Player.sav$");

        private const string _STEAM_REGISTRY_PATH = @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam";
        private const string _LIBRARY_FOLDERS_VDF = "libraryfolders.vdf";
        private const string _DRG_APP_ID = "548430"; // DRG's app id on Steam
        private const string _STEAM_LIBRARY_DRG_SAVE_DIRECTORY_PATH = @"steamapps\common\Deep Rock Galactic\FSD\Saved\SaveGames";

        public SteamSaveManager()
        {
            var drgSteamLibraryPath = FindSteamLibraryContainingAppId(_DRG_APP_ID);
            _SaveDirectoryPath = FindSaveDirectoryPathOnFileSystem(drgSteamLibraryPath);
        }

        public SteamSaveManager(string saveDirectoryPath)
        {
            _SaveDirectoryPath = saveDirectoryPath;
        }

        private string FindSteamInstallPath()
        {
            var installPath = Registry.GetValue(_STEAM_REGISTRY_PATH, "SteamPath", null);

            if (installPath == null)
            {
                throw new IOException("Unable to retrieve Steam install location from the registry");
            }

            if (!Directory.Exists((string)installPath))
            {
                throw new IOException("Steam install location doesn't exist");
            }

            return (string)installPath;
        }

        private string FindSteamLibraryContainingAppId(string appId)
        {
            string libraryFoldersVdfPath = Path.Combine(FindSteamInstallPath(), "config", _LIBRARY_FOLDERS_VDF);
            dynamic libraryFolderJson = VdfConvert.Deserialize(File.ReadAllText(libraryFoldersVdfPath)).ToJson().Value;

            foreach (dynamic item in libraryFolderJson)
            {
                /*
                 * Ideally this would be an array.Exists, but the JSON produced by VdfConvert isn't very nice. This
                 * might just be my inexperience though.
                 */
                foreach (dynamic app in item.Value.apps)
                {
                    if (app.Name == appId)
                    {
                        return item.Value.path;
                    }
                }
            }

            // Otherwise something went wrong, and the whole thing is unrecoverable!
            throw new IOException($"Unable to find Steam library containing app id: {appId}");
        }

        private string FindSaveDirectoryPathOnFileSystem(string steamLibraryPath)
        {
            return Path.Combine(steamLibraryPath, _STEAM_LIBRARY_DRG_SAVE_DIRECTORY_PATH);
        }

        public override SaveFile GetNewestSaveFile()
        {
            var files = Glob.Files(_SaveDirectoryPath, "*_Player.sav").Select(name => Path.Combine(_SaveDirectoryPath, name)).ToList();

            // No files? Something went wrong!
            if (files.Count == 0)
            {
                throw new IOException($"Unable to find save file in directory {_SaveDirectoryPath}");
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
