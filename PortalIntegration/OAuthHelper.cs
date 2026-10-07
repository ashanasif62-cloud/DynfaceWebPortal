using GeneralAuxiliary;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PortalIntegration
{

    public class OAuthHelper
    {
        public const string OAuthHeader = "Authorization";

        // Thread-safe token cache
        private static string _cachedAuthorizationHeader;
        private static DateTimeOffset _tokenExpiresOn;
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private const int ExpiryBufferMinutes = 5;

        // Phase 4 Optimization: Configure TLS + cert callback ONCE at app-domain startup
        static OAuthHelper()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => true;
        }

        /// <summary>
        /// Asynchronously gets the authorization header, using a cached token if valid.
        /// </summary>
        public static async Task<string> AuthorizationHeader()
        {
            // 1. Fast Path: Check cache without lock
            if (IsCacheValid())
            {
                return _cachedAuthorizationHeader;
            }

            // 2. Slow Path: Enter lock to fetch/refresh
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                // Double-check inside lock
                if (IsCacheValid())
                {
                    return _cachedAuthorizationHeader;
                }

                await RefreshTokenAsync().ConfigureAwait(false);
                return _cachedAuthorizationHeader;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Synchronously gets the authorization header. Used extensively across the legacy codebase.
        /// </summary>
        public static string getAuthenticationHeader(bool useWebAppAuthentication = true)
        {
            string isD365User = SessionVariables.IsD365FOUser();
            if (!string.IsNullOrEmpty(isD365User) && isD365User.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                _semaphore.Wait();
                try
                {
                    string refreshToken = SessionVariables.getCurrentRefreshToken();
                    var (newAccess, newRefresh) = Task.Run(async () =>
                        await RefreshUserSessionTokenAsync(refreshToken).ConfigureAwait(false)).Result;

                    SessionVariables.setCurrentD365FOToken(newAccess);
                    SessionVariables.setCurrentRefreshToken(newRefresh);
                    return _cachedAuthorizationHeader;
                }
                finally
                {
                    _semaphore.Release();
                }
            }
            // 1. Fast Path: Check cache without lock (TLS configured once in static ctor)
            if (IsCacheValid())
            {
                return _cachedAuthorizationHeader;
            }

            // 2. Slow Path: Blocking wait on semaphore
            _semaphore.Wait();
            try
            {
                // Double-check inside lock
                if (IsCacheValid())
                {
                    return _cachedAuthorizationHeader;
                }

                // Call async fetch and block safely
                Task.Run(async () => await RefreshTokenAsync().ConfigureAwait(false)).Wait();
                return _cachedAuthorizationHeader;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Forces the cache to clear. Call this if a 401 Unauthorized is received from the server.
        /// </summary>
        public static void ClearCache()
        {
            _cachedAuthorizationHeader = null;
            _tokenExpiresOn = DateTimeOffset.MinValue;
        }

        private static bool IsCacheValid()
        {
            return !string.IsNullOrEmpty(_cachedAuthorizationHeader) && 
                   DateTimeOffset.UtcNow.AddMinutes(ExpiryBufferMinutes) < _tokenExpiresOn;
        }

        private static async Task RefreshTokenAsync()
        {
            string aadTenant = ClientConfiguration.Default.ActiveDirectoryTenant;
            string aadClientAppId = ClientConfiguration.Default.ActiveDirectoryClientAppId;
            string aadResource = ClientConfiguration.Default.ActiveDirectoryResource;
            string azureEndPoint = ClientConfiguration.Default.AzureAuthEndPoint;
            string aadClientSecret = ClientConfiguration.Default.ActiveDirectoryClientAppSecret;

            var uri = new UriBuilder(azureEndPoint) { Path = aadTenant };
            var authContext = new AuthenticationContext(uri.ToString());
            var credentials = new ClientCredential(aadClientAppId, aadClientSecret);

            // Fetching token from Azure AD
            AuthenticationResult authResult = await authContext.AcquireTokenAsync(aadResource, credentials).ConfigureAwait(false);

            if (authResult != null)
            {
                _cachedAuthorizationHeader = authResult.CreateAuthorizationHeader();
                _tokenExpiresOn = authResult.ExpiresOn;
            }
            else
            {
                throw new Exception("Failed to acquire OAuth token from Azure AD.");
            }
        }
        private static async Task<(string accessToken, string newRefreshToken)> RefreshUserSessionTokenAsync(string _refreshToken)
        {
            string refreshToken = _refreshToken;
            if (string.IsNullOrEmpty(refreshToken))
                return (null, null);

            string clientId = ClientConfiguration.Default.ActiveDirectoryClientAppId;
            string clientSecret = ClientConfiguration.Default.ActiveDirectoryClientAppSecret;
            string tenantId = ClientConfiguration.Default.ActiveDirectoryTenant;
            string resource = ClientConfiguration.Default.ActiveDirectoryResource;

            var values = new Dictionary<string, string>
            {
                { "grant_type",    "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id",     clientId },
                { "client_secret", clientSecret },
                { "scope",         resource + ".default" }
            };

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token",
                    new FormUrlEncodedContent(values)
                ).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    //Logger.Warning("Session token refresh failed: " + await response.Content.ReadAsStringAsync());
                    return (null, null);
                }

                var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                string newAccessToken = json.Value<string>("access_token");
                string newRefreshToken = json.Value<string>("refresh_token");

                _cachedAuthorizationHeader = "Bearer " + newAccessToken;
                return (newAccessToken, newRefreshToken);
            }
        }
    }
}



