using Core.SaveFiles.Manipulator;
using Core.SaveFiles.Models;

namespace GUI.Data
{
    public class SaveFileManagerService
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

        public SaveFileManagerService()
        {
            _SteamSaveFileManipulator = SaveFileManipulatorFactory.Create<SteamSaveFileManipulator>();
            _XboxSaveFileManipulator = SaveFileManipulatorFactory.Create<XboxSaveFileManipulator>();
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
                throw new IOException("Unable to overwrite save file.");
            }
        }
    }
}
