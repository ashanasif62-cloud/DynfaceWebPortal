using PortalIntegration;
using PortalIntegration.ESSHRRecruitmentSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSHRRecruitmentDetailForm : MainForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Recruitment Projects Detail";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Recruitment Projects Detail"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }
                // Get RecruitmentProject ID from query string
                string projectId = Request.QueryString["RecruitmentProject"];

                if (!string.IsNullOrEmpty(projectId))
                {
                    // Load data for this project
                    BindProjectDetails(projectId);
                }
            }


        }

        private void BindProjectDetails(string projectId)
        {
            try
            {
                // Call your service to get the project details
                ESSHRRecruitment svc = new ESSHRRecruitment();
                DataTable dt = svc.retrieveAll();

                // Filter the DataTable to the selected project
                DataRow[] rows = dt.Select($"RecruitmentProject = '{projectId}'");
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];

                    // Bind values to labels
                    txtRecruitmentProject.Text = row["RecruitmentProject"].ToString();
                    hdnRecruitmentProject.Value = row["RecruitmentProject"].ToString();
                    txtDescription.Text = row["Description"].ToString();
                   // txtDepartment.Text = row["Organization"].ToString();

                    txtRecruiter.Text = row["Recruiter"].ToString();
                    txtProjectStatus.Text = row["Status"].ToString();
                    txtJob.Text = row["Job"].ToString();

                    txtNumberOfOpenings.Text = row["Qty"].ToString();

                    txtHiringManager.Text = row["HiringManager"].ToString();
                    //txtAlternativeContact.Text = row["AlternativeContact"].ToString();

                    txtReqApprovedOn.Text = FormatDate(row["RequisitionApprovalDate"]);
                    txtOpenDate.Text = FormatDate(row["StartDate"]);
                    txtApplicationDeadline.Text = FormatDate(row["ApplicationDeadline"]);
                    txtCloseDate.Text = FormatDate(row["EndDate"]);
                    txtEstimatedStartDate.Text = FormatDate(row["EstimatedStartDate"]);

                   // txtDisplayOnESS.Text = row["DisplayOnESS"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Optional: log error or show a message
                txtRecruitmentProject.Text = "Error loading project details";
            }
        }

        private string FormatDate(object dateObj)
        {
            if (dateObj != null && dateObj != DBNull.Value)
            {
                DateTime dt;
                if (DateTime.TryParse(dateObj.ToString(), out dt))
                {
                    return dt.ToString("yyyy-MM-dd");
                }
            }
            return "";
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            ESSHRRecruitment svc = new ESSHRRecruitment();
            HRMRecruitingProjectContract contract = new HRMRecruitingProjectContract();

            // 🔑 ORIGINAL Recruitment Project (FROM LIST PAGE)
            contract.RecruitmentProject = hdnRecruitmentProject.Value;

            // ✏️ Editable fields
            contract.Description = txtDescription.Text;
            contract.Recruiter = txtRecruiter.Text;
            contract.Job = txtJob.Text;
            contract.Qty = Convert.ToInt32(txtNumberOfOpenings.Text);
            contract.HiringManager = txtHiringManager.Text;

            bool result = svc.updateByRecruitingId(contract);

            if (result)
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(),
                    "success",
                    "alert('Recruitment project updated successfully');",
                    true
                );
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ESSHRRecruitment svc = new ESSHRRecruitment();
            HRMRecruitingProjectContract contract = new HRMRecruitingProjectContract();

            // 🔑 Recruitment Project ID from hidden field
            contract.RecruitmentProject = hdnRecruitmentProject.Value;

            if (!string.IsNullOrEmpty(contract.RecruitmentProject))
            {
                bool result = svc.deleteByRecruitingId(contract);

                if (result)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(),
                        "success",
                        "alert('Recruitment project deleted successfully');",
                        true
                    );
                }
                else
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(),
                        "error",
                        "alert('Failed to delete recruitment project.');",
                        true
                    );
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(),
                    "error",
                    "alert('Recruitment project ID is missing.');",
                    true
                );
            }
        }


    }
}