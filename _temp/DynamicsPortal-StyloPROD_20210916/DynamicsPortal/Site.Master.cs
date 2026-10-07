using GeneralAuxiliary;
using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;

namespace DynamicsPortal
{
    public partial class Site : System.Web.UI.MasterPage
    {
        private const string antiXsrfTokenKey = "2bx3c22dfUserg3dd42sdLoginb36k8s";     //"__AntiXsrfToken";
        private const string antiXsrfUserNameKey = "dfg2gfiYG4J78HDfdshG64KKUwe23rtha43dfww32egh";  //"__AntiXsrfUserName";
        private string antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            //First, check for the existence of the Anti-XSS cookie
            var requestCookie = Request.Cookies[antiXsrfTokenKey];
            Guid requestCookieGuidValue;

            //If the CSRF cookie is found, parse the token from the cookie.
            //Then, set the global page variable and view state user
            //key. The global variable will be used to validate that it matches 
            //in the view state form field in the Page.PreLoad method.
            if (requestCookie != null
                && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                //Set the global token variable so the cookie value can be
                //validated against the value in the view state form field in
                //the Page.PreLoad method.
                antiXsrfTokenValue = requestCookie.Value;

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = antiXsrfTokenValue;
            }
            //If the CSRF cookie is not found, then this is a new session.
            else
            {
                //Generate a new Anti-XSRF token
                antiXsrfTokenValue = Guid.NewGuid().ToString("N");

                //Set the view state user key, which will be validated by the
                //framework during each request
                Page.ViewStateUserKey = antiXsrfTokenValue;

                //Create the non-persistent CSRF cookie
                var responseCookie = new HttpCookie(antiXsrfTokenKey)
                {
                    //Set the HttpOnly property to prevent the cookie from
                    //being accessed by client side script
                    HttpOnly = true,

                    //Add the Anti-XSRF token to the cookie value
                    Value = antiXsrfTokenValue
                };

                //If we are using SSL, the cookie should be set to secure to
                //prevent it from being sent over HTTP connections
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }

                //Add the CSRF cookie to the response
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                //During the initial page load, add the Anti-XSRF token and user
                //name to the ViewState
                if (!IsPostBack)
                {
                    //Set Anti-XSRF token
                    ViewState[antiXsrfTokenKey] = Page.ViewStateUserKey;

                    //If a user name is assigned, set the user name
                    ViewState[antiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
                }
                //During all subsequent post backs to the page, the token value from
                //the cookie should be validated against the token in the view state
                //form field. Additionally user name should be compared to the
                //authenticated users name
                else
                {
                    //Validate the Anti-XSRF token
                    if ((string)ViewState[antiXsrfTokenKey] != antiXsrfTokenValue || (string)ViewState[antiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                    {
                        throw new InvalidOperationException("Validation of token failed.");
                    }
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally
            { }
        }






        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadCurrentUserImage();
            }
            generateMainMenu();
        }

        public void loadCurrentUserImage()
        {
            string userImageData = ControlsHelper.getCurrentUserImage();
            HtmlImage imgUser = HeadLoginView.FindControl("imgUser") as HtmlImage;

            if (imgUser != null)
            {
                if (string.IsNullOrEmpty(userImageData))
                    imgUser.Src = "/distribution/img/User.png";
                else
                    imgUser.Src = "data:image/png;base64," + userImageData;
            }
            //if ((HttpContext.Current.CurrentHandler as Page).Master != null)
            //{
            //    HtmlImage imgUser = (HttpContext.Current.CurrentHandler as Page).Master.FindControl("imgUser") as HtmlImage;
            //}
        }

        private void generateMainMenu()
        {
            DataTable userMenuItems = SessionVariables.getUserMenuItems();
            if (userMenuItems != null)
            {
                GenerateMenu menu = new GenerateMenu();
                menu.generateMenu(nav_userMenuItems, userMenuItems);
            }
        }

    }
}