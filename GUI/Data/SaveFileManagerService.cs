using Core.SaveFiles.Manager;
using Core.SaveFiles.Models;
using Microsoft.AspNetCore.Components;
using System.Reactive.Subjects;

namespace GUI.Data
{
    public class SaveFileManagerService
    {
        [Inject]
        private ILogger<SaveFileManagerService> _Logger { get; set; }
        [Inject]
        private ISaveFileManagerService _SaveFileManagerService { get; set; }

        private bool _SaveFileLocked;
        private int _OverwriteFileRefreshIgnoreLockChangesMilliseconds;
        private DateTime _LastOverwriteDateTime;

        public SteamSaveFile? SteamSaveFile
        {
            get
            {
                return _SaveFileManagerService.SteamSaveFile;
            }
        }
        public XboxSaveFile? XboxSaveFile
        {
            get
            {
                return _SaveFileManagerService.XboxSaveFile;
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

        public SaveFileManagerService(ILogger<SaveFileManagerService> logger, ISaveFileManagerService saveFileManagerService, ConfigLoaderService configLoader)
        {
            _Logger = logger;

            _LastOverwriteDateTime = DateTime.UnixEpoch;
            _OverwriteFileRefreshIgnoreLockChangesMilliseconds = configLoader.Config.OverwriteFileRefreshIgnoreLockChangesMilliseconds ?? 6000;

            _SaveFileManagerService = saveFileManagerService;
            _SaveFileLocked = _SaveFileManagerService.SaveFileLocked;
            _SaveFileManagerService.SaveFileLockedChanged.Subscribe(
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
                        SteamSaveFile steamSaveFile = _SaveFileManagerService.SteamSaveFile;
                        XboxSaveFile xboxSaveFile = _SaveFileManagerService.XboxSaveFile;

                        if (steamSaveFile?.LastModifiedTime != xboxSaveFile?.LastModifiedTime)
                        {
                            _Logger.LogInformation($"Save file lock state changed inside dead zone: Locked = {locked}");
                            SaveFileLocked = locked;
                        }
                    }
                    else
                    {
                        _Logger.LogInformation($"Save file lock state changed: Locked = {locked}");
                        SaveFileLocked = locked;
                    }
                }
            );
        }

        // Methods

        public void OverwriteSaveFile(SaveFile overwriter, SaveFile overwritee)
        {
            _LastOverwriteDateTime = DateTime.Now;

            _Logger.LogInformation($"Overwriting save file. {overwriter} will overwrite {overwritee}");
            _SaveFileManagerService.OverwriteSaveFile(overwriter, overwritee);

            // Alert subscribers of the newly overwritten files
            SteamSaveFile SteamSaveFile = _SaveFileManagerService.SteamSaveFile;
            XboxSaveFile XboxSaveFile = _SaveFileManagerService.XboxSaveFile;
            SyncedSaveFilesChanged.OnNext(new List<SaveFile> { SteamSaveFile, XboxSaveFile });
        }
    }
}
