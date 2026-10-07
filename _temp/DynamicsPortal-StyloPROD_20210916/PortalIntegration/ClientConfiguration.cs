using System;
using System.Configuration;

namespace PortalIntegration
{
    public partial class ClientConfiguration
    {
        public static ClientConfiguration Default { get { return ClientConfiguration.OneBox; } }

        public static ClientConfiguration OneBox = new ClientConfiguration()
        {
            //  You only need to populate this section if you are logging on via a native app.
            //  For Service to Service scenarios in which you e.g. use a service principal you don't need that.
            UriString = "https://ssaxprod.operations.dynamics.com",
            UserName = ConfigurationManager.ConnectionStrings["D365AuthUser"].ToString(),   //"maison.erp@stylo.pk",
            Password = ConfigurationManager.ConnectionStrings["D365AuthPass"].ToString(),   //"Vak51199",            //  Insert the correct password here.

            //  You need this only if you logon via service principal using a client secret.
            //  See: https://docs.microsoft.com/en-us/dynamics365/unified-operations/dev-itpro/data-entities/services-home-page to get more data on how to populate those fields.
            //  You can find that under AAD in the azure portal

            ActiveDirectoryResource = "https://ssaxprod.operations.dynamics.com",//  Don't have a trailing "/"

            ActiveDirectoryTenant = "3119fa5c-44e9-4c57-bcb1-f3f4b39e76b8",          //Directory Id

            ActiveDirectoryClientAppId = "39a77069-abc1-4f84-8307-2e8862b84106",    //
            //  Insert here the application secret when authenticate with AAD by the application
            ActiveDirectoryClientAppSecret = "",

            AzureAuthEndPoint = "https://login.windows.net",
            // Change TLS version of HTTP request from the client here
            // Ex: TLSVersion = "1.2"
            // Leave it empty if want to use the default version
            TLSVersion = "1.2",
        };

        public string TLSVersion { get; set; }
        public string UriString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ActiveDirectoryResource { get; set; }
        public String ActiveDirectoryTenant { get; set; }
        public String ActiveDirectoryClientAppId { get; set; }
        public string ActiveDirectoryClientAppSecret { get; set; }

        public string AzureAuthEndPoint { get; set; }
        //   public string AADClientSecret { get; set; }
    }
}
