using PortalIntegration;
using PortalIntegration.HRMApplicationsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSHRApplications_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Applications";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Applications"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }

                bindGrid();
                bindRecGrid();  

            }
        }

        private void bindGrid()
        {
            ESSHRApplications svc = new ESSHRApplications();
            DataTable dt = svc.retrieveAll();

            ////// Use same column names as in GridView (and service result)
            //dt.Columns.Add("Application");
            //dt.Columns.Add("Name");
            //dt.Columns.Add("ApplicantType");
            //dt.Columns.Add("DateOfReceipt");
            //dt.Columns.Add("Status");
            //dt.Columns.Add("CorrespondanceAction");
            //dt.Columns.Add("RecruitmentProject");




            gvApplications.DataSource = dt;
            gvApplications.DataBind();
        }

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
        }

        private long GetRecIdByApplicaton(string application)
        {
            // Call your service to get all applicants
            ESSHRApplications svc = new ESSHRApplications();
            DataTable dt = svc.retrieveAll(); // Implement this to call your retrieveAll() method

            // Find the row that matches the first name
            DataRow[] rows = dt.Select($"ApplicationId = '{application.Replace("'", "''")}'");

            if (rows.Length > 0)
            {
                return Convert.ToInt64(rows[0]["RecId"]); // make sure your DataTable has RecId column
            }
            else
            {
                return 0; // not found
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "OpenCreateApplicant",
                "openPopupPanel('/ESS/HR/ESSHRApplications_Create.aspx',1000);",
                //"openPopupPanel('/ESS/HR/ESSHRApplicant_Create.aspx',1000);",
                true
            );
        }

        private void bindRecGrid()
        {
            ESSHRApplications svc = new ESSHRApplications();
            DataTable dt;

            string recruitmentProject =
                Request.QueryString["RecruitmentProject"];

            if (!string.IsNullOrEmpty(recruitmentProject))
            {
                // 🔑 Call your service method
                dt = svc.retrieveByRecruitment(recruitmentProject);
            }
            else
            {
                dt = svc.retrieveAll();
            }

            gvApplications.DataSource = dt;
            gvApplications.DataBind();
        }

        protected void lnkApplication_Click(object sender, EventArgs e)
        {
            LinkButton lnk = sender as LinkButton;
            if (lnk != null)
            {
                string application = lnk.CommandArgument;

                if (!string.IsNullOrEmpty(application))
                {
                    if (!string.IsNullOrEmpty(application))
                    {
                        // 1️⃣ Get the RecId for this applicant (you need a method for this)
                        long recId = GetRecIdByApplicaton(application); // implement this method

                        // 2️⃣ Store the RecId in session
                        Session["SelectedApplicationRecId"] = recId;
                        // Redirect to the detail page, passing RecruitmentProject as query string
                        Response.Redirect($"~/ESS/HR/ESSHRApplications_Detail.aspx?ApplicationId={Server.UrlEncode(application)}");

                    }
                }
            }
        }


    }
   
}