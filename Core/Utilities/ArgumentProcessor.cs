using System.Text.RegularExpressions;

namespace DeepRockGalacticSaveSyncer.Utilities
{
    public static class ArgumentProcessor
    {
        private static readonly Regex _LAUNCH_OPTIONS_REGEX = new Regex("^--(.+)=(.+)$");

        public static Dictionary<string, string> ProcessArguments(string[] arguments)
        {
            var kwargs = new Dictionary<string, string>();

            foreach (var arg in arguments)
            {
                var matches = _LAUNCH_OPTIONS_REGEX.Match(arg);

                if (matches.Groups.Count != 3)
                {
                    continue;
                }

                var key = matches.Groups[1].Value;
                var value = matches.Groups[2].Value;

                kwargs.Add(key, value);
            }

            return kwargs;
        }
    }
}
