using AuthenticationHelper;
using BussinessObject;
using GeneralAuxiliary;
using Newtonsoft.Json;
using PortalIntegration;
using System;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace DynamicsPortal.Administration
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            checkConfiguration();
            if (!IsPostBack)
            {
                if (Session["LoginError"] != null)
                {
                    string errorMsg = Session["LoginError"].ToString();
                    SysOperationResult_BOL result = new SysOperationResult_BOL();
                    result.Message = errorMsg;
                    result.AlertType = AlertType.Error.ToString();
                    NotificationMessage.showMessage(result);
                    Session["LoginError"] = null;
                }


                if (Session["LoginError"] == null)
                {
                    AuthenticateUser authenticateUser = new AuthenticateUser();
                    authenticateUser.clearAutentication();
                }
            }
        }

        //protected void btnLogin_Click(object sender, EventArgs e)
        //{
        //    string userName = txtUserId.Value;
        //    string userPassword = txtPassword.Value;
        //    bool isAuthenticated;

        //    AuthenticateUser authenticateUser = new AuthenticateUser();
        //    isAuthenticated = authenticateUser.authenticateUser(userName, userPassword);
        //    if (isAuthenticated)
        //    {
        //    }
        //}

        protected void btnMicrosoftLogin_Click(object sender, EventArgs e)
        {
            string clientId = ClientConfiguration.Default.ActiveDirectoryClientAppId;
            string redirectUri = HttpUtility.UrlEncode(ClientConfiguration.Default.AzureADRedirectURI);
            string tenant = ClientConfiguration.Default.ActiveDirectoryTenant; // or your tenant ID
            string scope = HttpUtility.UrlEncode("openid profile email offline_access " + ClientConfiguration.Default.ActiveDirectoryResource + ".default");

            string authUrl = $"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize" +
                             $"?client_id={clientId}" +
                             $"&response_type=code" +
                             $"&redirect_uri={redirectUri}" +
                             $"&response_mode=query" +
                             $"&scope={scope}" +
                             $"&state=12345" +
                             $"&prompt=select_account";

            Response.Redirect(authUrl, false);
        }
        protected void btnGoogleLogin_Click(object sender, EventArgs e)
        {
            string clientId = "127486062920-go66l63uj5a33kr4lqfr43kbo8bffkk8.apps.googleusercontent.com";
            string redirectUri = ClientConfiguration.Default.GoogleRedirectURI;
            string responseType = "code";
            string scope = "openid email profile";

            string authUrl = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}" +
                             $"&redirect_uri={redirectUri}" +
                             $"&response_type={responseType}" +
                             $"&scope={scope}" +
                             $"&access_type=offline" +
                             $"&prompt=consent";

            Response.Redirect(authUrl);
        }

        private void checkConfiguration()
        {
            string ConfigPath = Server.MapPath("~/App_Data/ClientConfiguration.json");

            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                dynamic config = JsonConvert.DeserializeObject(json);

                if (config != null)
                {
                    ClientConfiguration.Default.UriString = config?.ActiveDirectoryResource ?? "";
                    ClientConfiguration.Default.ActiveDirectoryResource = config?.ActiveDirectoryResource ?? "";
                    ClientConfiguration.Default.ActiveDirectoryTenant = config?.ActiveDirectoryTenant ?? "";
                    ClientConfiguration.Default.ActiveDirectoryClientAppId = config?.ActiveDirectoryClientAppId ?? "";
                    ClientConfiguration.Default.ActiveDirectoryClientAppSecret = config?.ActiveDirectoryClientAppSecret ?? "";

                }
                else
                {
                    Response.Redirect("/EditConfig.aspx");
                }
            }
            else
            {
                Response.Redirect("/EditConfig.aspx");
            }
        }
    }
}