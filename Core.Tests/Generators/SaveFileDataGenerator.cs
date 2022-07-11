using DeepRockGalacticSaveSyncer.Tests.Models;
using Newtonsoft.Json;

namespace DeepRockGalacticSaveSyncer.Tests.Generators
{
    public class SaveFileDataGenerator : TheoryData<SaveFileTruthPair>
    {
        private static readonly string _TEST_SAVE_FILES_DIR = @"..\..\..\TestData\SaveFiles";
        private static readonly string _SAVE_FILE_SUFFIX = ".player.sav";
        private static readonly string _TRUTH_FILE_SUFFIX = ".truth.json";

        public SaveFileDataGenerator()
        {
            // todo: Store List<SaveFileTruthPair> in a static, so it's not recomputed for every test case?
            foreach (var datum in WalkTestSaveFileData())
            {
                Add(datum);
            }
        }

        private List<SaveFileTruthPair> WalkTestSaveFileData()
        {
            var output = new List<SaveFileTruthPair>();

            foreach (var directory in Directory.GetDirectories(_TEST_SAVE_FILES_DIR))
            {
                // Note that Path.GetDirectoryName just gets the name of the parent, so GetFileName is accurate
                string directoryName = Path.GetFileName(directory);

                string? saveFilePath = null;
                string? truthFilePath = null;

                foreach (var file in Directory.GetFiles(directory))
                {
                    string fileName = Path.GetFileName(file);

                    if (fileName == $"{directoryName}{_SAVE_FILE_SUFFIX}")
                    {
                        saveFilePath = file;
                    }
                    else if (fileName == $"{directoryName}{_TRUTH_FILE_SUFFIX}")
                    {
                        truthFilePath = file;
                    }
                }

                if (saveFilePath != null && truthFilePath != null)
                {
                    using (StreamReader r = new StreamReader(truthFilePath))
                    {
                        string json = r.ReadToEnd();
                        TruthFile truthFile = JsonConvert.DeserializeObject<TruthFile>(json);

                        SaveFileTruthPair pair = new SaveFileTruthPair(saveFilePath, truthFile);

                        output.Add(pair);
                    }
                }
            }

            return output;
        }
    }
}
