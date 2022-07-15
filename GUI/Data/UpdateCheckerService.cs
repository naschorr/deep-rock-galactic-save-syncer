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
        private readonly SemanticVersion? _LatestVersion;

        // Constructor

        public UpdateCheckerService(ElectronManifestService electronManifestService, ConfigLoaderService configLoaderService)
        {
            ElectronManifest = electronManifestService;
            ConfigLoader = configLoaderService;

            _Url = ConfigLoader.Config?.updateCheckUrl;
            _CurrentVersion = ElectronManifest.Version;

            if (_Url != null)
            {
                _LatestVersion = GetLatestReleaseVersion(_Url);
            }
        }

        // Methods

        public dynamic? GetReleases(string url)
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

        private SemanticVersion? GetLatestReleaseVersion(string url)
        {
            dynamic? json = GetReleases(url);

            if (json == null)
            {
                return null;
            }
            
            // todo: Verify that Github returns these in chronologic order
            return new SemanticVersion(json[0].tag_name.ToString());
        }

        public bool IsNewReleaseAvailable()
        {
            if (_LatestVersion != null)
            {
                return _LatestVersion > _CurrentVersion;
            }

            return false;
        }
    }
}
