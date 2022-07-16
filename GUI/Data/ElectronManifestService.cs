using Core.Models;
using Newtonsoft.Json.Linq;

namespace GUI.Data
{
    public class ElectronManifestService
    {
        private dynamic _Manifest;

        public SemanticVersion Version
        {
            get { return GetVersion(); }
        }

        public ElectronManifestService()
        {
            // the actual manifest json is copied into the root build folder
            string manifestPath = Path.Combine(Environment.CurrentDirectory, "electron.manifest.json");

            using (StreamReader reader = new StreamReader(manifestPath))
            {
                string json = reader.ReadToEnd();
                _Manifest = JObject.Parse(json);
            }
        }

        private SemanticVersion GetVersion()
        {
            return new SemanticVersion(_Manifest.build.buildVersion.ToString());
        }
    }
}
