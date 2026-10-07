using GeneralAuxiliary;
using System;
using System.Web.Security;

namespace DynamicsPortal
{
    public class Global : System.Web.HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            if (User == null || !User.Identity.IsAuthenticated)
                System.Web.Routing.RouteTable.Routes.MapPageRoute("UserLogin", "UserLogin", "~/Administration/UserLogin.aspx");
            //Context.Response.Redirect("/Administration/UserLogin.aspx");
            //FormsAuthentication.RedirectToLoginPage();
            //System.Web.Routing.RouteTable.Routes.MapPageRoute("Index", "Home", "~/Index.aspx");
            //System.Web.Routing.RouteTable.Routes.MapPageRoute("UserHome", "User/Index", "~/UserPages/UserHome.aspx");
        }

        private void Application_BeginRequest(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            //HttpContext.Current.Response.Cache.SetNoStore();
            //HttpContext.Current.Response.AddHeader("X-FRAME-OPTIONS ", "DENY");
        }

        private void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            FormsAuthentication.SignOut();
        }

        private void Application_Error(object sender, EventArgs e)
        {
            //// Code that runs when an unhandled error occurs

            //// Get the exception object.
            Exception exc = Server.GetLastError();
            //// Clear the error from the server
            //Server.ClearError();

            //Handle HTTP errors
            //if (exc.GetType() == typeof(HttpException))
            //{
            //    // The Complete Error Handling Example generates
            //    // some errors using URLs with "NoCatch" in them;
            //    // ignore these here to simulate what would happen
            //    // if a global.asax handler were not implemented.
            //    if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
            //        return;

            //    //Redirect HTTP errors to HttpError page
            //    Server.Transfer("HttpErrorPage.aspx");
            //}

            //string message = exc.Message;
            SysErrorLog objErrorLog = new SysErrorLog();
            System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
            string currentMethodName = currentMethod.DeclaringType.FullName;
            objErrorLog.write(currentMethodName, exc);
            
            // Handle specific exception.
            //if (exc is HttpUnhandledException)
            //{
            //    NotificationMessage.showMessage(AlertType.Error, "An error occurred on this page. Please verify your " +
            //    "information to resolve the issue.");
            //}
        }

        private void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            //// here it checks if session is reuired, as
            //// .aspx requires session, and session should be available there
            //// even if you implemented URL Rewritter, or custom IHttp Module
            //if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
            //{
            //    // here is your actual code
            //    // check if session is new one
            //    // or any of your logic
            //    if (Session.IsNewSession || Session.Count < 1 || HttpContext.Current == null || HttpContext.Current.Session == null)
            //    {
            //        // for instance your login page is default.aspx
            //        // it should not be redirected if,
            //        // if the request is for login page (i.e. default.aspx)
            //        if (!Context.Request.Url.AbsoluteUri.ToLower().Contains("userslogin.aspx"))
            //        {
            //            // redirect to your login page
            //            Context.Response.Redirect("~\\UsersLogin.aspx");
            //        }
            //    }
            //}
        }

        private void Session_Start(object sender, EventArgs e)
        {
            //userInfo_UserId   userInfo_Partition   userInfo_DataAreaId    userInfo_UserName  
            // Code that runs when a new session is started  
            //if (Session["userInfo_UserId"] == null || Session["userInfo_UserName"] == null || Session["userInfo_Partition"] == null || Session["userInfo_DataAreaId"] == null)
            //{
            //    //Redirect to Welcome Page if Session is not null
            //    Response.Redirect("~\\Administration\\UsersLogin.aspx", true);
            //}
        }

        private void Session_End(object sender, EventArgs e)
        {
            // Note: The Session_End event is raised only when the sessionstate mode is set to InProc in the Web.config file.
            // If session mode is set to StateServer or SQLServer, the event is not raised.
        }
        protected void Application_PostAuthorizeRequest()
        {
            //HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        /*
            ----------------------------------------------------------------------------------------------------
            Application_Init: Fired when an application initializes or is first called. It's invoked for all HttpApplication object instances.
            Application_Disposed: Fired just before an application is destroyed. This is the ideal location for cleaning up previously used resources.
            Application_Error: Fired when an unhandled exception is encountered within the application.
            Application_Start: Fired when the first instance of the HttpApplication class is created. It allows you to create objects that are accessible by all HttpApplication instances.
            Application_End: Fired when the last instance of an HttpApplication class is destroyed. It's fired only once during an application's lifetime.
            Application_BeginRequest: Fired when an application request is received. It's the first event fired for a request, which is often a page request (URL) that a user enters.
            Application_EndRequest: The last event fired for an application request.
            Application_PreRequestHandlerExecute: Fired before the ASP.NET page framework begins executing an event handler like a page or Web service.
            Application_PostRequestHandlerExecute: Fired when the ASP.NET page framework is finished executing an event handler.
            Applcation_PreSendRequestHeaders: Fired before the ASP.NET page framework sends HTTP headers to a requesting client (browser).
            Application_PreSendContent: Fired before the ASP.NET page framework sends content to a requesting client (browser).
            Application_AcquireRequestState: Fired when the ASP.NET page framework gets the current state (Session state) related to the current request.
            Application_ReleaseRequestState: Fired when the ASP.NET page framework completes execution of all event handlers. This results in all state modules to save their current state data.
            Application_ResolveRequestCache: Fired when the ASP.NET page framework completes an authorization request. It allows caching modules to serve the request from the cache, thus bypassing handler execution.
            Application_UpdateRequestCache: Fired when the ASP.NET page framework completes handler execution to allow caching modules to store responses to be used to handle subsequent requests.
            Application_AuthenticateRequest: Fired when the security module has established the current user's identity as valid. At this point, the user's credentials have been validated.
            Application_AuthorizeRequest: Fired when the security module has verified that a user can access resources.
            Session_Start: Fired when a new user visits the application Web site.
            Session_End: Fired when a user's session times out, ends, or they leave the application Web site.
         */
    }
}
