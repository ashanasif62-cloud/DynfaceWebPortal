using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HCMApplicantsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSHRApplicant_DetailForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get the 'Applicant' parameter from the query string
                string name = Request.QueryString["FirstName"];

                if (string.IsNullOrEmpty(name))
                {
                    // Optional: redirect back to list page if no applicant is specified
                    Response.Redirect("~/ESS/HR/ESSHRApplicants_ListPage.aspx");
                    return;
                }

                // Set page title
                Page.Title = "Applicant Detail";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Applicant Detail"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }

                // Bind data to the page
                BindGeneral(name);
                BindAddressGrid();
                BindContactGrid();
            }

        }


        private void BindGeneral(string name)
        {
            try
            {
                // Call your service to get the project details
                HCMApplicants svc = new HCMApplicants();
                DataTable dt = svc.retrieveAll();

                // Filter the DataTable to the selected project
                DataRow[] rows = dt.Select($"FirstName = '{name}'");
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];

                    // Bind values to labels
                    txtApplicant.Text = row["Applicant"].ToString();
                    hdnFirstName.Value = row["FirstName"].ToString();
                    txtApplicantType.Text = row["applicantType"].ToString();
                    // txtDepartment.Text = row["Organization"].ToString();

                    //txtPersonalTitle.Text = row["Recruiter"].ToString();
                    txtFirstName.Text = row["FirstName"].ToString();
                    txtMiddleName.Text = row["MiddleName"].ToString();

                    txtLastNamePrefix.Text = row["LastNamePrefix"].ToString();

                    txtLastName.Text = row["LastName"].ToString();
                    //txtPersonalSuffix.Text = row["PersonalSuffix"].ToString();
                    txtSearchName.Text = row["NameAlias"].ToString();
                    txtInitials.Text = row["Initials"].ToString();
                    txtKnownAs.Text = row["KnownAs"].ToString();
                    txtProfessionalTitle.Text = row["ProfessionalTitle"].ToString();
                    txtProfessionalSuffix.Text = row["ProfessionalSuffix"].ToString();
                    txtPhoneticFirst.Text = row["PhoneticFirstName"].ToString();
                    txtPhoneticMiddle.Text = row["PhoneticMiddleName"].ToString();
                    txtPhoneticLast.Text = row["PhoneticLastName"].ToString();
                    txtDisplayAs.Text = row["NameSequenceDisplayAs"].ToString();
                    txtCurrentJobTitle.Text = row["CurrentJobTitle"].ToString();
                    txtHighestDegree.Text = row["EducationLevelId"].ToString();
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
                txtFirstName.Text = "Error loading Applicant details";
            }
        }

        private void BindAddressGrid()
        {
            //ESSHRRecruitment svc = new ESSHRRecruitment();
            DataTable dt = new DataTable();

            //// Use same column names as in GridView (and service result)
            dt.Columns.Add("Name");
            dt.Columns.Add("Address");
            dt.Columns.Add("Purpose");
            dt.Columns.Add("Primary");
          




            gvAddresses.DataSource = dt;
            gvAddresses.DataBind();
        }

        private void BindContactGrid()
        {
            //ESSHRRecruitment svc = new ESSHRRecruitment();
            DataTable dt = new DataTable();

            //// Use same column names as in GridView (and service result)
            dt.Columns.Add("Description");
            dt.Columns.Add("Type");
            dt.Columns.Add("ContactValue");
            dt.Columns.Add("Extension");
            dt.Columns.Add("Primary");
            dt.Columns.Add("Private");





            gvContacts.DataSource = dt;
            gvContacts.DataBind();
        }

        //protected void btnNew_Click(object sender, EventArgs e)
        //{
        //    // Store values in session
        //    Session["Applicant"] = txtApplicant.Text;
        //    Session["applicantType"] = txtApplicantType.Text;
        //    Session["NameSequenceDisplayAs"] = txtDisplayAs.Text;

        ////optional

        //if (Session["SelectedApplicantRecId"] != null)
        //{
        //    Session["ApplicantRecId"] = Session["SelectedApplicantRecId"];
        //}

        //// Open create form in popup
        //ScriptManager.RegisterStartupScript(
        //        this,
        //        this.GetType(),
        //        "OpenCreateApplicant",
        //        "openPopupPanel('/ESS/HR/ESSHRApplicant_Create.aspx');",
        //        true
        //    );
        //}

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Create and populate the contract
                HCMApplicantContract applicantContract = new HCMApplicantContract();

                // Fill the contract fields (replace with your actual form controls)
                 // Applicant identifier
                applicantContract.Applicant = txtApplicant.Text;
                applicantContract.FirstName = txtFirstName.Text;
                applicantContract.MiddleName = txtMiddleName.Text;
                applicantContract.LastName = txtLastName.Text;
                applicantContract.LastNamePrefix = txtLastNamePrefix.Text;
                applicantContract.NameAlias = txtSearchName.Text;
                applicantContract.Initials = txtInitials.Text;
                applicantContract.KnownAs = txtKnownAs.Text;
                applicantContract.NameSequenceDisplayAs = txtDisplayAs.Text;
                applicantContract.CurrentJobTitle = txtCurrentJobTitle.Text;
                applicantContract.EducationLevelId = txtHighestDegree.Text;

                // 2️⃣ Call the service to update the applicant
                HCMApplicants svc = new HCMApplicants();
                bool isUpdated = svc.updateApplicant(applicantContract);

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


    }
}                                                                                                  