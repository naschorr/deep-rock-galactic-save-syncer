using Microsoft.Toolkit.Uwp.Notifications;
using System.Runtime.Versioning;

namespace DeepRockGalacticSaveSwapper
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    public class DeepRockGalacticSaveSyncer
    {
        public static void Main()
        {
            var xboxSaveManager = new XboxDRGSaveManager();
            var newestXboxSaveFileSnapshot = xboxSaveManager.getNewestSaveFileSnapshot();

            var steamSaveManager = new SteamDRGSaveManager();
            var newestSteamSaveFileSnapshot = steamSaveManager.getNewestSaveFileSnapshot();

            /*
             * Prepare notifications. Note that these only show up in the Action Center.
             * See this link for more info: https://stackoverflow.com/a/50977632/1724602
             */
            ToastNotificationManagerCompat.History.Clear();
            var notification = new ToastContentBuilder()
                .AddText("Deep Rock Galactic Save Syncer");

            // Is the latest Xbox (Windows) save newer?
            if (newestXboxSaveFileSnapshot > newestSteamSaveFileSnapshot)
            {
                steamSaveManager.overwriteNewestSaveFileData(newestXboxSaveFileSnapshot);
                
                notification
                    .AddText("Save file sync complete! Updated Steam save to use most recent Xbox (Windows) data.")
                    .Show();
            }
            // Is the Steam save newer?
            else if (newestSteamSaveFileSnapshot > newestXboxSaveFileSnapshot)
            {
                xboxSaveManager.overwriteNewestSaveFileData(newestSteamSaveFileSnapshot);
                
                notification
                    .AddText("Save file sync complete! Updated Xbox (Windows) save to use most recent Steam data.")
                    .Show();
            }
            else // Are the saves the same age?
            {
                notification
                    .AddText("All saves are already synced up.")
                    .Show();
            }

            // The toast system needs some extra time to get it to reliably pop up
            Thread.Sleep(500);
        }
    }
}
