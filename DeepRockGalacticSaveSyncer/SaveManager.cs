using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace DeepRockGalacticSaveSwapper
{
    [SupportedOSPlatform("windows10.0.17763.0")]
    internal abstract class SaveManager
    {
        public const string BACKUP_TEXT = ".backup";
        private const string TEMP_BACKUP_TEXT = ".backup.temp";

        public abstract FileSnapshot getNewestSaveFileSnapshot();

        private string? backupFileSnapshot(FileSnapshot fileSnapshot)
        {
            var backupPath = fileSnapshot.Path + BACKUP_TEXT;
            var tempBackupPath = fileSnapshot.Path + TEMP_BACKUP_TEXT;
            bool tempBackupCreated = false;

            // Move any preexisting backups, but don't delete them yet
            if (File.Exists(backupPath))
            {
                File.Move(backupPath, tempBackupPath);
                tempBackupCreated = true;
            }

            // todo: More clever backup management?
            File.Copy(fileSnapshot.Path, backupPath);

            if (tempBackupCreated)
            {
                return tempBackupPath;
            }
            else
            {
                return null;
            }
        }

        public virtual void overwriteNewestSaveFileData(FileSnapshot incomingFileSnapshot)
        {
            FileSnapshot existingSaveFile = getNewestSaveFileSnapshot();

            // Save a backup, just in case something goes wrong
            var tempBackupPath = backupFileSnapshot(existingSaveFile);

            // Copy the incoming file into the existing save file's location
            File.Delete(existingSaveFile.Path);
            File.Copy(incomingFileSnapshot.Path, existingSaveFile.Path);

            // Delete any temporary backups to keep the directory clean
            if (tempBackupPath != null)
            {
                File.Delete(tempBackupPath);
            }
        }
    }
}
