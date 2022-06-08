using Gameloop.Vdf;
using Gameloop.Vdf.JsonConverter;
using GlobExpressions;
using Microsoft.Win32;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace DeepRockGalacticSaveSwapper
{
    [SupportedOSPlatform("windows")]
    internal class SteamDRGSaveManager : SaveManager
    {
        private string _saveDirectoryPath;
        private Regex _saveFileRegex = new Regex("^[0-9]+_Player.sav$");

        const string STEAM_REGISTRY_PATH = @"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam";
        const string LIBRARY_FOLDERS_VDF = "libraryfolders.vdf";
        const string DRG_APP_ID = "548430"; // DRG's app id on Steam
        const string STEAM_LIBRARY_DRG_SAVE_DIRECTORY_PATH = @"steamapps\common\Deep Rock Galactic\FSD\Saved\SaveGames";

        public SteamDRGSaveManager()
        {
            var drgSteamLibraryPath = findSteamLibraryContainingAppId(DRG_APP_ID);
            _saveDirectoryPath = findSaveDirectoryPathOnFileSystem(drgSteamLibraryPath);
        }

        public SteamDRGSaveManager(string saveDirectoryPath)
        {
            _saveDirectoryPath = saveDirectoryPath;
        }

        private string findSteamInstallPath()
        {
            var installPath = Registry.GetValue(STEAM_REGISTRY_PATH, "SteamPath", null);

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

        private string findSteamLibraryContainingAppId(string appId)
        {
            String libraryFoldersVdfPath = Path.Combine(findSteamInstallPath(), "config", LIBRARY_FOLDERS_VDF);
            dynamic libraryFolderJson = VdfConvert.Deserialize(File.ReadAllText(libraryFoldersVdfPath)).ToJson().Value;

            foreach(dynamic item in libraryFolderJson)
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

        private string findSaveDirectoryPathOnFileSystem(string steamLibraryPath)
        {
            return Path.Combine(steamLibraryPath, STEAM_LIBRARY_DRG_SAVE_DIRECTORY_PATH);
        }

        public override FileSnapshot getNewestSaveFileSnapshot()
        {
            var files = Glob.Files(_saveDirectoryPath, "*_Player.sav").Select(name => Path.Combine(_saveDirectoryPath, name)).ToList();

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
