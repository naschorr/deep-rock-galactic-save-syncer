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
        private int _OverwriteFileRefreshIgnoreLockChangesMilliseconds;
        private DateTime _LastOverwriteDateTime;

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
        public Subject<List<SaveFile>> SyncedSaveFilesChanged { get; set; } = new();

        // Constructors

        public SaveFileManagerService(ConfigLoaderService configLoader)
        {
            var ConfigLoader = configLoader;

            _LastOverwriteDateTime = DateTime.UnixEpoch;
            _OverwriteFileRefreshIgnoreLockChangesMilliseconds = ConfigLoader.Config?.overwriteFileRefreshIgnoreLockChangesMilliseconds ?? 6000;

            _SaveFileManager = new SaveFileManager();
            _SaveFileLocked = _SaveFileManager.SaveFileLocked;
            _SaveFileManager.SaveFileLockedChanged.Subscribe(
                locked => {
                    TimeSpan TimeSinceLastOverwrite = DateTime.Now - _LastOverwriteDateTime;

                    /*
                     * When a user syncs their save files, one of those files will trigger a change in the file
                     * watcher. However, we don't need to update the UI as a result of this sync, so we can ignore
                     * any changes that happen within the configured dead zone.
                     */
                    if (TimeSinceLastOverwrite.TotalMilliseconds < _OverwriteFileRefreshIgnoreLockChangesMilliseconds)
                    {
                        /*
                         * In the unlikely event of a (genuine) file change during this period, make sure that doesn't
                         * get ignored.
                         * 
                         * This can be determined by comparing update times for the files, and if they're different
                         * then a real file change must've happened.
                         */
                        SteamSaveFile steamSaveFile = _SaveFileManager.SteamSaveFile;
                        XboxSaveFile xboxSaveFile = _SaveFileManager.XboxSaveFile;

                        if (steamSaveFile?.LastModifiedTime != xboxSaveFile?.LastModifiedTime)
                        {
                            SaveFileLocked = locked;
                        }
                    }
                    else
                    {
                        SaveFileLocked = locked;
                    }
                }
            );
        }

        // Methods

        public void OverwriteSaveFile(SaveFile overwriter, SaveFile overwritee)
        {
            _LastOverwriteDateTime = DateTime.Now;

            _SaveFileManager.OverwriteSaveFile(overwriter, overwritee);
            
            // Alert subscribers of the newly overwritten files
            SteamSaveFile SteamSaveFile = _SaveFileManager.SteamSaveFile;
            XboxSaveFile XboxSaveFile = _SaveFileManager.XboxSaveFile;
            SyncedSaveFilesChanged.OnNext(new List<SaveFile> { SteamSaveFile, XboxSaveFile });
        }
    }
}
