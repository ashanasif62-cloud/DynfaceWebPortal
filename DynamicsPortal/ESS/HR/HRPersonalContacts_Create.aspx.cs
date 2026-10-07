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
    public partial class HRPersonalContacts : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            showPageTitle = false;
            if (!IsPostBack)
            {

                txtBirthDate.Attributes["max"] = DateTime.Today.ToString("yyyy-MM-dd");

                BindRelationShipType();
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

                // ================= CREATE =================

                SysOperationResult_BOL result = new SysOperationResult_BOL();
                 result = svc.create(contract);

                // ================= SUCCESS: CLOSE MODAL & REFRESH PARENT =================
            //    if (result != null && result.isSuccess)
            //    {
            //        // ✅ Show success alert and close dialog
            //        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", $"alert('{result.Message}'); closeDialog(true);", true);

            //        string script = @"setTimeout(function() { 
            //        if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
            //            window.parent.refreshParentGrid();
            //        }
            //        closeDialog(); 
            //    }, 3000);";

            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
            //    }
            //    else
            //    {
            //        // ❌ Show error alert
            //        ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // 🔥 Show exception message
            //    ScriptManager.RegisterStartupScript(this, GetType(), "ShowException", $"alert('Exception: {ex.Message}');", true);
            //}
              NotificationMessage.showMessage(result);

                if (result.isSuccess)
                {
                    // Close modal and reload the list iframe in parent
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


    }
}