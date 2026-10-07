using AuthenticationHelper;
using System;
using System.Web;
using System.Web.Security;

namespace DynamicsPortal.Administration
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AuthenticateUser authenticateUser = new AuthenticateUser();
                authenticateUser.clearAutentication();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserId.Value;
            string userPassword = txtPassword.Value;
            bool isAuthenticated;

            AuthenticateUser authenticateUser = new AuthenticateUser();
            isAuthenticated = authenticateUser.authenticateUser(userName, userPassword);
            if (isAuthenticated)
            {
            }
        }

    }
}