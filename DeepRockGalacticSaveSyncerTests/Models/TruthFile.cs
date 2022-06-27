using DeepRockGalacticSaveSyncer.Enums;
using DeepRockGalacticSaveSyncer.Models;
using Newtonsoft.Json;

namespace DeepRockGalacticSaveSyncer.Tests.Models
{
    public class TruthFile
    {
        [JsonProperty(PropertyName = "metadata")]
        public Metadata? Metadata { get; set; }
        [JsonProperty(PropertyName = "dwarves")]
        public Dictionary<DwarfName, Dwarf>? Dwarves { get; set; }
    }
}
