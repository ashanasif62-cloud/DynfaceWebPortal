using BussinessLogic;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HcmWorkerDetailsSvcReference;
using System;
using System.Data;
using System.Web.Security;

namespace AuthenticationHelper
{
    public class AuthenticateUser
    {
        public bool authenticateUser(string _userName, string _userPassword)
        {
            string userName = _userName;
            string userPassword = _userPassword;
            string alertMessage = string.Empty;
            bool isAuthenticated = false;

            // Todo Login Request Count Flag

            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(userPassword))
            {
                AuthenticationHelper authenticationHelper = new AuthenticationHelper();
                isAuthenticated = authenticationHelper.authenticateUser(userName, userPassword);
                alertMessage = authenticationHelper.AuthenticationMessage;


                if (isAuthenticated)
                {
                    isAuthenticated = isESSAllowed();
                    if (isAuthenticated)
                    {
                        DataTable dtUserMenuItems = SysUserMenuItems_BLL.SysUserMenuItems_Retrieve();
                        DataTable dtUserRoles = SysUserRoles_BLL.SysUserRoles_RetrieveByUserId();
                        DataTable dtUserCompanies = SysUserCompanies_BLL.SysUserCompanies_Retrieve();
                        //DataTable dtUserQuickLinks = UserQuickLinks_BLL.getUserQuickLinks();
                        //DataTable dtUserRecentMenuItems = UserRecentMenuItems_BLL.getUserRecentMenuItems();

                        bool isUserMenuItemsStored = SessionVariables.setUserMenuItems(dtUserMenuItems);
                        bool isUserRolesStored = SessionVariables.setUserRoles(dtUserRoles);
                        bool isUserCompaniesStored = SessionVariables.setUserCompanies(dtUserCompanies);
                        //bool isUserQuickLinkStored = SessionVariables.setUserQuickLinks(dtUserQuickLinks);
                        //bool isUserRecentCofigStored = SessionVariables.setUserRecentMenuItems(dtUserRecentMenuItems);

                        if (isUserCompaniesStored)
                            if (isUserRolesStored)
                                if (isUserMenuItemsStored)
                                {
                                    string EmployeeName = SessionVariables.getCurrentEmployeeName();
                                    ////Server.Transfer("Default.aspx", true);
                                    bool rememberMe = false;//cbRememberMe.Checked;
                                    FormsAuthentication.RedirectFromLoginPage(EmployeeName, rememberMe);
                                    ////Response.Redirect("Default.aspx");
                                }
                                else
                                    alertMessage = "Unable to Retrieve User Menu(s).";
                            else
                                alertMessage = "Unable to Retrieve User Role(s).";
                        else
                            alertMessage = "Unable to Retrieve User Company(s).";

                    }
                    else
                    {
                        alertMessage = "You are not authorised to access this portal.";
                    }

                }
            }
            else
            {
                alertMessage = "User Name or Password is Missing.";
            }
            NotificationMessage.showMessage(AlertType.Error, alertMessage);
            return isAuthenticated;

        }

        public bool clearAutentication()
        {
            SessionVariables.clearSession();
            FormsAuthentication.SignOut();
            return true;
        }


        public bool isESSAllowed()
        {
            bool isESSAllowed = false;
            try
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();

                if (string.IsNullOrEmpty(employeeId))
                    return isESSAllowed;

                HcmWorkerDetails hcmWorkerDetails = new HcmWorkerDetails();
                HcmWorkerDetailsSvcContract workerDetailsContract = hcmWorkerDetails.retrieveWorkerDetails(employeeId);
                if (workerDetailsContract != null)
                    isESSAllowed = workerDetailsContract.isValid;

                return isESSAllowed;

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

    }
}
