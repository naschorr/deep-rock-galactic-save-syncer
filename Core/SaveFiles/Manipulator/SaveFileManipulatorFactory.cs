namespace Core.SaveFiles.Manipulator
{
    public class SaveFileManipulatorFactory
    {
        public static T Create<T>() where T : SaveFileManipulator, new()
        {
            return Create<T>(null);
        }

        public static T Create<T>(string? saveDirectory) where T : SaveFileManipulator, new()
        {
            if (saveDirectory == null)
            {
                return new T();
            }

            if (!Directory.Exists(saveDirectory))
            {
                throw new DirectoryNotFoundException($"Unable to locate {saveDirectory}");
            }

            /*
             * The language doesn't like generic constructors to be given arguments upon construction, which is fine.
             * However, there isn't really a way to set abstract or virtual constructors to ensure that constructors
             * with arguments would be fine! This line bypasses that issue nicely.
             * Thanks to: https://stackoverflow.com/a/5598999/1724602
             */
            var created = Activator.CreateInstance(typeof(T), saveDirectory) as T;
            if (created == null)
            {
                throw new Exception($"Unable to create {typeof(T)}.");
            }

            return created;
        }
    }
}
