using Core.SaveFiles.Manipulator;
using Core.SaveFiles.Models;

namespace Core.SaveFiles.Manager
{
    public class SaveFileManager
    {
        private SteamSaveFileManipulator _SteamSaveFileManipulator;
        private XboxSaveFileManipulator _XboxSaveFileManipulator;

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

        // Constructors

        public SaveFileManager() : this(null) { }

        public SaveFileManager(Dictionary<string, string>? config)
        {
            if (config == null)
            {
                config = new Dictionary<string, string>();
            }

            _SteamSaveFileManipulator = SaveFileManipulatorFactory.Create<SteamSaveFileManipulator>(config.GetValueOrDefault("steamSavesDir"));
            _XboxSaveFileManipulator = SaveFileManipulatorFactory.Create<XboxSaveFileManipulator>(config.GetValueOrDefault("xboxSavesDir"));
        }

        // Methods

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
                throw new ArgumentException("Unable to overwrite save file.");
            }
        }
    }
}
