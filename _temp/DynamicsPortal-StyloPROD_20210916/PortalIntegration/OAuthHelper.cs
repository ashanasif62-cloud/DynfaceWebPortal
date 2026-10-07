using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PortalIntegration
{

    public class OAuthHelper
    {
        public const string OAuthHeader = "Authorization";
        private static string _authorizationHeader;
        private static AuthenticationResult _authResult { get; set; }

        //public static string AuthenticationHeader(bool useWebAppAuthentication = false)

        public static async Task<string> AuthorizationHeader()
        {
            string aadTenant = ClientConfiguration.Default.ActiveDirectoryTenant;
            string aadClientAppId = ClientConfiguration.Default.ActiveDirectoryClientAppId;
            string aadResource = ClientConfiguration.Default.ActiveDirectoryResource;
            // Added following two
            string azureEndPoint = ClientConfiguration.Default.AzureAuthEndPoint;
            string aadClientSecret = ClientConfiguration.Default.ActiveDirectoryClientAppSecret;

            try
            {
                if (!string.IsNullOrEmpty(_authorizationHeader) &&

                DateTime.UtcNow.AddSeconds(180) < _authResult.ExpiresOn) return _authorizationHeader;

                var uri = new UriBuilder(azureEndPoint) { Path = aadTenant };

                var authContext = new AuthenticationContext(uri.ToString());

                var credentials = new ClientCredential(aadClientAppId, aadClientSecret);

                bool validateAuthority = authContext.ValidateAuthority;

                _authResult = await authContext.AcquireTokenAsync(aadResource, credentials);

                _authorizationHeader = _authResult.CreateAuthorizationHeader();

                return _authorizationHeader;
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public static string getAuthenticationHeader(bool useWebAppAuthentication = false)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) => {return true;};
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            string aadTenant = ClientConfiguration.Default.ActiveDirectoryTenant;

            string aadResource = ClientConfiguration.Default.ActiveDirectoryResource;
            string azureEndPoint = ClientConfiguration.Default.AzureAuthEndPoint;

            string aadClientAppId = ClientConfiguration.Default.ActiveDirectoryClientAppId;
            string aadClientAppSecret = ClientConfiguration.Default.ActiveDirectoryClientAppSecret;

            if (!string.IsNullOrEmpty(_authorizationHeader) && DateTime.UtcNow.AddSeconds(180) < _authResult.ExpiresOn)
                return _authorizationHeader;

            var uri = new UriBuilder(azureEndPoint) { Path = aadTenant };
            AuthenticationContext authenticationContext = new AuthenticationContext(uri.ToString(), false);
            AuthenticationResult authenticationResult;

            #region Odata Services
            if (useWebAppAuthentication)
            {
                if (string.IsNullOrEmpty(aadClientAppSecret))
                {
                    //Console.WriteLine("Please fill AAD application secret in ClientConfiguration if you choose authentication by the application.");
                    throw new Exception("Failed OAuth by empty application secret.");
                }

                try
                {
                    // OAuth through application by application id and application secret.
                    var creadential = new ClientCredential(aadClientAppId, aadClientAppSecret);
                    authenticationResult = authenticationContext.AcquireTokenAsync(aadResource, creadential).Result;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(string.Format("Failed to authenticate with AAD by application with exception {0} and the stack trace {1}", ex.ToString(), ex.StackTrace));
                    throw new Exception("Failed to authenticate with AAD by application.");
                }
            }
            else
            #endregion
            #region Soap/Rest-Based Custom Services
            {
                // OAuth through username and password.
                string username = ClientConfiguration.Default.UserName;
                string password = ClientConfiguration.Default.Password;

                if (string.IsNullOrEmpty(password))
                {
                    //Console.WriteLine("Please fill user password in ClientConfiguration if you choose authentication by the credential.");
                    throw new Exception("Failed OAuth by empty password.");
                }

                try
                {
                    // Get token object
                    var userCredential = new UserPasswordCredential(username, password);
                    authenticationResult = authenticationContext.AcquireTokenAsync(aadResource, aadClientAppId, userCredential).Result;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(string.Format("Failed to authenticate with AAD by the credential with exception {0} and the stack trace {1}", ex.ToString(), ex.StackTrace));
                    throw new Exception("Failed to authenticate with AAD by the credential.");
                }
            }
            #endregion

            // Create and get JWT token
            return authenticationResult.CreateAuthorizationHeader();
        }

        //public static string getChennel<T>(string _serviceName, ref T input1) where T : new()
        //{
        //    string serviceName = _serviceName;

        //    //var response = OAuthHelper.AuthorizationHeader();
        //    var authenticationHeader = OAuthHelper.getAuthenticationHeader();
        //    var serviceUriString = SoapHelper.GetSoapServiceUriString(serviceName, ClientConfiguration.Default.UriString);

        //    //var request = HttpWebRequest.Create(ClientConfiguration.Default.UriString + "api/services/UserSessionService/AifUserSessionService/GetUserSessionInfo");
        //    //request.Headers[OAuthHelper.OAuthHeader] = OAuthHelper.GetAuthenticationHeader();
        //    //request.Method = "POST";
        //    //request.GetResponse();

        //    var endpointAddress = new EndpointAddress(serviceUriString);
        //    var binding = SoapHelper.GetBinding();





        //    var client = new PREarningsSvcReference.PREarningsSvcClient(binding, endpointAddress);
        //    var channel = client.InnerChannel;


        //    using (OperationContextScope operationContextScope = new OperationContextScope(channel))
        //    {
        //        HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //        requestMessage.Headers[OAuthHelper.OAuthHeader] = authenticationHeader;
        //        OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;



        //    }


        //    return "";
        //}



    }
}


