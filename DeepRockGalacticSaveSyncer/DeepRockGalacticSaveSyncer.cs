using System.Runtime.Versioning;

namespace DeepRockGalacticSaveSwapper
{
    [SupportedOSPlatform("windows")]
    public class DeepRockGalacticSaveSyncer
    {
        public static void Main()
        {
            var xboxSaveManager = new XboxDRGSaveManager();
            var newestXboxSaveFileSnapshot = xboxSaveManager.getNewestSaveFileSnapshot();

            var steamSaveManager = new SteamDRGSaveManager();
            var newestSteamSaveFileSnapshot = steamSaveManager.getNewestSaveFileSnapshot();

            // Is the latest Xbox (Windows) save newer?
            if (newestXboxSaveFileSnapshot > newestSteamSaveFileSnapshot)
            {
                Console.WriteLine($"Steam DRG save is older than Xbox (Windows) DRG save ({newestXboxSaveFileSnapshot.LastModifiedTime} > {newestSteamSaveFileSnapshot.LastModifiedTime}).");
                steamSaveManager.overwriteNewestSaveFileData(newestXboxSaveFileSnapshot);
                Console.WriteLine("Overwrote (and backed up) Steam DRG save with Xbox (Windows) DRG save.");
            }
            // Is the Steam save newer?
            else if (newestSteamSaveFileSnapshot > newestXboxSaveFileSnapshot)
            {
                Console.WriteLine($"Xbox (Windows) DRG save is older than Steam DRG save ({newestXboxSaveFileSnapshot.LastModifiedTime} > {newestSteamSaveFileSnapshot.LastModifiedTime}).");
                xboxSaveManager.overwriteNewestSaveFileData(newestSteamSaveFileSnapshot);
                Console.WriteLine("Overwrote (and backed up) Xbox DRG save with Steam DRG save.");
            }
            else // Are the saves the same age?
            {
                Console.WriteLine("Both the Steam and Xbox (Windows) Deep Rock Galactic saves are up to date!");
                return;
            }
        }
    }
}
