using Core.Dwarves;
using Core.Enums;
using Core.Models;

namespace Core.SaveFiles.Models
{
    public class XboxSaveFile : SaveFile
    {
        public XboxSaveFile(string path) : base(path) { }

        internal XboxSaveFile(ImmutableFile file, Dictionary<DwarfType, Dwarf> dwarves) : base(file, dwarves) { }
    }
}
