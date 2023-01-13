namespace GUI.Data.Models
{
    public class Config
    {
        public string? RepoUrl { get; set; }
        public string? WikiUrl { get; set; }
        public string? UpdateCheckUrl { get; set; }
        public string? ExitSteamExampleUrl { get; set; }
        public int? OverwriteFileRefreshIgnoreLockChangesMilliseconds { get; set; }
    }
}
