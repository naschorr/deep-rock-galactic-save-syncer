namespace DeepRockGalacticSaveSyncer.Tests.Models
{
    public struct SaveFileTruthPair
    {
        public string Path { get; }
        public TruthFile Truth { get; }

        public SaveFileTruthPair(string saveFilePath, TruthFile truth)
        {
            Path = saveFilePath;
            Truth = truth;
        }
    }
}
