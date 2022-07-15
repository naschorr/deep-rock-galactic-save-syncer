using Core.Enums;
using Core.SaveFiles.Models;
using Microsoft.AspNetCore.Components;
using System.Reactive.Subjects;

namespace GUI.Data
{
    public class SyncerManagerService
    {
        [Inject]
        private SaveFileManagerService SaveFileManager { get; set; }

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

        public SyncerManagerService(SaveFileManagerService saveFileManager)
        {
            SaveFileManager = saveFileManager;

            _SteamSaveFile = SaveFileManager.SteamSaveFile;
            _XboxSaveFile = SaveFileManager.XboxSaveFile;

            CalculateOverwriterOverwritee();
        }

        // Methods

        public void Refresh()
        {
            SteamSaveFile = SaveFileManager.SteamSaveFile;
            XboxSaveFile = SaveFileManager.XboxSaveFile;

            CalculateOverwriterOverwritee();
        }

        private void CalculateOverwriterOverwritee()
        {
            if (SteamSaveFile > XboxSaveFile)
            {
                Overwriter = SteamSaveFile;
                Overwritee = XboxSaveFile;
            }
            else if (XboxSaveFile > SteamSaveFile)
            {
                Overwriter = XboxSaveFile;
                Overwritee = SteamSaveFile;
            }
            else
            {
                Overwriter = null;
                Overwritee = null;
            }
        }

        public Platform GetSaveFilePlatform(SaveFile saveFile)
        {
            if (ReferenceEquals(saveFile, SteamSaveFile))
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
                    SaveFileManager.OverwriteSaveFile(Overwriter, Overwritee);
                    // todo: success modal - on modal close, refresh the app
                }
                catch (Exception e)
                {
                    // todo: failure modal
                }
            }
            else
            {
                // todo: failure modal
            }
        }

        public void OpenSaveFileInExplorer(SaveFile saveFile)
        {
            ElectronNET.API.Electron.Shell.ShowItemInFolderAsync(saveFile.Path);
        }
    }
}
