using Newtonsoft.Json.Linq;

namespace GUI.Data
{
    public class ConfigLoaderService
    {
        // todo: technically this is still unsafe, there really needs to be a dedicated query interface
        public dynamic? Config { get; private set; }

        public ConfigLoaderService()
        {
            // config.json is copied into wwwroot on build, see libman.json
            string configPath = Path.Combine(Environment.CurrentDirectory, "wwwroot" , "config.json");

            using (StreamReader reader = new StreamReader(configPath))
            {
                string json = reader.ReadToEnd();
                Config = JObject.Parse(json);
            }
        }
    }
}
