using Newtonsoft.Json;

namespace DeepRockGalacticSaveSyncer.Tests.Models
{
    public class Metadata
    {
        [JsonProperty(PropertyName = "lastModifiedTimeIso")]
        public DateTime LastModifiedTime { get; set; }
    }
}
