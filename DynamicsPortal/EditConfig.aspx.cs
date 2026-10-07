using AuthenticationHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Security;

namespace YourNamespace
{
    public partial class EditConfig : System.Web.UI.Page
    {
        private string ConfigPath => Server.MapPath("~/App_Data/ClientConfiguration.json");

        public class MinimalConfig
        {
            public string Title { get; set; }
            public string ActiveDirectoryResource { get; set; }
            public string ActiveDirectoryTenant { get; set; }
            public string ActiveDirectoryClientAppId { get; set; }
            public string ActiveDirectoryClientAppSecret { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadConfig();
            }
        }

        private void LoadConfig()
        {
            if (File.Exists(ConfigPath))
            {
                var json = File.ReadAllText(ConfigPath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    var config = JsonConvert.DeserializeObject<MinimalConfig>(json);
                    if (config != null)
                    {
                        AuthenticationHelper.AuthenticationHelper authenticationHelper = new AuthenticationHelper.AuthenticationHelper();
                        bool isPageAuthorizated = authenticationHelper.pageAuthentication("EditConfiguration");
                        //isPageAuthorizated = true;
                        if (!isPageAuthorizated)
                            return;

                        txtTitle.Text = config.Title ?? "";
                        txtActiveDirectoryResource.Text = config.ActiveDirectoryResource ?? "";
                        txtActiveDirectoryTenant.Text = config.ActiveDirectoryTenant ?? "";
                        txtActiveDirectoryClientAppId.Text = config.ActiveDirectoryClientAppId ?? "";
                        txtActiveDirectoryClientAppSecret.Text = config.ActiveDirectoryClientAppSecret ?? "";
                    }
                }
            }
        }

        protected async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var newConfig = new MinimalConfig
                {
                    Title = txtTitle.Text.Trim(),
                    ActiveDirectoryResource = txtActiveDirectoryResource.Text.Trim(),
                    ActiveDirectoryTenant = txtActiveDirectoryTenant.Text.Trim(),
                    ActiveDirectoryClientAppId = txtActiveDirectoryClientAppId.Text.Trim(),
                    ActiveDirectoryClientAppSecret = txtActiveDirectoryClientAppSecret.Text.Trim()
                };

                // ✅ First test credentials
                bool isValid = await TestConnectionAsync(
                    newConfig.ActiveDirectoryTenant,
                    newConfig.ActiveDirectoryClientAppId,
                    newConfig.ActiveDirectoryClientAppSecret,
                    newConfig.ActiveDirectoryResource
                );

                if (!isValid)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "❌ Invalid Configuration. Connection to D365FO failed.";
                    return;
                }

                // ✅ If valid, save config as single record
                string json = JsonConvert.SerializeObject(newConfig, Formatting.Indented);
                File.WriteAllText(ConfigPath, json);

                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "✅ Configuration saved successfully.";
                AuthenticateUser authenticateUser = new AuthenticateUser();
                authenticateUser.clearAutentication();
                FormsAuthentication.RedirectToLoginPage();
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "❌ Error: " + ex.Message;
            }
        }

        private async Task<bool> TestConnectionAsync(string tenantId, string clientId, string clientSecret, string resource)
        {
            try
            {
                string authority = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";

                using (var client = new HttpClient())
                {
                    var body = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("client_id", clientId),
                        new KeyValuePair<string, string>("client_secret", clientSecret),
                        new KeyValuePair<string, string>("resource", resource),
                        new KeyValuePair<string, string>("grant_type", "client_credentials")
                    };

                    var content = new FormUrlEncodedContent(body);
                    var response = await client.PostAsync(authority, content);
                    string result = await response.Content.ReadAsStringAsync();

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
