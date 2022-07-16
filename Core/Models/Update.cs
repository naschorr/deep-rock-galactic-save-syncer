namespace Core.Models
{
    public class Update : SemanticVersion
    {
        public readonly Uri UpdateLink;

        public Update(int major, int minor, int patch, string? extension, Uri updateLink) : base(major, minor, patch, extension)
        {
            UpdateLink = updateLink;
        }

        public Update(string version, string updateLink) : base(version)
        {
            UpdateLink = new Uri(updateLink);
        }
    }
}
