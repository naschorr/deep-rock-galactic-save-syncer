namespace Core.Exceptions
{
    public class DivergentSaveFileException : Exception
    {
        public DivergentSaveFileException() { }

        public DivergentSaveFileException(string message) : base(message) { }
    }
}
