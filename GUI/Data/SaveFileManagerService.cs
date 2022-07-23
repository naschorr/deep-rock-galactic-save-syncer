using Core.SaveFiles.Manager;
using Core.SaveFiles.Manipulator;
using Core.SaveFiles.Models;
using System.Reactive.Subjects;

namespace GUI.Data
{
    public class SaveFileManagerService
    {
        private SaveFileManager _SaveFileManager;
        private bool _SaveFileLocked;

        public SteamSaveFile? SteamSaveFile
        {
            get
            {
                return _SaveFileManager.SteamSaveFile;
            }
        }
        public XboxSaveFile? XboxSaveFile
        {
            get
            {
                return _SaveFileManager.XboxSaveFile;
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

        public SaveFileManagerService()
        {
            _SaveFileManager = new SaveFileManager();

            _SaveFileLocked = _SaveFileManager.SaveFileLocked;
            _SaveFileManager.SaveFileLockedChanged.Subscribe(
                locked => { SaveFileLocked = locked; }
            );
        }

        // Methods

        public void OverwriteSaveFile(SaveFile overwriter, SaveFile overwritee)
        {
            _SaveFileManager.OverwriteSaveFile(overwriter, overwritee);
        }
    }
}
