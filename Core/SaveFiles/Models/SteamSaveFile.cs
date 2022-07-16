using Core.Dwarves;
using Core.Enums;
using Core.Models;

namespace Core.SaveFiles.Models
{
    public class SteamSaveFile : SaveFile
    {
        public SteamSaveFile(string path) : base(path) { }

        internal SteamSaveFile(ImmutableFile file, Dictionary<DwarfType, Dwarf> dwarves) : base(file, dwarves) { }
    }
}
