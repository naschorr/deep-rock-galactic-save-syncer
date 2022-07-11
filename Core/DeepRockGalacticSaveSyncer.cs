using Core.SaveFiles.Manager;
using DeepRockGalacticSaveSyncer.Utilities;

namespace DeepRockGalacticSaveSyncer
{
    public class DeepRockGalacticSaveSyncer
    {
        public static void test(string[] arguments)
        {
            // todo: this config and save manager init is kind of ugly
            var kwargs = ArgumentProcessor.ProcessArguments(arguments);

            Dictionary<string, string> config;
            if (kwargs.ContainsKey("configPath"))
            {
                config = ConfigLoader.Load(kwargs["configPath"]);
            }
            else
            {
                config = new Dictionary<string, string>();
            }

            var saveFileManager = new SaveFileManager(config);
            var newestSteamSaveFileSnapshot = saveFileManager.SteamSaveFile;
            var newestXboxSaveFileSnapshot = saveFileManager.XboxSaveFile;

            // Is the latest Xbox (Windows) save newer?
            if (newestXboxSaveFileSnapshot > newestSteamSaveFileSnapshot)
            {
                saveFileManager.OverwriteSaveFile(newestXboxSaveFileSnapshot, newestSteamSaveFileSnapshot);
            }
            // Is the Steam save newer?
            else if (newestSteamSaveFileSnapshot > newestXboxSaveFileSnapshot)
            {
                saveFileManager.OverwriteSaveFile(newestSteamSaveFileSnapshot, newestXboxSaveFileSnapshot);
            }
        }
    }
}
