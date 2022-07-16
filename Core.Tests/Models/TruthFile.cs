using Core.Dwarves;
using Core.Enums;
using Newtonsoft.Json;

namespace DeepRockGalacticSaveSyncer.Tests.Models
{
    public class TruthFile
    {
        [JsonProperty(PropertyName = "metadata")]
        public Metadata? Metadata { get; set; }
        [JsonProperty(PropertyName = "dwarves")]
        public Dictionary<DwarfType, Dwarf>? Dwarves { get; set; }
    }
}
