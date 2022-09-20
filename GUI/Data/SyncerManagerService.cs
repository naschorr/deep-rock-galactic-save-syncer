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

        private SteamSaveFile? _SteamSaveFile;
        private XboxSaveFile? _XboxSaveFile;
        private SaveFile? _Overwriter;
        private SaveFile? _Overwritee;

        public SteamSaveFile? SteamSaveFile
        {
            get { return _SteamSaveFile; }
            set
            {
                _SteamSaveFile = value;
                _Logger.LogDebug($"SteamSaveFile set to {_SteamSaveFile}.");
                SteamSaveFileChanged.OnNext(_SteamSaveFile);
            }
        }
        public XboxSaveFile? XboxSaveFile
        {
            get { return _XboxSaveFile; }
            set
            {
                _XboxSaveFile = value;
                _Logger.LogDebug($"XboxSaveFile set to {_XboxSaveFile}.");
                XboxSaveFileChanged.OnNext(_XboxSaveFile);
            }
        }
        public SaveFile? Overwriter
        {
            get { return _Overwriter; }
            set
            {
                _Overwriter = value;
                _Logger.LogDebug($"Overwriter set to {(_Overwriter == null ? "null" : _Overwriter)}.");
                OverwriterChanged.OnNext(_Overwriter);
            }
        }
        public SaveFile? Overwritee
        {
            get { return _Overwritee; }
            set
            {
                _Overwritee = value;
                _Logger.LogDebug($"Overwritee set to {(_Overwritee == null ? "null" : _Overwritee)}.");
                OverwriteeChanged.OnNext(_Overwritee);
            }
        }

        public Subject<SteamSaveFile?> SteamSaveFileChanged { get; private set; } = new();
        public Subject<XboxSaveFile?> XboxSaveFileChanged { get; private set; } = new();
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
            _Logger.LogDebug("Refreshing save files");

            SteamSaveFile = _SaveFileManager.SteamSaveFile;
            XboxSaveFile = _SaveFileManager.XboxSaveFile;

            CalculateOverwriterOverwritee();
        }

        private void CalculateOverwriterOverwritee()
        {
            if (SteamSaveFile != null && XboxSaveFile != null)
            {
                try
                {
                    if (SteamSaveFile > XboxSaveFile)
                    {
                        _Logger.LogInformation("SteamSaveFile > XboxSaveFile");

                        Overwriter = SteamSaveFile;
                        Overwritee = XboxSaveFile;

                        return;
                    }
                    else if (XboxSaveFile > SteamSaveFile)
                    {
                        _Logger.LogInformation("XboxSaveFile > SteamSaveFile");

                        Overwriter = XboxSaveFile;
                        Overwritee = SteamSaveFile;

                        return;
                    }
                }
                catch (DivergentSaveFileException)
                {
                    _Logger.LogWarning("Divergent save files detected. No automatic way to determine precedence, user must intervene.");
                }
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
                    _Logger.LogInformation($"Syncing save files: Overwriting {Overwritee} with {Overwriter}.");
                    _SaveFileManager.OverwriteSaveFile(Overwriter, Overwritee);
                }
                catch (Exception e)
                {
                    _Logger.LogError($"Unable to overwrite save files!", e);
                    // todo: failure modal
                }
            }
            else
            {
                _Logger.LogCritical($"Unable to overwrite save files, one or both overwriter and overwritee are null. Overwriter: {Overwriter}, overwritee: {Overwritee}.");
                // todo: failure modal
            }

            Refresh();
        }

        public void OpenSaveFileInExplorer(SaveFile saveFile)
        {
            if (File.Exists(saveFile.Path))
            {
                _Logger.LogInformation($"Opening save file {saveFile.Path} in file explorer.");
                ElectronNET.API.Electron.Shell.ShowItemInFolderAsync(saveFile.Path);
            }
            else
            {
                var parent = Directory.GetParent(saveFile.Path);
                if (parent != null)
                {
                    _Logger.LogInformation($"Unable to open save file in explorer, opening save directory {parent.FullName} instead.");

                    // Use Window's URL handling to force it to open a directory
                    ElectronNET.API.Electron.Shell.OpenExternalAsync($"file:///{parent.FullName}");
                }
                else
                {
                    _Logger.LogError($"Unable to get parent of {saveFile.Path} to open in file explorer.");
                }
            }
        }
    }
}
