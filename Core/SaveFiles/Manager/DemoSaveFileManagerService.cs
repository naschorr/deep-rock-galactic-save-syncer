using Core.Dwarves;
using Core.Enums;
using Core.Models;
using Core.SaveFiles.Models;
using System.Reactive.Subjects;
using System.Runtime.Versioning;

namespace Core.SaveFiles.Manager
{
    [SupportedOSPlatform("windows")]
    /// <summary>
    /// This class provided a static and (mostly) consistent view of a before and after Steam and Xbox save file for
    /// presentation in demo mode. (GUI project's "DEMO" environment variable must equal "true" to enable this). The
    /// main goal is to make it easier to generate screenshots that illustrate how DRGSS works with a practical
    /// example.
    /// </summary>
    /// <seealso cref="ISaveFileManagerService"/>
    /// <seealso cref="CoreServiceExtensions"/>
    public class DemoSaveFileManagerService : ISaveFileManagerService
    {
        private bool _SaveFileLocked;

        public SteamSaveFile? SteamSaveFile
        {
            get
            {
                return GetSteamSaveFile();
            }
        }
        public XboxSaveFile? XboxSaveFile
        {
            get
            {
                return GetXboxSaveFile();
            }
        }
        public bool SaveFileLocked
        {
            get { return _SaveFileLocked; }
            set
            {
                _SaveFileLocked = value;
                SaveFileLockedChanged.OnNext(SaveFileLocked);
            }
        }

        public Subject<bool> SaveFileLockedChanged { get; set; } = new();

        // Constructors

        public DemoSaveFileManagerService()
        {
            _SaveFileLocked = false;
        }

        // Methods

        private SteamSaveFile GetSteamSaveFile()
        {
            String fileName = $"{String.Join("", Enumerable.Range(0, 17).Select(n => new Random().Next(10)))}_Player.sav";
            DateTime today = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                19,
                51,
                23,
                DateTimeKind.Local
            );

            ImmutableFile file = new ImmutableFile("this/isnt/a/real/path", fileName, today);

            var dwarves = new Dictionary<DwarfType, Dwarf>();
            dwarves.Add(DwarfType.Engineer, new Dwarf(5, 8742));
            dwarves.Add(DwarfType.Scout, new Dwarf(1, 194522));
            dwarves.Add(DwarfType.Driller, new Dwarf(3, 64128));
            dwarves.Add(DwarfType.Gunner, new Dwarf(7, 15837));

            return new SteamSaveFile(file, dwarves);
        }

        private XboxSaveFile GetXboxSaveFile()
        {
            DateTime lastMonth = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                22,
                16,
                08,
                DateTimeKind.Local
            ).AddDays(-37);

            ImmutableFile file = new ImmutableFile("neither/is/this/one", "636C65766572206769726C21203A29", lastMonth);

            var dwarves = new Dictionary<DwarfType, Dwarf>();
            dwarves.Add(DwarfType.Engineer, new Dwarf(5, 8742));
            dwarves.Add(DwarfType.Scout, new Dwarf(1, 175122));
            dwarves.Add(DwarfType.Driller, new Dwarf(2, 315000));
            dwarves.Add(DwarfType.Gunner, new Dwarf(6, 190451));

            return new XboxSaveFile(file, dwarves);
        }

        public void OverwriteSaveFile(SaveFile overwriter, SaveFile overwritee)
        {
            return;
        }
    }
}
