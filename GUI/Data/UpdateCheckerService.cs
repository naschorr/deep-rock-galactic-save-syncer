using Core.Models;
using Newtonsoft.Json.Linq;

namespace GUI.Data
{
    public class UpdateCheckerService
    {
        private ConfigLoaderService ConfigLoader { get; set; } = default!;

        private ElectronManifestService ElectronManifest;
        private readonly string? _Url;
        private readonly SemanticVersion _CurrentVersion;

        public readonly Update? LatestVersion;

        // Constructor

        public UpdateCheckerService(ElectronManifestService electronManifestService, ConfigLoaderService configLoaderService)
        {
            ElectronManifest = electronManifestService;
            ConfigLoader = configLoaderService;

            _Url = ConfigLoader.Config?.updateCheckUrl;
            _CurrentVersion = ElectronManifest.Version;

            if (_Url != null)
            {
                LatestVersion = GetLatestReleaseVersion(_Url);
            }
        }

        // Methods

        private dynamic? GetReleases(string url)
        {
            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url)
                };

                request.Headers.UserAgent.ParseAdd($"drgss-{_CurrentVersion.ToString()}");
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github+json"));

                response = client.SendAsync(request).Result;
            }

            string? content = response.Content.ReadAsStringAsync().Result;
            if (response.IsSuccessStatusCode && content != null)
            {
                return JArray.Parse(content);
            }

            return null;
        }

        private Update? GetLatestReleaseVersion(string url)
        {
            dynamic? json = GetReleases(url);

            if (json == null)
            {
                return null;
            }

            // todo: Verify that Github returns these in chronologic order
            foreach (var item in json)
            {
                var isDraft = Boolean.Parse(item.draft.ToString());
                var isPrerelease = Boolean.Parse(item.prerelease.ToString());
                if (!isDraft && !isPrerelease)
                {
                    var releaseVersion = item.tag_name.ToString();
                    var releaseUrl = item.html_url.ToString();

                    return new Update(releaseVersion, releaseUrl);
                }
            }

            return null;
        }

        public bool IsNewReleaseAvailable()
        {
            if (LatestVersion != null)
            {
                return LatestVersion > _CurrentVersion;
            }

            return false;
        }
    }
}
