using Core.SaveFiles.Manipulator;
using Core.SaveFiles.Models;
using System.Reactive.Subjects;

namespace GUI.Data
{
    public class SaveFileManagerService
    {
        private SteamSaveFileManipulator _SteamSaveFileManipulator;
        private XboxSaveFileManipulator _XboxSaveFileManipulator;
        private bool _SaveFileLocked;
        private FileSystemWatcher _SteamSaveFileWatcher;
        private FileSystemWatcher _XboxSaveFileWatcher;

        public SteamSaveFile SteamSaveFile
        {
            get
            {
                return _SteamSaveFileManipulator.GetNewestSaveFile();
            }
        }
        public XboxSaveFile XboxSaveFile
        {
            get
            {
                return _XboxSaveFileManipulator.GetNewestSaveFile();
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
            _SteamSaveFileManipulator = SaveFileManipulatorFactory.Create<SteamSaveFileManipulator>();
            _XboxSaveFileManipulator = SaveFileManipulatorFactory.Create<XboxSaveFileManipulator>();

            _SaveFileLocked = IsFileLocked(SteamSaveFile.Path) || IsFileLocked(XboxSaveFile.Path);

            _SteamSaveFileWatcher = new FileSystemWatcher(_SteamSaveFileManipulator.GetSaveFileDirectoryPath());
            // The Steam save system just modifies the same file, and has separate backup files
            _SteamSaveFileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess;
            _SteamSaveFileWatcher.Changed += OnSaveFileChanged;
            _SteamSaveFileWatcher.EnableRaisingEvents = true;

            _XboxSaveFileWatcher = new FileSystemWatcher(_XboxSaveFileManipulator.GetSaveFileDirectoryPath());
            // The Xbox save system seems to write a new file on every change, and delete the old one at the same moment
            _XboxSaveFileWatcher.NotifyFilter = NotifyFilters.FileName;
            _XboxSaveFileWatcher.Changed += OnSaveFileChanged;
            _XboxSaveFileWatcher.Created += OnSaveFileChanged;
            _XboxSaveFileWatcher.Deleted += OnSaveFileChanged;
            _XboxSaveFileWatcher.EnableRaisingEvents = true;
        }

        // Methods

        private bool IsFileLocked(string path)
        {
            /*
             * Technically this isn't perfectly safe, but given that there should really only be DRG (and potentially
             * Steam/Xbox for uploading after a session) accessing the file, this should be more than fine.
             */

            var file = new FileInfo(path);

            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }

        private void OnSaveFileChanged(object sender, FileSystemEventArgs eventArgs)
        {
            // Ignore irrelevant file changes
            if (    !_SteamSaveFileManipulator.IsValidSaveFilePath(eventArgs.FullPath) &&
                    !_XboxSaveFileManipulator.IsValidSaveFilePath(eventArgs.FullPath)
            )
            {
                return;
            }

            // Deleted files clearly aren't locked.
            if (eventArgs.ChangeType == WatcherChangeTypes.Deleted)
            {
                SaveFileLocked = false;
                return;
            }

            var locked = IsFileLocked(eventArgs.FullPath);
            SaveFileLocked = locked;

            if (locked)
            {
                // Very lazy check to see if the file was only locked briefly. todo: make this less bad
                Thread.Sleep(5000);

                SaveFileLocked = IsFileLocked(eventArgs.FullPath);
            }
        }

        public void OverwriteSaveFile(SaveFile overwriter, SaveFile overwritee)
        {
            if (overwriter.GetType() == overwritee.GetType())
            {
                throw new ArgumentException("Unable to overwrite a save file with the same type.");
            }

            if (overwriter is SteamSaveFile && overwritee is XboxSaveFile)
            {
                _XboxSaveFileManipulator.OverwriteNewestSaveFileData(overwriter);
            }
            else if (overwriter is XboxSaveFile && overwritee is SteamSaveFile)
            {
                _SteamSaveFileManipulator.OverwriteNewestSaveFileData(overwriter);
            }
            else
            {
                throw new IOException("Unable to overwrite save file.");
            }
        }
    }
}
