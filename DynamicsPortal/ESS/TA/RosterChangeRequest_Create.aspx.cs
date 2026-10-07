using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.TA
{
    public partial class RosterChangeRequest_Create : ModalForm
    {
        private TASRosterChangeRequestsSvc tASRosterChangeRequestsSvc = new TASRosterChangeRequestsSvc();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.Title = "Shift Request";
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Create Request";
                    titleDiv.Style["font-weight"] = "bold";
                }
                txtRequestDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

              //  txtPersonnelNumber.Text = SessionVariables.getCurrentEmployeeId();
                txtEmployeeName.Text = SessionVariables.getCurrentEmployeeName();

                BindShiftIdDropdown();
            }
        }

        //private void BindShiftIdDropdown()
        //{
        //    SysErrorLog objErrorLog = new SysErrorLog();
        //    string currentMethodName = $"{this.GetType().FullName}.{nameof(BindShiftIdDropdown)}";

        //    try
        //    {
        //        DataTable dt = tASRosterChangeRequestsSvc.retrieveShiftId();

        //        ddlShiftId.Items.Clear();
        //        ddlShiftId.Items.Add(new ListItem("Select Shift", ""));

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            ddlShiftId.DataSource = dt;
        //            ddlShiftId.DataTextField = "ShiftId";
        //            ddlShiftId.DataValueField = "ShiftId";
        //            ddlShiftId.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.write(currentMethodName, ex);
        //    }
        //}


        private void BindShiftIdDropdown()
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(BindShiftIdDropdown)}";

            try
            {
                DataTable dt = tASRosterChangeRequestsSvc.retrieveShiftId();

                ddlShiftId.Items.Clear();
                ddlShiftId.Items.Add(new ListItem("Select Shift", ""));

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string shiftId = row["ShiftId"].ToString();
                        string shiftName = row["ShiftCode"].ToString();
                        string description = row["Description"].ToString();

                        // Display both ID and Name
                        string displayText = $"{shiftId} - {shiftName} - {description}";

                        ddlShiftId.Items.Add(new ListItem(displayText, shiftId));
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.write(currentMethodName, ex);
            }
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            // Handle employee selection from the DropDownList_EmployeeDetails user control if needed
        }

        //protected void btnOk_Click(object sender, EventArgs e)
        //{
        //    SysErrorLog objErrorLog = new SysErrorLog();
        //    string currentMethodName = $"{this.GetType().FullName}.{nameof(btnOk_Click)}";
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(txtFromDate.Text) || string.IsNullOrWhiteSpace(txtToDate.Text)
        //            || string.IsNullOrWhiteSpace(ddlShiftId.SelectedValue))
        //        {
        //            ShowToast("Please fill in all required fields.", "error");
        //            return;
        //        }

        //        long recId = tASRosterChangeRequestsSvc.createRoster(
        //            txtPersonnelNumber.Text,
        //            ddlShiftId.SelectedValue,
        //            txtFromDate.Text,
        //            txtToDate.Text,
        //            txtRequestDate.Text,
        //            shiftStartTime: null,
        //            shiftEndTime: null,
        //            recId: 0
        //        );

        //        if (recId > 0)
        //        {
        //            ShowToast("Roster change request submitted successfully.", "success");
        //            // Optional: clear the form after success
        //            // txtFromDate.Text = string.Empty;
        //            // txtToDate.Text = string.Empty;
        //            // ddlShiftId.SelectedIndex = 0;
        //        }
        //        else
        //        {
        //            ShowToast("Failed to submit roster change request. Please try again.", "error");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.write(currentMethodName, ex);
        //        ShowToast("An error occurred while submitting your request.", "error");
        //    }
        //}



        protected void btnOk_Click(object sender, EventArgs e)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(btnOk_Click)}";

            try
            {
                // ========== VALIDATION ==========
                if (string.IsNullOrWhiteSpace(txtFromDate.Text) ||
                    string.IsNullOrWhiteSpace(txtToDate.Text) ||
                    string.IsNullOrWhiteSpace(ddlShiftId.SelectedValue))

                {
                    SysOperationResult_BOL warning = new SysOperationResult_BOL();
                    warning.AlertType = AlertType.Warning.ToString();
                    warning.isSuccess = false;
                    warning.Message = "Please fill in all required fields.";
                    NotificationMessage.showMessage(warning);
                    return;   // overlay will stay until user closes the message or you can hide it if needed
                }

                // ========== CALL SERVICE ==========
                string personalNumber = "";
                var txtEmpId = cddlEmployeeDetails.FindControl("txtEmployeeId") as System.Web.UI.WebControls.TextBox;
                if (txtEmpId != null)
                {
                    personalNumber = txtEmpId.Text.Trim();
                }
                long recId = tASRosterChangeRequestsSvc.createRoster(
                   personalNumber,
                    ddlShiftId.SelectedValue,
                    txtFromDate.Text,
                    txtToDate.Text,
                    txtRequestDate.Text,
                    shiftStartTime: null,
                    shiftEndTime: null,
                    recId: 0
                );

                if (recId > 0)
                {
                    // Success
                    SysOperationResult_BOL success = new SysOperationResult_BOL();
                    success.AlertType = AlertType.Success.ToString();
                    success.isSuccess = true;
                    success.Message = "Roster change request created successfully.";
                    NotificationMessage.showMessage(success);

                    // Close dialog + refresh parent grid after short delay (same as Overtime)
                    string script = @"
                        setTimeout(function() {
                            if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                                window.parent.refreshParentGrid();
                            }
                            closeDialog();
                        }, 1500);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseAndRefresh", script, true);
                }
                else
                {
                    SysOperationResult_BOL error = new SysOperationResult_BOL();
                    error.AlertType = AlertType.Error.ToString();
                    error.isSuccess = false;
                    error.Message = "Failed to create roster change request. Please try again.";
                    NotificationMessage.showMessage(error);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.write(currentMethodName, ex);

                SysOperationResult_BOL error = new SysOperationResult_BOL();
                error.AlertType = AlertType.Error.ToString();
                error.isSuccess = false;
                error.Message = "An error occurred while creating the request.";
                NotificationMessage.showMessage(error);
            }
        }
        private void ShowToast(string message, string type)
        {
            //lblMessage.Text = message;
            string script = $"showToast({Newtonsoft.Json.JsonConvert.SerializeObject(message)}, '{type}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastScript", script, true);
        }
    }
}