using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class UploadApk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticationHelper.AuthenticationHelper authenticationHelper = new AuthenticationHelper.AuthenticationHelper();
            bool isPageAuthorizated = authenticationHelper.pageAuthentication("UploadApplication");
            //isPageAuthorizated = true;
            if (!isPageAuthorizated)
                return;
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fileUploadApk.HasFile)
            {
                // Ensure only APK is allowed
                string extension = Path.GetExtension(fileUploadApk.FileName);
                if (extension.Equals(".apk", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        string savePath = Server.MapPath("~/App_Data/MobileApp.apk");

                        // Save the uploaded file, overwriting old one
                        fileUploadApk.SaveAs(savePath);

                        lblStatus.Text = "APK uploaded successfully. It is now available for download.";
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error uploading APK: " + ex.Message;
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    lblStatus.Text = "Please upload a valid .apk file.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                lblStatus.Text = "No file selected.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}