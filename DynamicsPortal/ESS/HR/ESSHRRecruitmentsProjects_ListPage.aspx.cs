using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using System.Windows.Controls;

namespace DynamicsPortal.ESS.HR
{
    public partial class HRRecruitmentsProjects_ListPage : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Recruitment Projects";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Recruitment Projects"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }

                BindGrid();
            }

        }

        private void BindGrid()
        {
            ESSHRRecruitment svc = new ESSHRRecruitment();
            DataTable dt = svc.retrieveAll();

            //// Use same column names as in GridView (and service result)
            //dt.Columns.Add("RecruitmentProjects");
            //dt.Columns.Add("Description");
            //dt.Columns.Add("Recruiter");
            //dt.Columns.Add("ProjectStatus");
            //dt.Columns.Add("OpenDate");
            //dt.Columns.Add("ApplicationDeadline");
            //dt.Columns.Add("CloseDate");
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (row["StartDate"] != DBNull.Value)
            //        row["StartDate"] = Convert.ToDateTime(row["StartDate"]).Date;

            //    if (row["ApplicationDeadline"] != DBNull.Value)
            //        row["ApplicationDeadline"] = Convert.ToDateTime(row["ApplicationDeadline"]).Date;

            //    if (row["EndDate"] != DBNull.Value)
            //        row["EndDate"] = Convert.ToDateTime(row["EndDate"]).Date;
            //}



            gvRecruitmentProjects.DataSource = dt;
            gvRecruitmentProjects.DataBind();
        }
        protected void lnkPurchReqId_Click(object sender, EventArgs e)
        {
            LinkButton lnk = sender as LinkButton;
            if (lnk != null)
            {
                string projectId = lnk.CommandArgument;

                if (!string.IsNullOrEmpty(projectId))
                {
                    // Redirect to the detail page, passing RecruitmentProject as query string
                    Response.Redirect($"~/ESS/HR/ESSHRRecruitmentDetailForm.aspx?RecruitmentProject={projectId}");
                }
            }
        }


        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            // Allow only ONE checkbox to be selected
            foreach (GridViewRow row in gvRecruitmentProjects.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chk_SelectSingle");
                if (chk != sender)
                {
                    chk.Checked = false;
                }
            }

            // Get selected row
            CheckBox selectedChk = (CheckBox)sender;
            GridViewRow selectedRow = (GridViewRow)selectedChk.NamingContainer;

            // Get RecruitmentProject from LinkButton
            LinkButton lnkRecruitmentProject =
                (LinkButton)selectedRow.FindControl("lnkRecruitmentProject");

            if (lnkRecruitmentProject != null)
            {
                // Store selected Recruitment Project in Session
                Session["SelectedRecruitmentProject"] =
                    lnkRecruitmentProject.CommandArgument;
            }
        }
        protected void btnApplication_Click(object sender, EventArgs e)
        {
            if (Session["SelectedRecruitmentProject"] == null)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "NoSelection",
                    "alert('Please select a recruitment project first.');",
                    true
                );
                return;
            }

            string recruitmentProject =
                Session["SelectedRecruitmentProject"].ToString();

            Response.Redirect(
                $"~/ESS/HR/ESSHRApplications_ListPage.aspx?RecruitmentProject={Server.UrlEncode(recruitmentProject)}"
            );
        }


    }
}