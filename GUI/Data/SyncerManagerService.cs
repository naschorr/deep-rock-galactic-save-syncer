using Core.Enums;
using Core.Exceptions;
using Core.SaveFiles.Models;
using Microsoft.AspNetCore.Components;
using System.Reactive.Subjects;

namespace GUI.Data
{
    public class SyncerManagerService
    {
        [Inject]
        private ILogger<SyncerManagerService> _Logger { get; set; }
        [Inject]
        private SaveFileManagerService _SaveFileManager { get; set; }

        private SteamSaveFile _SteamSaveFile;
        private XboxSaveFile _XboxSaveFile;
        private SaveFile? _Overwriter;
        private SaveFile? _Overwritee;

        public SteamSaveFile SteamSaveFile
        {
            get { return _SteamSaveFile; }
            set
            {
                _SteamSaveFile = value;
                SteamSaveFileChanged.OnNext(_SteamSaveFile);
            }
        }
        public XboxSaveFile XboxSaveFile
        {
            get { return _XboxSaveFile; }
            set
            {
                _XboxSaveFile = value;
                 XboxSaveFileChanged.OnNext(_XboxSaveFile);
            }
        }
        public SaveFile? Overwriter
        {
            get { return _Overwriter; }
            set
            {
                _Overwriter = value;
                 OverwriterChanged.OnNext(_Overwriter);
            }
        }
        public SaveFile? Overwritee
        {
            get { return _Overwritee; }
            set
            {
                _Overwritee = value;
                 OverwriteeChanged.OnNext(_Overwritee);
            }
        }

        public Subject<SteamSaveFile> SteamSaveFileChanged { get; private set; } = new();
        public Subject<XboxSaveFile> XboxSaveFileChanged { get; private set; } = new();
        public Subject<SaveFile?> OverwriterChanged { get; private set; } = new();
        public Subject<SaveFile?> OverwriteeChanged { get; private set; } = new();

        // Constructor

        public SyncerManagerService(ILogger<SyncerManagerService> logger, SaveFileManagerService saveFileManager)
        {
            _Logger = logger;
            _SaveFileManager = saveFileManager;

            _SteamSaveFile = _SaveFileManager.SteamSaveFile;
            _XboxSaveFile = _SaveFileManager.XboxSaveFile;

            CalculateOverwriterOverwritee();
        }

        // Methods

        public void Refresh()
        {
            SteamSaveFile = _SaveFileManager.SteamSaveFile;
            XboxSaveFile = _SaveFileManager.XboxSaveFile;

            CalculateOverwriterOverwritee();
        }

        private void CalculateOverwriterOverwritee()
        {
            try
            {
                if (SteamSaveFile > XboxSaveFile)
                {
                    Overwriter = SteamSaveFile;
                    Overwritee = XboxSaveFile;

                    return;
                }
                else if (XboxSaveFile > SteamSaveFile)
                {
                    Overwriter = XboxSaveFile;
                    Overwritee = SteamSaveFile;

                    return;
                }
            }
            catch (DivergentSaveFileException)
            {
                _Logger.LogInformation("Divergent Steam and Xbox saves detected.");
            }

            Overwriter = null;
            Overwritee = null;
        }

        public Platform GetSaveFilePlatform(SaveFile saveFile)
        {
            /*
             * Note: I've been running into a weird issue where the saveFile param can be of type SteamSaveFile and yet
             * ReferenceEquals(saveFile, SteamSaveFile) won't correctly determine the type. Similarly a ReferenceEquals
             * check for XboxSavFile will also fail. Thus this slightly more convoluted method
             */

            if (typeof(SteamSaveFile).IsAssignableFrom(saveFile.GetType()))
            {
                return Platform.Steam;
            }

            return Platform.Xbox;
        }

        public void OverwriteSaveFile()
        {
            if (Overwriter != null && Overwritee != null)
            {
                try
                {
                    _SaveFileManager.OverwriteSaveFile(Overwriter, Overwritee);
                    // todo: success modal - on modal close, refresh the app
                }
                catch (Exception)
                {
                    // todo: failure modal
                }
            }
            else
            {
                // todo: failure modal
            }

            Refresh();
        }

        public void OpenSaveFileInExplorer(SaveFile saveFile)
        {
            ElectronNET.API.Electron.Shell.ShowItemInFolderAsync(saveFile.Path);
        }
    }
}
