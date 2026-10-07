using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSPersonalContactsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.HR
{
    public partial class HRPersonalContacts_Update : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Personal Details";
            }

            if (!IsPostBack)
            {
                txtBirthDate.Attributes["max"] = DateTime.Today.ToString("yyyy-MM-dd");
                BindRelationShipType();
                DataTable dt = SessionVariables.getSessionDataTable();
                if (dt != null)
                {
                    BindUpdateData(dt);
                }
                else
                {
                    SysOperationResult_BOL error = new SysOperationResult_BOL();
                    error.isSuccess = false;
                    error.AlertType = AlertType.Error.ToString();
                    error.Message = "Record to Edit Not Found";
                    NotificationMessage.showMessage(error);
                }
            }
        }


        private bool BindRelationShipType(string selectedValue = "")
        {
            ESSHRPersonalContacts svc = new ESSHRPersonalContacts();
            DataTable dt = svc.retrieveRelationShipTypeId();

            if (dt != null && dt.Rows.Count > 0)
            {
                ddlRelationship.DataSource = dt;
                ddlRelationship.DataTextField = "Name";   // what user sees
                ddlRelationship.DataValueField = "Name";  // underlying value
                ddlRelationship.DataBind();

                // Insert empty option at the top
                ddlRelationship.Items.Insert(0, new ListItem(""));
            }

            // ✅ Priority 1: use provided selected value
            if (!string.IsNullOrEmpty(selectedValue) &&
                ddlRelationship.Items.FindByValue(selectedValue) != null)
            {
                ddlRelationship.SelectedValue = selectedValue;
            }
            // ✅ Priority 2: default to FamilyContact
            else if (ddlRelationship.Items.FindByValue("FamilyContact") != null)
            {
                ddlRelationship.SelectedValue = "FamilyContact";
            }
            // ✅ Fallback: keep empty
            else
            {
                ddlRelationship.SelectedIndex = 0;
            }


            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ESSHRPersonalContacts svc = new ESSHRPersonalContacts();
                ESSPersonalContactsContract contract = new ESSPersonalContactsContract();

                // ================= BASIC INFO =================
                contract.RecId = Convert.ToInt64(lblRecId.Text);
                contract.EmployeeId = SessionVariables.getCurrentEmployeeId ();
                contract.FirstName = txtFirstName.Text.Trim();
                contract.LastName = txtLastName.Text.Trim();
                contract.Name = txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim();
                contract.RelationShipTypeId = ddlRelationship.SelectedValue;
               

                // ================= GENERAL =================
                contract.EmergencyContact = chkEmergencyContact.Checked;

                // ================= DEPENDANT =================
                contract.isDependent = ChkDependant.Checked;

                if (ChkDependant.Checked)
                {
                    contract.Gender = ddlGender.SelectedValue;
                    contract.BirthDate = string.IsNullOrEmpty(txtBirthDate.Text)
                        ? DateTime.MinValue
                        : Convert.ToDateTime(txtBirthDate.Text);
                    contract.DependentValidFrom = string.IsNullOrEmpty(txtDependenatValidFromDate.Text)
                        ? DateTime.MinValue
                        : Convert.ToDateTime(txtDependenatValidFromDate.Text);
                    contract.DependentValidTo = string.IsNullOrEmpty(txtDependenatValidToDate.Text)
                        ? DateTime.MinValue
                        : Convert.ToDateTime(txtDependenatValidToDate.Text);
                    contract.IsFullTimeStudent = chkFullTimeStudent.Checked;
                    contract.IsPersonWithDisabilities = chkDisability.Checked;
                    contract.VerificationDate = txtVerificationDate.Text.toDateTime();
                }
                else
                {
                    contract.Gender = "None";
                }

                contract.isBeneficiary = ChkBeneficiary.Checked;
                
                if (ChkBeneficiary.Checked)
                {
                    contract.BenefecieryPercentage = txtBeneficiaryPercentage.Text.Trim();
                    contract.BeneficiaryFromDate = Convert.ToDateTime(txtBeneficiaryValidFrom.Text);
                    contract.BeneficiaryToDate = Convert.ToDateTime(txtBeneficiaryValidTo.Text);
                }


                SysOperationResult_BOL result = new SysOperationResult_BOL();
                 result = svc.updatePersonalContract(contract);


              NotificationMessage.showMessage(result);

                if (result.isSuccess)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal",
                        @"(function(){
                var win = window;
                while(win !== win.parent) win = win.parent;
                if(win.GlobalPopup) win.GlobalPopup.hide();
                // Reload the personal contacts iframe
                var frames = win.document.querySelectorAll('iframe');
                for(var i=0;i<frames.length;i++){
                    if(frames[i].src && frames[i].src.indexOf('HRPersonalContacts_ListPage')!==-1){
                        frames[i].contentWindow.location.reload();
                        break;
                    }
                }
            })();", true);
                    SessionVariables.setSessionDataTable(new DataTable());
                }

            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }

        private void BindUpdateData(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return;

            DataRow dr = dt.Rows[0];

            /* ---------------- BASIC INFO ---------------- */
            lblRecId.Text = dr["RecId"]?.ToString() ?? string.Empty;
            string fullName = dr["Name"]?.ToString() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(fullName))
            {
                string[] nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                txtFirstName.Text = nameParts.Length > 0 ? nameParts[0] : string.Empty;
                txtLastName.Text =
                    nameParts.Length > 1
                        ? string.Join(" ", nameParts.Skip(1))
                        : string.Empty;
            }
            else
            {
                txtFirstName.Text = string.Empty;
                txtLastName.Text = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(dr["RelationShipTypeId"]?.ToString()))
                ddlRelationship.SelectedValue = dr["RelationShipTypeId"].ToString();

            chkEmergencyContact.Checked = Convert.ToBoolean(dr["EmergencyContact"]);

            /* ---------------- BENEFICIARY ---------------- */

            ChkBeneficiary.Checked = Convert.ToBoolean(dr["isBeneficiary"]);

            txtBeneficiaryValidFrom.Text =
                dr["BeneficiaryFromDate"] != DBNull.Value
                    ? Convert.ToDateTime(dr["BeneficiaryFromDate"]).ToString("yyyy-MM-dd")
                    : string.Empty;

            txtBeneficiaryValidTo.Text =
                dr["BeneficiaryToDate"] != DBNull.Value
                    ? Convert.ToDateTime(dr["BeneficiaryToDate"]).ToString("yyyy-MM-dd")
                    : string.Empty;

            txtBeneficiaryPercentage.Text = dr["BenefecieryPercentage"] != DBNull.Value ? dr["BenefecieryPercentage"].ToString() : string.Empty;

            ChkPrimary.Checked = Convert.ToBoolean(dr["IsBeneficiary"]);

            /* ---------------- DEPENDANT ---------------- */

            ChkDependant.Checked = Convert.ToBoolean(dr["isDependent"]);

            // Valid From Date
            // ✅ Match exact column names from the DataTable
            txtDependenatValidFromDate.Text =
                dr["DependentValidFrom"] != DBNull.Value
                    ? Convert.ToDateTime(dr["DependentValidFrom"]).ToString("yyyy-MM-dd")
                    : string.Empty;

            txtDependenatValidToDate.Text =
                dr["DependentValidTo"] != DBNull.Value
                    ? Convert.ToDateTime(dr["DependentValidTo"]).ToString("yyyy-MM-dd")
                    : string.Empty;


            ddlGender.SelectedValue = dr["Gender"]?.ToString() ?? string.Empty;

            txtBirthDate.Text =
                dr["BirthDate"] != DBNull.Value
                    ? Convert.ToDateTime(dr["BirthDate"]).ToString("yyyy-MM-dd")
                    : string.Empty;

            chkFullTimeStudent.Checked =
                dr["isFullTimeStudent"]?.ToString().Equals("True", StringComparison.OrdinalIgnoreCase) ?? false;

            chkDisability.Checked =
                dr["isPersonWithDisabilities"]?.ToString().Equals("True", StringComparison.OrdinalIgnoreCase) ?? false;

            txtVerificationDate.Text =
                dr["VerificationDate"] != DBNull.Value
                    ? Convert.ToDateTime(dr["VerificationDate"]).ToString("yyyy-MM-dd")
                    : string.Empty;
        }

    }
}