using BussinessObject;
using PortalIntegration;
using PortalIntegration.TASEmployeeAdjustmentsLineSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.TA
{
    public partial class TASEmployeeAdjustments_Create : ModalForm
    {
        private string employeeId;

        protected override void Page_Load(object sender, EventArgs e)

        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Employee Adjustment";
                //  titleDiv.Style["font-weight"] = "bold";
            }
            // Optional: You could load data only once on first load 
            if (!IsPostBack)
            {
                txtClockIn.Attributes["type"] = "datetime-local";
                TxtClockOut.Attributes["type"] = "datetime-local";
                bindData();
                BindReasonCode();
            }
        }

        protected void cddlEmployeeDetails_OnEmployeeSelected(object sender, EventArgs e)
        {
            employeeId = (DropDownList_EmployeeDetails1.FindControl("txtEmployeeId") as TextBox)?.Text;
        }
        private void bindData()
        {
            TASEmployeeRoster TASEmployeeRoster = new TASEmployeeRoster();
            DataTable dt = TASEmployeeRoster.retrieveShiftIds();

            ddlShiftId.DataSource = dt;
            ddlShiftId.DataValueField = "ShiftId";   // value to pass
            ddlShiftId.DataTextField = "ShiftId";    // text to show
            ddlShiftId.DataBind();
        }


        //    protected void btnOk_Click(object sender, EventArgs e)
        //    {
        //        try
        //        {
        //            // 🔹 Always re-fetch employeeId in case event didn't fire
        //            employeeId = (DropDownList_EmployeeDetails1.FindControl("txtEmployeeId") as TextBox)?.Text;


        //            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();

        //            // Prepare data
        //            DataTable dt = new DataTable();
        //            dt.Columns.Add("EmployeeId");
        //            dt.Columns.Add("ShiftId");
        //            dt.Columns.Add("fromDate");
        //            dt.Columns.Add("toDate");
        //            dt.Columns.Add("AttendanceDate");

        //            DataRow row = dt.NewRow();
        //            row["EmployeeId"] = employeeId;
        //            row["ShiftId"] = ddlShiftId.SelectedValue;//txtShiftId.Text.Trim();
        //            row["AttendanceDate"] = txtAttendanceDate.Text.Trim();
        //            row["ClockIn"] = txtClockIn.Text.Trim();
        //            row["ClockOut"] = TxtClockOut.Text.Trim();
        //            dt.Rows.Add(row);

        //            // Call the business logic/service class
        //            TASEmployeeAdjustmentLines employeeAdjustmentslines = new TASEmployeeAdjustmentLines();
        //            SysOperationResult_BOL result = employeeAdjustmentslines.create(dt);


        //            if (result != null && result.isSuccess)
        //            {
        //                // ✅ Show success alert and close dialog
        //                ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage", $"alert('{result.Message}'); closeDialog(true);", true);
        //            }
        //            else
        //            {
        //                // ❌ Show error alert
        //                ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // 🔥 Show exception message
        //            ScriptManager.RegisterStartupScript(this, GetType(), "ShowException", $"alert('Exception: {ex.Message}');", true);
        //        }
        //    }


        private void BindReasonCode()
        {
            try
            {
                // Call your service method (adjust class name if different)
                var svc = new TASEmployeeAdjustmentLines();   // or whichever class has retrieveReasonCode / lookupReasonCode
                DataTable dt = svc.retrieveReasonCode();    // empty search = all records

                ddlReasonCode.Items.Clear();
                ddlReasonCode.Items.Add(new ListItem("-- Select Reason Code --", ""));

                if (dt != null && dt.Rows.Count > 0)
                {
                    ddlReasonCode.DataSource = dt;
                    ddlReasonCode.DataValueField = "ReasonCode";
                    ddlReasonCode.DataTextField = "ReasonCode";   // or "ReasonCodeDescription" if you prefer
                    // If you want to show both:
                    // ddlReasonCode.DataTextField = "ReasonCodeDescription";
                    ddlReasonCode.DataBind();
                }
            }
            catch (Exception ex)
            {
                // Optional: log error
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // Get Employee ID from user control
                string employeeId = (DropDownList_EmployeeDetails1.FindControl("txtEmployeeId") as TextBox)?.Text;

                // Get form values
                string shiftId = ddlShiftId.SelectedValue;
                string attendanceDate = txtAttendanceDate.Text.Trim();
                string clockIn = txtClockIn.Text.Trim();
                string clockOut = TxtClockOut.Text.Trim();
                string remarks = txtRemarks.Text.Trim();
                string reasonCode = ddlReasonCode.SelectedValue;

                // Validation
                if (string.IsNullOrWhiteSpace(employeeId) || string.IsNullOrWhiteSpace(clockIn) || string.IsNullOrWhiteSpace(clockOut))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ValidationError", "alert('Please fill all required fields.');", true);
                    return;
                }

                // Create the DataTable
                DataTable dt = new DataTable();
                dt.Columns.Add("EmployeeId");
                dt.Columns.Add("ShiftId");
                dt.Columns.Add("fromDate");
                dt.Columns.Add("toDate");
                dt.Columns.Add("AttendanceDate");
                dt.Columns.Add("ClockIn");
                dt.Columns.Add("ClockOut");
                dt.Columns.Add("Remarks");
                dt.Columns.Add("ReasonCode");

                DataRow row = dt.NewRow();
                row["EmployeeId"] = employeeId;
                row["ShiftId"] = shiftId;
                row["AttendanceDate"] = attendanceDate;
                row["ClockIn"] = clockIn;
                row["ClockOut"] = clockOut;
                row["Remarks"] = remarks;
                row["ReasonCode"] = reasonCode;
                dt.Rows.Add(row);

                // ⚙️ Call create method
                TASEmployeeAdjustmentLines logic = new TASEmployeeAdjustmentLines();
                SysOperationResult_BOL createResult = logic.create(dt);

                if (createResult != null && createResult.isSuccess)
                {
                    long createdRecId = createResult.RecId;

                   // ESSWorkflow eSSWorkflow = new ESSWorkflow();
                    SysOperationResult_BOL submitResult = logic.pREmployeeAdjustmentRequest_Submit(new long[] { createdRecId });

                    if (submitResult != null && submitResult.isSuccess)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage",
                            $"alert('Request created and submitted successfully!'); closeDialog(true);", true);

                        string script = @"setTimeout(function() { 
                    if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                        window.parent.refreshParentGrid();
                    }
                    closeDialog(); 
                }, 3000);";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);

                        return; 
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "SubmitError",
                            $"alert('Request created, but failed to submit. Reason: {submitResult?.Message ?? "Unknown"}');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "CreateError",
                        $"alert('Error during creation: {createResult?.Message ?? "Unknown error."}');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Exception",
                    $"alert('Exception: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }

    }
}