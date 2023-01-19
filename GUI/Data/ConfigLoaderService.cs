using GUI.Data.Models;
using System.Text.Json;

namespace GUI.Data
{
    public class ConfigLoaderService
    {
        public readonly Config Config;

        public ConfigLoaderService()
        {
            // config.json is copied into wwwroot on build, see libman.json
            // todo: config.json path as environment variable?
            string configPath = Path.Combine(Environment.CurrentDirectory, "wwwroot" , "config.json");

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Read and deserialize the configuration file
            Config? deserializedConfig;
            using (StreamReader reader = new StreamReader(configPath))
            {
                string json = reader.ReadToEnd();
                deserializedConfig = JsonSerializer.Deserialize<Config>(json, serializerOptions);
            }

            // Ensure Config exists, even if the configuration wasn't loaded
            Config = deserializedConfig ?? new Config();
        }
    }
}
