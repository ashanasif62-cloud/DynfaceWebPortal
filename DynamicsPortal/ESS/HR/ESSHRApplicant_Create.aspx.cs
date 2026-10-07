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
    public partial class ESSHRApplicant_Create : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {

              
                Page.Title = "Applicant Create";

                // Dynamically set the page title in the master page div
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Create"; // Set the text in the div
                    titleDiv.Style["font-weight"] = "bold"; // Set the style to bold
                }
                txtApplicantType.Text = "External applicant";
                txtDisplayAs.Text = "FirstMiddleLast";
                bindGeneral();


            }
        }

        private void bindGeneral()
        {
            HCMApplicants svc = new HCMApplicants();

            // This service should return the next/new Applicant
            DataTable dt = svc.retrieveApplicantId();

            if (dt != null && dt.Rows.Count > 0)
            {
                txtApplicant.Text = dt.Rows[0]["Applicant"].ToString();
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // 1️⃣ Create and populate the contract
                HCMApplicantContract applicantContract = new HCMApplicantContract();

                // General Information
                //applicantContract.parmApplicant(txtApplicant.Text);
               /* applicantContract.parmApplicantType(txtApplicantType.Text)*/;
                applicantContract.NameSequenceDisplayAs = txtDisplayAs.Text;
              
                // Name details
               
                applicantContract.FirstName = txtFirstName.Text;
                applicantContract.MiddleName = txtMiddleName.Text;
                applicantContract.LastName = txtLastName.Text;
                applicantContract.LastNamePrefix = txtLastNamePrefix.Text;
                applicantContract.NameAlias = txtSearchName.Text;
                applicantContract.Initials = txtInitials.Text;
                applicantContract.KnownAs = txtKnownAs.Text;
                //applicantContract.parmProfessionalTitle(txtProfessionalTitle.Text);
                //applicantContract.parmProfessionalSuffix(txtProfessionalSuffix.Text);
                //applicantContract.parmPhoneticFirst(txtPhoneticFirst.Text);
                //applicantContract.parmPhoneticMiddle(txtPhoneticMiddle.Text);
                //applicantContract.parmPhoneticLast(txtPhoneticLast.Text);

                // Applicant Information
                //applicantContract.parmCurrentJobTitle(txtCurrentJobTitle.Text);
                //applicantContract.parmHighestDegree(txtHighestDegree.Text);
                //applicantContract.parmApplications(txtApplications.Text);
                //applicantContract.parmTotalApplications(txtTotalApplications.Text);

                // Future Consideration
                //applicantContract.parmPreviousEmployee(txtPreviousEmployee.Text);
                //applicantContract.parmSkillMapping(txtSkillMapping.Text);
                //applicantContract.parmIncludeInSkillMapping(txtIncludeInSkillMapping.Text);

                // Other Information
                //applicantContract.parmReasonCode(txtReasonCode.Text);
                //applicantContract.parmOtherInformation(txtOtherInformation.Text);
                //applicantContract.parmAddressBooks(txtAddressBooks.Text);

                // 2️⃣ Call the service to create the applicant
                HCMApplicants svc = new HCMApplicants();
                long recId = svc.CreateApplicant(applicantContract);

                if (recId > 0)
                {
                    // Store RecId in session for later use
                    Session["SelectedApplicantRecId"] = recId;

                    // Show success message and close popup
                    ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "ClosePopup",
                "alert('Applicant created successfully!'); " +
                "if (typeof closePopupPanel === 'function') closePopupPanel();",
                true
            );
                }
                else
                {
                    ScriptManager.RegisterStartupScript(
                        this,
                        this.GetType(),
                        "CreateFailed",
                        "alert('Failed to create applicant.');",
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
                    "CreateError",
                    "alert('An error occurred while creating the applicant.');",
                    true
                );
            }
        }


        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    // Close popup
        //    ScriptManager.RegisterStartupScript(
        //        this,
        //        this.GetType(),
        //        "ClosePopup",
        //        "window.close();",
        //        true
        //    );
        //}

    }
}