using BussinessLogic;
using BussinessObject;
using GeneralAuxiliary;
using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace AuthenticationHelper
{
    public class AuthenticationHelper
    {
        private string authenticationMessage = string.Empty;

        private string pageMenuId = string.Empty;

        public string AuthenticationMessage { get => authenticationMessage; set => authenticationMessage = value; }

        public bool authenticateUser(string _userId, string _password)
        {
            try
            {
                bool isAuthenticated = false;
                string userId = _userId;
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    DataTable userData;
                    string message = string.Empty;
                    string encryptedPass = SecurePassword.securePassword(userId, _password);

                    SysUserInfo_BOL objBOL = new SysUserInfo_BOL();
                    objBOL.UserId = userId;
                    objBOL.Password = encryptedPass;

                    SysUserInfo_BLL objBLL = new SysUserInfo_BLL();
                    userData = objBLL.validateUser(objBOL);

                    if (userData != null)
                        if (userData.Rows.Count > 0)
                        {
                            DataRow dataRow = userData.Rows[0];
                            userId = dataRow["UserId"] == DBNull.Value ? string.Empty : (dataRow["UserId"]).ToString();
                            string employeeId = dataRow["EmployeeId"] == DBNull.Value ? string.Empty : dataRow["EmployeeId"].ToString();
                            string employeeName = dataRow["EmployeeName"] == DBNull.Value ? string.Empty : dataRow["EmployeeName"].ToString();
                            string defaultCompany = dataRow["DefaultCompany"] == DBNull.Value ? string.Empty : dataRow["DefaultCompany"].ToString();
                            string defaultCompanyName = dataRow["DefaultCompanyName"] == DBNull.Value ? string.Empty : dataRow["DefaultCompanyName"].ToString();
                            long partition = 0;// dr["Partition"] == DBNull.Value ? -1 : Convert.ToInt64(dr["Partition"]);
                            Int64.TryParse(dataRow["Partition"].ToString(), out partition);

                            int authCode = 0;
                            Int32.TryParse(dataRow["isAuthenticated"].ToString(), out authCode);
                            message = dataRow["Message"] == DBNull.Value ? string.Empty : dataRow["Message"].ToString();

                            switch (authCode)
                            {
                                case 1:         // User is Authenticated & Activated
                                    objBOL.UserId = userId;
                                    objBOL.EmployeeId = employeeId;
                                    objBOL.EmployeeName = employeeName;
                                    objBOL.DataAreaId = defaultCompany;
                                    objBOL.DataAreaName = defaultCompanyName;
                                    objBOL.Partition = partition;

                                    bool isStored = SessionVariables.setUserInfo(objBOL);

                                    if (isStored)
                                        isAuthenticated = true;
                                    else
                                    {
                                        isAuthenticated = false;
                                        message = "Incorrect User Related Information";
                                    }
                                    break;
                            }
                        }
                    if (string.IsNullOrEmpty(message))
                        message = "Something went wrong!";

                    authenticationMessage = message;
                }
                return isAuthenticated;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
                return false;
            }
            finally
            { }
        }



        public bool validateUserRoles()
        {
            bool isValidated = false;
            Page currentPage = HttpContext.Current.CurrentHandler as Page;

            HtmlGenericControl menuESS = currentPage.Master.FindControl("userMenuItems_ESS") as HtmlGenericControl;
            HtmlGenericControl menuMSS = currentPage.Master.FindControl("userMenuItems_MSS") as HtmlGenericControl;
            HtmlGenericControl menuAdmin = currentPage.Master.FindControl("userMenuItems_Admin") as HtmlGenericControl;

            DataTable dtUserRoles = SessionVariables.getUserRoles();
            if (dtUserRoles.Columns.Contains("RoleId") && currentPage.Master != null)
            {
                isValidated = true;
                if (menuESS != null)
                {
                    DataRow drESS = dtUserRoles.Select("RoleId = 'ESS'").FirstOrDefault();
                    if (drESS == null)
                        menuESS.Visible = false;
                    else
                        menuESS.Visible = true;
                }

                if (menuMSS != null)
                {
                    DataRow drMSS = dtUserRoles.Select("RoleId = 'MSS'").FirstOrDefault();
                    if (drMSS == null)
                        menuMSS.Visible = false;
                    else
                        menuMSS.Visible = true;
                }

                if (menuAdmin != null)
                {
                    DataRow drAdmin = dtUserRoles.Select("RoleId = 'Admin'").FirstOrDefault();
                    if (drAdmin == null)
                        menuAdmin.Visible = false;
                    else
                        menuAdmin.Visible = true;
                }
            }
            else
                isValidated = false;

            return isValidated;
        }

        public bool userAuthentication()
        {
            bool isUserAuthenticated = validateUserAuthentication();
            if (!isUserAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            return isUserAuthenticated;
        }

        private bool validateUserAuthentication()
        {
            bool isUserAuthenticated = true;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //FormsAuthentication.RedirectToLoginPage();
                isUserAuthenticated = false;
            }
            else
            {
                string employeeName = SessionVariables.getCurrentEmployeeName();
                if (string.IsNullOrWhiteSpace(employeeName) || HttpContext.Current.User.Identity.Name != employeeName)
                {
                    //FormsAuthentication.RedirectToLoginPage();
                    isUserAuthenticated = false;
                }
            }
            return isUserAuthenticated;
        }

        public bool pageAuthentication(string _pageMenuId)
        {
            pageMenuId = _pageMenuId;

            bool isPageAuthorizated = validatePageAuthorization();
            if (!isPageAuthorizated)
            {
                //Response.Redirect(Request.UrlReferrer.ToString());

                HttpContext.Current.Server.Transfer("/ErrorPages/403.aspx");
                //Context.ApplicationInstance.CompleteRequest();
                //FormsAuthentication.RedirectToLoginPage();
            }
            return isPageAuthorizated;
        }

        private bool validatePageAuthorization()
        {
            bool isValidated = false;

            DataTable dtUserMenuItems = SessionVariables.getUserMenuItems();
            if (dtUserMenuItems.Columns.Contains("MenuId"))
            {
                DataRow drMenu = dtUserMenuItems.Select("MenuId = '" + pageMenuId + "'").FirstOrDefault();
                if (drMenu != null)
                {
                    isValidated = true;
                }
            }

            return isValidated;
        }




    }
}