using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;

namespace DeepRockGalacticSaveSyncer.SaveManager
{
    internal class SaveManagerFactory
    {
        public static T Create<T>(string? saveDirectory) where T : SaveManager, new()
        {
            if (saveDirectory == null)
            {
                return new T();
            }

            if (!Directory.Exists(saveDirectory))
            {
                new ToastContentBuilder()
                    .AddText("Deep Rock Galactic Save Syncer")
                    .AddText($"Unable to locate save directory at: {saveDirectory}.")
                    .Show();

                Thread.Sleep(500);
            }

            /*
             * The language doesn't like generic constructors to be given arguments upon construction, which is fine.
             * However, there isn't really a way to set abstract or virtual constructors to ensure that constructors
             * with arguments would be fine! This line bypasses that issue nicely.
             * Thanks to: https://stackoverflow.com/a/5598999/1724602
             */
            var created = Activator.CreateInstance(typeof(T), saveDirectory) as T;
            if (created == null)
            {
                throw new Exception($"Unable to create {typeof(T)}.");
            }

            return created;
        }
    }
}
