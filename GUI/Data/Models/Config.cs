using System.Text.Json.Serialization;

namespace GUI.Data.Models
{
    public class Config
    {
        public string? RepoUrl { get; }
        public string? WikiUrl { get; }
        public string? UpdateCheckUrl { get; }
        public string? ExitSteamExampleUrl { get; }
        public int? OverwriteFileRefreshIgnoreLockChangesMilliseconds { get; }

        // Give the deserializer a constructor to work with, otherwise it'll null all the read only values
        [JsonConstructor]
        public Config(
            string? repoUrl,
            string? wikiUrl,
            string? updateCheckUrl,
            string? exitSteamExampleUrl,
            int? overwriteFileRefreshIgnoreLockChangesMilliseconds
        )
        {
            RepoUrl = repoUrl;
            WikiUrl = wikiUrl;
            UpdateCheckUrl = updateCheckUrl;
            ExitSteamExampleUrl = exitSteamExampleUrl;
            OverwriteFileRefreshIgnoreLockChangesMilliseconds = overwriteFileRefreshIgnoreLockChangesMilliseconds;
        }

        // Fallback null member constructor
        public Config() { }
    }
}
