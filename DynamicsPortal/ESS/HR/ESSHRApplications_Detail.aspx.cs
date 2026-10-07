using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HCMApplicantsSvcReference;
using PortalIntegration.HRMApplicationsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSHRApplications_Detail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Get the 'Applicant' parameter from the query string
                string application = Request.QueryString["ApplicationId"];

                if (string.IsNullOrEmpty(application))
                {
                    // Optional: redirect back to list page if no applicant is specified
                    Response.Redirect("~/ESS/HR/ESSHRApplications_ListPage.aspx");
                    return;
                }

                // Set page title
                Page.Title = "Applications Detail";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Applications Detail"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }
                BindRecruitmentProject();
                BindDepartment();
                Bindjob();
                BindGeneral(application);
            }

            }

        private void BindGeneral(string application)
        {
            try
            {
                // Call your service to get the project details
                ESSHRApplications svc = new ESSHRApplications();
                DataTable dt = svc.retrieveAll();

                // Filter the DataTable to the selected project
                DataRow[] rows = dt.Select($"ApplicationId = '{application}'");
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];

                    // Bind values to labels
                    txtApplication.Text = row["ApplicationId"].ToString();
                    hdnApplication.Value = row["ApplicationId"].ToString();
                    txtApplicantId.Text = row["ApplicantId"].ToString();
                    txtApplicantType.Text = row["applicantType"].ToString();
                
                    // txtDepartment.Text = row["Organization"].ToString();

                    //txtPersonalTitle.Text = row["Recruiter"].ToString();
                   txtApplicantName.Text = row["FirstName"].ToString();
                    txtCreatedSource.Text = row["CreatedSource"].ToString();

                    txtDateOfReceipt.Text = row["DateOfReception"].ToString();

                    ddlRecruitmentProject.SelectedValue = row["RecruitmentProject"].ToString();
                    //txtPersonalSuffix.Text = row["PersonalSuffix"].ToString();
                    //ddlMedia.SelectedValue = row["Media"].ToString();
                    //ddlReasonCode.Text = row["Initials"].ToString();
                    ddlDepartment.SelectedValue = row["Department"].ToString();
                    txtExpireDate.Text = row["ExpireDate"].ToString();
                    txtStatus.Text = row["Status"].ToString();
                    ddlJob.SelectedValue = row["JobId"].ToString();
                    //ddlCorrespondenceAction.SelectedValue= row["CorrespondenceAction"].ToString();
                    //ddlRating.Text = row["Rating"].ToString();
                    //txtStartDateTime.Text = row["NameSequenceDisplayAs"].ToString();
                    //txtTravelCost.Text = row["CurrentJobTitle"].ToString();
                    //txtLodgingCost.Text = row["EducationLevelId"].ToString();
                    //txtOtherCost.Text = row["EducationLevelId"].ToString();
                    //ddlContact.Text = row["EducationLevelId"].ToString();
                    // txtApplications.Text = row["HiringManager"].ToString();
                    //txtAlternativeContact.Text = row["AlternativeContact"].ToString();

                    //txtReqApprovedOn.Text = FormatDate(row["RequisitionApprovalDate"]);
                    //txtOpenDate.Text = FormatDate(row["StartDate"]);
                    //txtApplicationDeadline.Text = FormatDate(row["ApplicationDeadline"]);
                    //txtCloseDate.Text = FormatDate(row["EndDate"]);
                    //txtEstimatedStartDate.Text = FormatDate(row["EstimatedStartDate"]);

                    // txtDisplayOnESS.Text = row["DisplayOnESS"].ToString();
                }
            }
            catch (Exception ex)
            {
                // Optional: log error or show a message
                txtApplicantId.Text = "Error loading Applicant details";
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Create and populate the contract
                HRMApplicationContract applicationContract = new HRMApplicationContract();

                // Fill the contract fields (replace with your actual form controls)
                // Applicant identifier
                //Hn to yahan likho atachment k logic
                applicationContract.ApplicantId = txtApplicantId.Text;
                applicationContract.ApplicationId = txtApplication.Text;
                applicationContract.JobId = ddlJob.SelectedValue;
                applicationContract.FirstName = txtApplicantName.Text;
                applicationContract.applicantType = txtApplicantType.Text;
                applicationContract.ExpireDate = DateTime.ParseExact(txtExpireDate.Text.Trim(),"yyyy-MM-dd", CultureInfo.InvariantCulture);
                applicationContract.DateOfReception = DateTime.ParseExact(txtDateOfReceipt.Text.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);


                applicationContract.RecruitmentProject = ddlRecruitmentProject.SelectedValue;


                // 2️⃣ Call the service to update the applicant
                ESSHRApplications svc = new ESSHRApplications();
                bool isUpdated = svc.updateApplicant(applicationContract);

                // 3️⃣ Show result to user
                if (isUpdated)
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "UpdateSuccess",
                        "alert('Applicant updated successfully!'); " +
                        "if (typeof closePopupPanel === 'function') closePopupPanel();",
                        true
                    );
                }
                else
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "UpdateFailed",
                        "alert('Failed to update applicant.');",
                        true
                    );
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "UpdateError",
                    "alert('An error occurred while updating the applicant.');",
                    true
                );
            }
        }

        private bool BindRecruitmentProject(string selectedValue = "")
        {
            ESSHRApplications svc = new ESSHRApplications();
            DataTable dt = svc.retrieveRecruitmentProject();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlRecruitmentProject.DataSource = dt;
                ddlRecruitmentProject.DataTextField = "RecruitmentProject";   // what user sees
                ddlRecruitmentProject.DataValueField = "RecruitmentProject";  // underlying value
                ddlRecruitmentProject.DataBind();

                // Insert empty option at the top
                ddlRecruitmentProject.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlRecruitmentProject.Items.FindByValue(selectedValue) != null)
            {
                ddlRecruitmentProject.SelectedValue = selectedValue;
            }
            else
            {
                ddlRecruitmentProject.SelectedIndex = 0; // keep it empty
            }

            return true;
        }

        private bool BindDepartment(string selectedValue = "")
        {
            ESSHRApplications svc = new ESSHRApplications();
            DataTable dt = svc.retrieveDepartment();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataTextField = "Department";   // what user sees
                ddlDepartment.DataValueField = "DepartmentId";  // underlying value
                ddlDepartment.DataBind();

                // Insert empty option at the top
                ddlDepartment.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlDepartment.Items.FindByValue(selectedValue) != null)
            {
                ddlDepartment.SelectedValue = selectedValue;
            }
            else
            {
                ddlDepartment.SelectedIndex = 0; // keep it empty
            }

            return true;
        }
        private bool Bindjob(string selectedValue = "")
        {
            ESSHRApplications svc = new ESSHRApplications();
            DataTable dt = svc.retrieveJob();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlJob.DataSource = dt;
                ddlJob.DataTextField = "JobId";   // what user sees
                ddlJob.DataValueField = "JobId";  // underlying value
                ddlJob.DataBind();

                // Insert empty option at the top
                ddlJob.Items.Insert(0, new ListItem(""));
            }

            // Set selected value if provided and exists
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlJob.Items.FindByValue(selectedValue) != null)
            {
                ddlJob.SelectedValue = selectedValue;
            }
            else
            {
                ddlJob.SelectedIndex = 0; // keep it empty
            }

            return true;
        }
        //Create ya Save k button kahan ha??
        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    if (!fuAttachment.HasFile)
        //    {
        //        lblMessage.Text = "Please select a file.";
        //        return;
        //    }

        //    try
        //    {
        //        // 🔹 Read file bytes
        //        byte[] fileBytes = fuAttachment.FileBytes;

        //        // 🔹 Convert to Base64
        //        string base64String = Convert.ToBase64String(fileBytes);

        //        // 🔹 File info
        //        string fileName = Path.GetFileNameWithoutExtension(fuAttachment.FileName);
        //        string fileExt = Path.GetExtension(fuAttachment.FileName);

        //        // 🔹 Application Id (hidden field / textbox)
        //        string applicationId = txtApplicationId.Text; // or hidden field

        //        // 🔹 Call service
        //        HRMApplication svc = new HRMApplication();
        //        bool result = svc.getAttachment(
        //            applicationId,
        //            fileName,
        //            fileExt,
        //            base64String
        //        );

        //        lblMessage.Text = result
        //            ? "File uploaded successfully."
        //            : "File upload failed.";
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = ex.Message;
        //    }
        //}

        //protected void btnUpload_Click(object sender, EventArgs e)
        //{
        //    if (!fuAttachment.HasFile)
        //    {
        //        ClientScript.RegisterStartupScript(
        //            this.GetType(),
        //            "alert",
        //            "alert('Please select a file.');",
        //            true);
        //        return;
        //    }

        //    try
        //    {
        //        byte[] fileBytes = fuAttachment.FileBytes;
        //        string base64String = Convert.ToBase64String(fileBytes);

        //        string fileName = Path.GetFileNameWithoutExtension(fuAttachment.FileName);
        //        string fileExt = Path.GetExtension(fuAttachment.FileName);

        //        // 🔹 Populate contract ONCE
        //        HRMApplicationContract contract = new HRMApplicationContract();
        //        contract.ApplicationId = txtApplication.Text;

        //        // REQUIRED application fields
        //        contract.RecruitmentProject = ddlRecruitmentProject.SelectedValue;
        //        contract.JobId = ddlJob.SelectedValue;
        //        contract.ApplicantId = txtApplicantId.Text;

        //        // 🔹 Attachment fields
        //        contract.FileName = fileName;
        //        contract.FileExt = fileExt;
        //        contract.FileBase64String = base64String;

        //        ESSHRApplications svc = new ESSHRApplications();
        //        long recId = svc.CreateApplicant(contract);

        //        string msg = recId > 0
        //            ? "Application created and file attached successfully."
        //            : "Operation failed.";

        //        ClientScript.RegisterStartupScript(
        //            this.GetType(),
        //            "alert",
        //            $"alert('{msg}');",
        //            true);
        //    }
        //    catch (Exception ex)
        //    {
        //        ClientScript.RegisterStartupScript(
        //            this.GetType(),
        //            "alert",
        //            $"alert('{ex.Message}');",
        //            true);
        //    }
        //}



    }


}