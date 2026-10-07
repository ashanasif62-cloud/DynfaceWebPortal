using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSHRApplicants_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Applicant";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Applicant"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }

                BindGrid();
            }

        }
        private void BindGrid()
        {
            HCMApplicants svc = new HCMApplicants();

            DataTable dt = svc.retrieveAll();

            //// Use same column names as in GridView (and service result)
            //dt.Columns.Add("Name");
            //dt.Columns.Add("Applicant");
            //dt.Columns.Add("ApplicantType");
            //dt.Columns.Add("HighestDegree");
           



            gvApplicants.DataSource = dt;
            gvApplicants.DataBind();
        }
        private long GetRecIdByFirstName(string name)
        {
            // Call your service to get all applicants
            HCMApplicants svc = new HCMApplicants();
            DataTable dt = svc.retrieveAll(); // Implement this to call your retrieveAll() method

            // Find the row that matches the first name
            DataRow[] rows = dt.Select($"FirstName = '{name.Replace("'", "''")}'");

            if (rows.Length > 0)
            {
                return Convert.ToInt64(rows[0]["RecId"]); // make sure your DataTable has RecId column
            }
            else
            {
                return 0; // not found
            }
        }


        protected void lnkFirstName_Click(object sender, EventArgs e)
        {
            LinkButton lnk = sender as LinkButton;
            if (lnk != null)
            {
                string name = lnk.CommandArgument;

                if (!string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        // 1️⃣ Get the RecId for this applicant (you need a method for this)
                        long recId = GetRecIdByFirstName(name); // implement this method

                        // 2️⃣ Store the RecId in session
                        Session["SelectedApplicantRecId"] = recId;
                        // Redirect to the detail page, passing RecruitmentProject as query string
                        Response.Redirect($"~/ESS/HR/ESSHRApplicant_DetailForm.aspx?FirstName={Server.UrlEncode(name)}");

                    }
                }
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "OpenCreateApplicant",
                "openPopupPanel('/ESS/HR/ESSHRApplicant_Create.aspx',1000);",
                true
            );
        }


        //protected void btnNew_Click(object sender, EventArgs e)
        //{
        //    string applicant = string.Empty;
        //    string applicantType = string.Empty;
        //    string displayAs = string.Empty;
        //    long recId = 0;

        //    foreach (GridViewRow row in gvApplicants.Rows)
        //    {
        //        CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;

        //        if (chk != null && chk.Checked)
        //        {
        //           // Label lblApplicant = row.FindControl("lblApplicant") as Label;
        //            Label lblApplicantType = row.FindControl("lblApplicantType") as Label;
        //            Label lbldisplayas = row.FindControl("lbldisplayas") as Label;
        //            Label lblRecId = row.FindControl("lblRecId") as Label;

        //            //if (lblApplicant != null)
        //            //    applicant = lblApplicant.Text;

        //            if (lblApplicantType != null)
        //                applicantType = lblApplicantType.Text;

        //            if (lbldisplayas != null)
        //                displayAs = lbldisplayas.Text;

        //            if (lblRecId != null && long.TryParse(lblRecId.Text, out long parsedRecId))
        //                recId = parsedRecId;

        //            break; // 🔑 single selection only
        //        }
        //    }

        //    // Validation
        //    if (string.IsNullOrEmpty(applicant))
        //    {
        //        ScriptManager.RegisterStartupScript(
        //            this,
        //            this.GetType(),
        //            "SelectApplicantAlert",
        //            "alert('Please select an applicant first.');",
        //            true
        //        );
        //        return;
        //    }

        //    // Store values in Session
        //    Session["Applicant"] = applicant;
        //    Session["applicantType"] = applicantType;
        //    Session["NameSequenceDisplayAs"] = displayAs;
        //    Session["ApplicantRecId"] = recId;

        //    // Open create form in popup
        //    ScriptManager.RegisterStartupScript(
        //        this,
        //        this.GetType(),
        //        "OpenCreateApplicant",
        //        "openPopupPanel('/ESS/HR/ESSHRApplicant_Create.aspx',1000);",
        //        true
        //    );
        //}


        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
        }
    }
}