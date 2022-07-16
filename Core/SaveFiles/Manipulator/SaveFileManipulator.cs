using Core.SaveFiles.Models;

namespace Core.SaveFiles.Manipulator
{
    public abstract class SaveFileManipulator
    {
        public const string BACKUP_TEXT = ".backup";
        private const string _TEMP_BACKUP_TEXT = ".backup.temp";

        public abstract SaveFile GetNewestSaveFile();

        private string? BackupSaveFile(SaveFile saveFile)
        {
            var backupPath = saveFile.Path + BACKUP_TEXT;
            var tempBackupPath = saveFile.Path + _TEMP_BACKUP_TEXT;
            bool tempBackupCreated = false;

            // Move any preexisting backups, but don't delete them yet
            if (File.Exists(backupPath))
            {
                File.Move(backupPath, tempBackupPath);
                tempBackupCreated = true;
            }

            // todo: More clever backup management?
            File.Copy(saveFile.Path, backupPath);

            if (tempBackupCreated)
            {
                return tempBackupPath;
            }
            else
            {
                return null;
            }
        }

        public virtual void OverwriteNewestSaveFileData(SaveFile incomingSaveFile)
        {
            SaveFile existingSaveFile = GetNewestSaveFile();

            // Save a backup, just in case something goes wrong
            var tempBackupPath = BackupSaveFile(existingSaveFile);

            // Copy the incoming file into the existing save file's location
            File.Delete(existingSaveFile.Path);
            File.Copy(incomingSaveFile.Path, existingSaveFile.Path);

            // Delete any temporary backups to keep the directory clean
            if (tempBackupPath != null)
            {
                File.Delete(tempBackupPath);
            }
        }
    }
}
