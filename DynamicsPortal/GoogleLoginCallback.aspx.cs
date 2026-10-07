using AuthenticationHelper;
using BussinessLogic;
using BussinessObject;
using DataAccess;
using GeneralAuxiliary;
using Newtonsoft.Json.Linq;
using PortalIntegration;
using PortalIntegration.DFUserInfoSvcReference;
using PortalIntegration.PREmploymentInformationSvcReference;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace DynamicsPortal
{
    public partial class GoogleLoginCallback : System.Web.UI.Page
    {
        private SysUserInfo_BLL sysUserInfo_BLL = new SysUserInfo_BLL();
        protected async void Page_Load(object sender, EventArgs e)
        {
            string code = Request.QueryString["code"];
            string error = Request.QueryString["error"];

            if (!string.IsNullOrEmpty(error))
            {
                // Handle access_denied or any other OAuth error
                Session["LoginError"] = "Google login was cancelled or denied.";
                string EmployeeName = SessionVariables.getCurrentEmployeeName();
                FormsAuthentication.RedirectFromLoginPage(EmployeeName, false);
                return;
            }

            if (!string.IsNullOrEmpty(code))
            {
                await ExchangeCodeForToken(code);
            }
        }

        private async Task ExchangeCodeForToken(string code)
        {
            string clientId = "127486062920-go66l63uj5a33kr4lqfr43kbo8bffkk8.apps.googleusercontent.com";
            string clientSecret = "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";
            string redirectUri = ClientConfiguration.Default.GoogleRedirectURI;
            bool isAuthenticated;

            var values = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            };

            HttpClient client = new HttpClient();
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync("https://oauth2.googleapis.com/token", content);
            var responseString = await response.Content.ReadAsStringAsync();
            SysUserInfo_BOL objBOL = new SysUserInfo_BOL();

            var json = JObject.Parse(responseString);
            string idToken = json.Value<string>("id_token");

            if (!string.IsNullOrEmpty(idToken))
            {
                dynamic userInfo = ParseIdToken(idToken);
                if (userInfo != null)
                {
                    // Session["GoogleUser"] = userInfo;

                    DFUserInfoSvcContract employeeInfo = new DFUserInfo().retrieveUserInfoByEmail((string)userInfo.email ?? (string)userInfo.upn);

                    if (employeeInfo == null)
                    {
                        Session["LoginError"] = "This Account is not Authorized to access this Portal.";
                        string EmployeeName = SessionVariables.getCurrentEmployeeName();
                        FormsAuthentication.RedirectFromLoginPage(EmployeeName, false);
                        return;
                    }
                    SessionVariables.IsD365FOUser(employeeInfo.isDynamicsUser.ToString());

                    objBOL.UserId = "su.admin";
                    objBOL.UserName = employeeInfo.EmployeeName;
                    objBOL.EmployeeId = employeeInfo.EmployeeId;
                    objBOL.EmployeeName = employeeInfo.EmployeeName;
                    objBOL.DataAreaId = employeeInfo.DefaultLegalEntity;
                    objBOL.DataAreaName = employeeInfo.DefaultLegalEntity;
                    objBOL.Partition = 5637144576;

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
                    //DataTable dtUserQuickLinks = UserQuickLinks_BLL.getUserQuickLinks();
                    //DataTable dtUserRecentMenuItems = UserRecentMenuItems_BLL.getUserRecentMenuItems();

                    bool isUserMenuItemsStored = SessionVariables.setUserMenuItems(dtUserMenuItems);
                    //bool isUserRolesStored = SessionVariables.setUserRoles(dtUserRoles);
                    bool isUserCompaniesStored = SessionVariables.setUserCompanies(dtUserCompanies);
                    //bool isUserQuickLinkStored = SessionVariables.setUserQuickLinks(dtUserQuickLinks);
                    //bool isUserRecentCofigStored = SessionVariables.setUserRecentMenuItems(dtUserRecentMenuItems);

                    if (isStored && isUserMenuItemsStored && isUserCompaniesStored)
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



                    //Response.Redirect("Default.aspx");
                }
            }
        }

        private dynamic ParseIdToken(string idToken)
        {
            string[] parts = idToken.Split('.');
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
