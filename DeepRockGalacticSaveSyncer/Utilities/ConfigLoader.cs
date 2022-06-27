﻿using Newtonsoft.Json;

namespace DeepRockGalacticSaveSyncer.Utilities
{
    internal class ConfigLoader
    {
        /*
         * Very simply string-string key value pairs, since a more complex schema doesn't make sense for the current
         * use case.
         */
        public static Dictionary<string, string> Load(string pathToConfigJson)
        {
            var text = File.ReadAllText(pathToConfigJson);

            return JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
        }
    }
}
