using GeneralAuxiliary;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace DynamicsPortal.ErrorPages
{
    public partial class CustomErrorPage : System.Web.UI.Page
    {
        private string pageTitle;
        protected void Page_Load(object sender, EventArgs e)
        {
            pageTitle = "404";
            isAuthenticatedUser();
            setPageTitle();
        }
        private Boolean isAuthenticatedUser()
        {
            bool isAuthenticated = true;
            if (!Page.User.Identity.IsAuthenticated)
            {
                isAuthenticated = false;
                FormsAuthentication.RedirectToLoginPage();
            }
            else
            {
                string userName = SessionVariables.getCurrentEmployeeName();
                if (string.IsNullOrWhiteSpace(userName) || Page.User.Identity.Name != userName)
                {
                    isAuthenticated = false;
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
            return isAuthenticated;
        }

        private void setPageTitle()
        {
            if ((HttpContext.Current.CurrentHandler as Page).Master != null)
            {
                HtmlGenericControl lblPageTitle = (HttpContext.Current.CurrentHandler as Page).Master.FindControl("pageTitle") as HtmlGenericControl;
                lblPageTitle.InnerHtml = pageTitle;
            }
        }


    }
}