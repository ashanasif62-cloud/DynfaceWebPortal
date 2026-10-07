using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HCMApplicantsSvcReference;
using PortalIntegration.HRMApplicationsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSHRApplications_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {


                Page.Title = "Application Create";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Create"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }
                // txtApplicantType.Text = "External applicant";

                bindApplicantId();
                bindApplicationId();
                BindGeneral();
                BindRecruitmentProject();
                BindDepartment();
                Bindjob();


            }

        }

        private void bindApplicantId()
        {
            ESSHRApplications svc = new ESSHRApplications();

            // This service should return the next/new Applicant
            DataTable dt = svc.retrieveApplicantId();

            if (dt != null && dt.Rows.Count > 0)
            {
                txtApplicantId.Text = dt.Rows[0]["ApplicantId"].ToString();
            }
        }

        private void bindApplicationId()
     {
            ESSHRApplications svc = new ESSHRApplications();

            // This service should return the next/new Applicant
            DataTable dt = svc.retrieveApplicationId();

            if (dt != null && dt.Rows.Count > 0)
            {
                txtApplication.Text = dt.Rows[0]["ApplicationId"].ToString();
            }
        }


        private void BindGeneral()
        {
            try
            {
                ESSHRApplications svc = new ESSHRApplications();
                DataTable dt = svc.retrieveAll();

                if (dt == null || dt.Rows.Count == 0)
                    return;

                // 🔹 Take the first row (no filtering)
                DataRow row = dt.Rows[0];

                txtApplicantType.Text = row["applicantType"].ToString();
               // txtApplicantName.Text = row["FirstName"].ToString();
                txtCreatedSource.Text = row["CreatedSource"].ToString();

                // ===================== DATES =====================
                txtDateOfReceipt.Text = FormatDate(row["DateOfReception"]);
                txtExpireDate.Text = FormatDate(row["ExpireDate"]);
                txtStartDateTime.Text = FormatDateTime(row["StartDateTime"]);

                // ===================== DROPDOWNS =====================
                //SetSelectedValue(ddlRecruitmentProject, row["RecruitmentProject"]);
                //SetSelectedValue(ddlMedia, row["Media"]);
                //SetSelectedValue(ddlReasonCode, row["ReasonCode"]);
                //SetSelectedValue(ddlDepartment, row["Department"]);
                //SetSelectedValue(ddlJob, row["JobId"]);
                //SetSelectedValue(ddlCorrespondenceAction, row["CorrespondenceAction"]);
                //SetSelectedValue(ddlRating, row["Rating"]);
                //SetSelectedValue(ddlContact, row["Contact"]);

                // ===================== STATUS =====================
                txtStatus.Text = row["Status"].ToString();

                // ===================== COSTS =====================
                //txtTravelCost.Text = FormatDecimal(row["TravelCost"]);
                //txtLodgingCost.Text = FormatDecimal(row["LodgingCost"]);
                //txtOtherCost.Text = FormatDecimal(row["OtherCost"]);
            }
            catch (Exception)
            {
                txtApplicantId.Text = "Error loading application details";
            }
        }
        private string FormatDateTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            DateTime dt;
            return DateTime.TryParse(value.ToString(), out dt)
                ? dt.ToString("yyyy-MM-ddTHH:mm")
                : string.Empty;
        }


        private string FormatDate(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            DateTime dt;
            return DateTime.TryParse(value.ToString(), out dt)
                ? dt.ToString("yyyy-MM-dd")
                : string.Empty;
        }
        //protected void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // 1️⃣ Create and populate the contract
        //        HRMApplicationContract applicationContract = new HRMApplicationContract();

        //        // General Information
        //        //applicantContract.parmApplicant(txtApplicant.Text);
        //        /* applicantContract.parmApplicantType(txtApplicantType.Text)*/



        //        // Name details

        //        applicationContract.FirstName = txtApplicantName.Text;
        //        applicationContract.RecruitmentProject = txtRecruitment.Text;
        //        applicationContract.JobId = TxtJob.Text;
        //        applicationContract.ApplicantId = txtApplicantId.Text;


        //        // 2️⃣ Call the service to create the applicant
        //        ESSHRApplications svc = new ESSHRApplications();
        //        long recId = svc.CreateApplicant(applicationContract);

        //        if (recId > 0)
        //        {
        //            // Store RecId in session for later use
        //            Session["SelectedApplicantRecId"] = recId;

        //            // Show success message and close popup
        //            ScriptManager.RegisterStartupScript(
        //        this,
        //        this.GetType(),
        //        "ClosePopup",
        //        "alert('Application created successfully!'); " +
        //        "if (typeof closePopupPanel === 'function') closePopupPanel();",
        //        true
        //    );
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(
        //                this,
        //                this.GetType(),
        //                "CreateFailed",
        //                "alert('Failed to create application.');",
        //                true
        //            );
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);

        //        ScriptManager.RegisterStartupScript(
        //            this,
        //            this.GetType(),
        //            "CreateError",
        //            "alert('An error occurred while creating the applicant.');",
        //            true
        //        );
        //    }
        //}

        //protected void btnSave_Click(object sender, EventArgs e)
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
        //        string fileType = fuAttachment.PostedFile.ContentType;



        //        string fileName = Path.GetFileNameWithoutExtension(fuAttachment.FileName);
        //        string fileExt = Path.GetExtension(fuAttachment.FileName);

        //        HCMApplicantContract applicantContract = new HCMApplicantContract();


        //        applicantContract.FirstName = txtApplicantName.Text;
        //        applicantContract.Applicant = txtApplicantId.Text;


        //        // ---------- Call Applicant Service ----------
        //        HCMApplicants applicantSvc = new HCMApplicants();
        //        long applicantRecId = applicantSvc.CreateApplicant(applicantContract);

        //        if (applicantRecId <= 0)
        //            throw new Exception("Applicant creation failed.");

        //        HRMApplicationContract applicationContract = new HRMApplicationContract();

        //        applicationContract.ApplicationId = txtApplication.Text;
        //        applicationContract.ApplicantId = txtApplicantId.Text; // generated earlier

        //        applicationContract.RecruitmentProject = txtRecruitment.Text;
        //        applicationContract.JobId = ddlJob.SelectedValue;
        //         applicationContract.ExpireDate = DateTime.ParseExact(txtExpireDate.Text,"yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture);
        //       applicationContract.DateOfReception = DateTime.ParseExact(txtDateOfReceipt.Text,"yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture);



        //        // ---------- Call Application Service ----------
        //        ESSHRApplications applicationSvc = new ESSHRApplications();
        //        applicationContract.FileName = fileName;
        //        applicationContract.FileExt = fileExt;
        //        applicationContract.FileBase64String = base64String;

        //        long applicationRecId = applicationSvc.CreateApplicant(applicationContract);

        //        if (applicationRecId <= 0)
        //            throw new Exception("Application creation failed.");



        //        ScriptManager.RegisterStartupScript(
        //            this,
        //            this.GetType(),
        //            "CreateSuccess",
        //            "alert('Applicant and Application created successfully!'); " +
        //            "if (typeof closePopupPanel === 'function') closePopupPanel();",
        //            true
        //        );
        //    }
        //    catch (Exception ex)
        //    {
        //        SysErrorLog objErrorLog = new SysErrorLog();
        //        var currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
        //        string currentMethodName = currentMethod.DeclaringType.FullName;
        //        objErrorLog.write(currentMethodName, ex);

        //        ScriptManager.RegisterStartupScript(
        //            this,
        //            this.GetType(),
        //            "CreateError",
        //            "alert('An error occurred while saving Applicant/Application.');",
        //            true
        //        );
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!fuAttachment.HasFile)
            {
                ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "alert",
                    "alert('Please select a file.');",
                    true);
                return;
            }

            try
            {
                byte[] fileBytes = fuAttachment.FileBytes;
                string base64String = Convert.ToBase64String(fileBytes);
                string fileType = fuAttachment.PostedFile.ContentType;

                string fileName = Path.GetFileNameWithoutExtension(fuAttachment.FileName);
                string fileExt = Path.GetExtension(fuAttachment.FileName);

                // ---------- Applicant ----------
                HCMApplicantContract applicantContract = new HCMApplicantContract();
                applicantContract.FirstName = txtApplicantName.Text;
                applicantContract.Applicant = txtApplicantId.Text;

                HCMApplicants applicantSvc = new HCMApplicants();
                long applicantRecId = applicantSvc.CreateApplicant(applicantContract);

                if (applicantRecId <= 0)
                    throw new Exception("Applicant creation failed.");

                // ---------- Application ----------
                HRMApplicationContract applicationContract = new HRMApplicationContract();
                applicationContract.ApplicationId = txtApplication.Text;
                applicationContract.ApplicantId = txtApplicantId.Text;
                applicationContract.RecruitmentProject = ddlRecruitmentProject.SelectedValue;
                applicationContract.JobId = ddlJob.SelectedValue;
                applicationContract.Department = ddlDepartment.SelectedValue;
                applicationContract.ExpireDate =
                    DateTime.ParseExact(txtExpireDate.Text, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);
                applicationContract.DateOfReception =
                    DateTime.ParseExact(txtDateOfReceipt.Text, "yyyy-MM-dd",
                    System.Globalization.CultureInfo.InvariantCulture);

                applicationContract.FileName = fileName;
                applicationContract.FileExt = fileExt;
                applicationContract.FileBase64String = base64String;

                ESSHRApplications applicationSvc = new ESSHRApplications();
                long applicationRecId = applicationSvc.CreateApplicant(applicationContract);

                if (applicationRecId <= 0)
                    throw new Exception("Application creation failed.");

                // ✅ DELAYED CLOSE + PARENT REFRESH
                string script = @"
            alert('Applicant and Application created successfully!');
            setTimeout(function() { 
                if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                    window.parent.refreshParentGrid();
                }
                if (typeof closeDialog === 'function') {
                    closeDialog();
                }
            }, 3000);";

                ScriptManager.RegisterStartupScript(
                    this,
                    this.GetType(),
                    "closeModal",
                    script,
                    true
                );
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
                    "CreateError",
                    "alert('An error occurred while saving Applicant/Application.');",
                    true
                );
            }
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



        private string FormatDecimal(object value)
        {
            if (value == null || value == DBNull.Value)
                return "0.00";

            decimal d;
            return decimal.TryParse(value.ToString(), out d)
                ? d.ToString("0.00")
                : "0.00";
        }


    }
}
