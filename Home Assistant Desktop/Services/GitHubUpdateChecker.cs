using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Home_Assistant_Desktop.Services
{
    /// <summary>
    ///  Result of an update check. RetryNotBefore is set when GitHub told us we're rate
    ///  limited, so the caller can push its next check out past that point instead of
    ///  retrying on its normal schedule and hitting the same limit again.
    /// </summary>
    public sealed record UpdateCheckResult(Version? LatestVersion, DateTimeOffset? RetryNotBefore);

    /// <summary>
    ///  Checks GitHub's unauthenticated releases/latest endpoint for a newer version.
    ///  Callers are responsible for throttling how often this runs.
    /// </summary>
    public sealed class GitHubUpdateChecker
    {
        private const string ReleasesLatestUrl = "https://api.github.com/repos/LxonWWW/Home-Assistant-Desktop-WF/releases/latest";

        private static readonly HttpClient httpClient = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            HttpClient client = new() { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Home-Assistant-Desktop-WF");
            return client;
        }

        public async Task<UpdateCheckResult> CheckAsync()
        {
            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync(ReleasesLatestUrl);

                if (IsRateLimited(response))
                    return new UpdateCheckResult(null, GetRetryNotBefore(response));

                if (!response.IsSuccessStatusCode)
                    return new UpdateCheckResult(null, null);

                using Stream stream = await response.Content.ReadAsStreamAsync();
                using JsonDocument document = await JsonDocument.ParseAsync(stream);

                string? tagName = document.RootElement.GetProperty("tag_name").GetString();
                string? versionText = tagName?.TrimStart('v', 'V');
                Version? version = Version.TryParse(versionText, out Version? parsed) ? parsed : null;

                return new UpdateCheckResult(version, null);
            }
            catch
            {
                return new UpdateCheckResult(null, null);
            }
        }

        private static bool IsRateLimited(HttpResponseMessage response)
        {
            if ((int)response.StatusCode == 429)
                return true;

            return response.StatusCode == HttpStatusCode.Forbidden
                && response.Headers.TryGetValues("X-RateLimit-Remaining", out IEnumerable<string>? remaining)
                && remaining.FirstOrDefault() == "0";
        }

        private static DateTimeOffset GetRetryNotBefore(HttpResponseMessage response)
        {
            if (response.Headers.RetryAfter?.Delta is TimeSpan delta)
                return DateTimeOffset.UtcNow + delta;

            if (response.Headers.RetryAfter?.Date is DateTimeOffset date)
                return date;

            if (response.Headers.TryGetValues("X-RateLimit-Reset", out IEnumerable<string>? resetValues)
                && long.TryParse(resetValues.FirstOrDefault(), out long resetUnixSeconds))
                return DateTimeOffset.FromUnixTimeSeconds(resetUnixSeconds);

            // Unknown response shape - fall back to a conservative backoff.
            return DateTimeOffset.UtcNow.AddHours(1);
        }
    }
}
