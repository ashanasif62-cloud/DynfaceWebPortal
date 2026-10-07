using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class DownloadApk : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/App_Data/MobileApp.apk");
            string fileName = "MobileApp.apk";

            if (File.Exists(filePath))
            {
                Response.Clear();
                Response.ContentType = "application/vnd.android.package-archive";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.TransmitFile(filePath);
                Response.End();
            }
            else
            {
                Response.Clear();
                Response.Write("<script>alert('APK not available'); window.history.back();</script>");
                Response.End();
            }
        }
    }
}