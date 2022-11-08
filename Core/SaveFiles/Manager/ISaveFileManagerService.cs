using Core.SaveFiles.Models;
using System.Reactive.Subjects;

namespace Core.SaveFiles.Manager
{
    public interface ISaveFileManagerService
    {
        // Members

        public SteamSaveFile? SteamSaveFile { get; }
        public XboxSaveFile? XboxSaveFile { get; }
        public bool SaveFileLocked { get; set; }
        public Subject<bool> SaveFileLockedChanged { get; set; }

        // Methods

        public void OverwriteSaveFile(SaveFile overwriter, SaveFile overwritee);
    }
}
