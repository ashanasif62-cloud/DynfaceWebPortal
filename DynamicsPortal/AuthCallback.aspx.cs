using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using Newtonsoft.Json.Linq;
using PortalIntegration;
using PortalIntegration.DFUserInfoSvcReference;
//using PortalIntegration.PREmploymentInformationSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Security;

namespace DynamicsPortal
{
    public partial class AuthCallback : System.Web.UI.Page
    {
        private SysUserInfo_BLL sysUserInfo_BLL = new SysUserInfo_BLL();

        protected async void Page_Load(object sender, EventArgs e)
        {
            string code = Request.QueryString["code"];
            string error = Request.QueryString["error"];

            if (!string.IsNullOrEmpty(error))
            {
                Session["LoginError"] = "Microsoft login was cancelled or denied.";
                string EmployeeName = SessionVariables.getCurrentEmployeeName();
                FormsAuthentication.RedirectFromLoginPage(EmployeeName, false);
                return;
            }

            if (!string.IsNullOrEmpty(code))
            {
                await ExchangeCodeForToken(code);
            }
            else
            {
                Session["LoginError"] = "Authentication failed or missing authorization code.";
                string EmployeeName = SessionVariables.getCurrentEmployeeName();
                FormsAuthentication.RedirectFromLoginPage(EmployeeName, false);
            }
        }

        private async Task ExchangeCodeForToken(string code)
        {
            string clientId = ClientConfiguration.Default.ActiveDirectoryClientAppId;
            string clientSecret = ClientConfiguration.Default.ActiveDirectoryClientAppSecret;
            string tenantId = ClientConfiguration.Default.ActiveDirectoryTenant;
            string redirectUri = ClientConfiguration.Default.AzureADRedirectURI;

            // Step 1: Exchange code for access token
            var values = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                //{ "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" },
                { "scope", "openid offline_access " + ClientConfiguration.Default.ActiveDirectoryResource + ".default"}
            };

            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync($"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Session["LoginError"] = "Failed to retrieve access token: " + responseString;
                string EmployeeName = SessionVariables.getCurrentEmployeeName();
                FormsAuthentication.RedirectFromLoginPage(EmployeeName, false);
                return;
            }

            var tokenJson = JObject.Parse(responseString);
            string accessToken = tokenJson.Value<string>("access_token");
            string refreshToken = tokenJson.Value<string>("refresh_token");
            
            dynamic userInfo = ParseAccessToken(accessToken);
            bool isAccessTokenStored = SessionVariables.setCurrentD365FOToken(accessToken);
            bool isRefreshTokenStored = SessionVariables.setCurrentRefreshToken(refreshToken);

            if (userInfo == null)
            {
                Session["LoginError"] = "Authentication Failed. Contact support.";
                string EmployeeName = SessionVariables.getCurrentEmployeeName();
                FormsAuthentication.RedirectFromLoginPage(EmployeeName, false);
                return;
            }

            // Step 3: Retrieve user data from D365FO via PREmploymentInformationSvc
            DFUserInfoSvcContract employeeInfo = new DFUserInfo().retrieveUserInfoByEmail((string)userInfo.mail ?? (string)userInfo.upn);

            if (employeeInfo == null)
            {
                Session["LoginError"] = "This Account is not Authorized to access this Portal.";
                string EmployeeName = SessionVariables.getCurrentEmployeeName();
                FormsAuthentication.RedirectFromLoginPage(EmployeeName, false);
                return;
            }
            SessionVariables.IsD365FOUser(employeeInfo.isDynamicsUser.ToString());

            // Step 4: Build SysUserInfo_BOL object
            SysUserInfo_BOL objBOL = new SysUserInfo_BOL
            {
                UserId = "su.admin",
                //UserId = (string)userInfo.mail ?? (string)userInfo.upn,
                UserName = employeeInfo.EmployeeName,
                EmployeeId = employeeInfo.EmployeeId,
                EmployeeName = employeeInfo.EmployeeName,
                DataAreaId = employeeInfo.DefaultLegalEntity,
                DataAreaName = employeeInfo.DefaultLegalEntity,
                EmployeePersonId = employeeInfo.Person,
                Partition = 5637144576,
                EmployeeEmailId = (string)userInfo.mail ?? (string)userInfo.upn
            };

            // Step 5: Store session variables
            bool isStored = SessionVariables.setUserInfo(objBOL);
            DataTable dtUserMenuItems, dtUserCompanies;
            dtUserMenuItems = dtUserCompanies = null;
            if (ClientConfiguration.Default.connectWithSQL)
            {
                dtUserMenuItems = SysUserMenuItems_BLL.SysUserMenuItems_Retrieve();
                //DataTable dtUserRoles = SysUserRoles_BLL.SysUserRoles_RetrieveByUserId();
                dtUserCompanies = SysUserCompanies_BLL.SysUserCompanies_Retrieve();
            }
            else
            {
                dtUserMenuItems = new DFUserInfo().retrieveEmployeeMenuItems();
                dtUserCompanies = new DFUserInfo().retrieveEmployeeLegalEntities();
            }

            bool isUserMenuItemsStored = SessionVariables.setUserMenuItems(dtUserMenuItems);
            //bool isUserRolesStored = SessionVariables.setUserRoles(dtUserRoles);
            bool isUserCompaniesStored = SessionVariables.setUserCompanies(dtUserCompanies);
            // Step 6: Final redirect if everything is set
            if (isStored && isUserMenuItemsStored && isUserCompaniesStored && isAccessTokenStored)
            {
                string employeeName = SessionVariables.getCurrentEmployeeName();
                FormsAuthentication.RedirectFromLoginPage(employeeName, false);
            }
            else
            {
                Session["LoginError"] = "Unable to initialize user session.";
                string EmployeeName = SessionVariables.getCurrentEmployeeName();
                FormsAuthentication.RedirectFromLoginPage(EmployeeName, false);
            }
        }

        private dynamic ParseAccessToken(string _accessToken)
        {
            string[] parts = _accessToken.Split('.');
            if (parts.Length < 3)
                return null;

            string payload = parts[1];
            payload = payload.Replace('-', '+').Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var bytes = Convert.FromBase64String(payload);
            var json = System.Text.Encoding.UTF8.GetString(bytes);
            return JObject.Parse(json);
        }
    }
}
