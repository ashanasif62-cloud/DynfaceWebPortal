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
            //UriString = "https://fawad-dev-0136f6b0c5cde38b77devaos.axcloud.dynamics.com/",
            UriString = "https://maison-uat.sandbox.operations.dynamics.com/",
            //UriString = "",


            //UserName = ConfigurationManager.ConnectionStrings["D365AuthUser"].ToString(),   //"maison.erp@stylo.pk",

            //Password = ConfigurationManager.ConnectionStrings["D365AuthPass"].ToString(),   //"Vak51199",            //  Insert the correct password here.

            //  You need this only if you logon via service principal using a client secret.

            //  See: https://docs.microsoft.com/en-us/dynamics365/unified-operations/dev-itpro/data-entities/services-ho… to get more data on how to populate those fields.

            //  You can find that under AAD in the azure portal

            //      ActiveDirectoryResource = "https://fawad-dev-0136f6b0c5cde38b77devaos.axcloud.dynamics.com/",//  Don't have a trailing "/"
            ActiveDirectoryResource = "https://maison-uat.sandbox.operations.dynamics.com/",//  Don't have a trailing "/"
            //ActiveDirectoryResource = "",//  Don't have a trailing "/"

            ActiveDirectoryTenant = "5df08255-a152-4fbc-a174-0d8a182aa594",          //Directory Id
            //ActiveDirectoryTenant = "",          //Directory Id

            //ActiveDirectoryTenant = "",          //Directory Id

            ActiveDirectoryClientAppId = "ef00dde4-61c3-4bc5-b9da-bb30c6ef7e20",    //
            //ActiveDirectoryClientAppId = "44c7cff3-bed3-4644-bed9-36c7c0a13dbe",
            //ActiveDirectoryClientAppId = "",


            //  Insert here the application secret when authenticate with AAD by the application
            ActiveDirectoryClientAppSecret = "hY98Q~oTlaINsIkH1KBeIHDcnHmyucLg0bIHbcrA",
            //ActiveDirectoryClientAppSecret = "",

            AzureAuthEndPoint = "https://login.windows.net",

            // Change TLS version of HTTP request from the client here

            // Ex: TLSVersion = "1.2"

            // Leave it empty if want to use the default version

            TLSVersion = "1.2",

            AzureADRedirectURI = "https://ess.ecbl.pk:8081/AuthCallback.aspx",
           // AzureADRedirectURI = "http://localhost:38474/AuthCallback.aspx",
            //AzureADRedirectURI = "http://116.58.39.61:8081/AuthCallback.aspx",
            //116.58.39.61:8081
            //AzureADRedirectURI = "http://localhost:8080/AuthCallback.aspx",
           // AzureADRedirectURI = "http://localhost:8081/AuthCallback.aspx",

            // AzureADRedirectURI = "https://dynaface-app.azurewebsites.net/AuthCallback.aspx",

            // GoogleRedirectURI = "http://localhost:38474/GoogleLoginCallback.aspx",
            GoogleRedirectURI = "http://localhost:8080/GoogleLoginCallback.aspx",

           // GoogleRedirectURI = "https://dynaface-app.azurewebsites.net/GoogleLoginCallback.aspx",

            connectWithSQL = false,

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

        public string AzureADRedirectURI { get; set; }

        public string GoogleRedirectURI { get; set; }

        public bool connectWithSQL { get; set; }

    }

}

 