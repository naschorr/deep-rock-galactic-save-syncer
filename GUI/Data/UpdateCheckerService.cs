using Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;

namespace GUI.Data
{
    public class UpdateCheckerService
    {
        [Inject]
        private ILogger<UpdateCheckerService> _Logger { get; set; }
        [Inject]
        private ConfigLoaderService _ConfigLoader { get; set; }
        [Inject]
        private ElectronManifestService _ElectronManifest { get; set; }

        private readonly string? _Url;
        private readonly SemanticVersion _CurrentVersion;

        public readonly Update? LatestVersion;

        // Constructor

        public UpdateCheckerService(ILogger<UpdateCheckerService> logger, ElectronManifestService electronManifestService, ConfigLoaderService configLoaderService)
        {
            _Logger = logger;
            _ElectronManifest = electronManifestService;
            _ConfigLoader = configLoaderService;

            _Url = _ConfigLoader.Config?.updateCheckUrl;
            _CurrentVersion = _ElectronManifest.Version;

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

            _Logger.LogWarning($"Unable to load releases from {url}");
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

                    var update = new Update(releaseVersion, releaseUrl);
                    _Logger.LogInformation($"Found latest release version: {update}, url: {update.UpdateLink}");

                    return update;
                }
            }

            return null;
        }

        public bool IsNewReleaseAvailable()
        {
            if (LatestVersion != null && LatestVersion > _CurrentVersion)
            {
                _Logger.LogInformation($"New relase is available: {LatestVersion} > {_CurrentVersion}");
                return true;
            }

            _Logger.LogInformation("No new releases available");
            return false;
        }
    }
}
