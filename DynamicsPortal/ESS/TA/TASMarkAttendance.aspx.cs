using BussinessObject;
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
    public partial class TASMarkAttendance : ModalForm
    {
        private string EmployeeId
        {
            get => ViewState["EmployeeId"] as string;
            set => ViewState["EmployeeId"] = value;
        }



        protected override void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
            pageMenuId = "TASMarkAttendance";

            base.Page_Load(sender, e);

            if (!isUserAuthenticated)
                return;
            var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
            if (titleDiv != null)
            {
                titleDiv.InnerText = "Mark Attendance";
                // titleDiv.Style["font-weight"] = "bold";
            }

            if (!IsPostBack)
            {
                string employeeId = Request.QueryString["EmployeeId"];
                string employeeName = Request.QueryString["EmployeeName"];
                string clockIn = Request.QueryString["ClockIn"];
                string clockOut = Request.QueryString["ClockOut"];
                string attendanceDate = Request.QueryString["AttendanceDate"];

                // Check if multiple records are accidentally passed
                if (!string.IsNullOrEmpty(employeeId) && employeeId.Contains(","))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Invalid request. Please select only 1 record.'); window.location='TASAttendanceRegister_ListPage.aspx';", true);
                    return;
                }

                if (!string.IsNullOrEmpty(employeeId))
                {
                    txtEmployeeId.Text = employeeId;
                    EmployeeId = employeeId; // <-- Add this line to store it in ViewState
                }

                if (!string.IsNullOrEmpty(employeeName))
                {
                    txtEmployeeName.Text = employeeName;
                }

                if (!string.IsNullOrEmpty(clockIn))
                {
                    txtClockIn.Text = Convert.ToDateTime(clockIn).ToString("HH:mm");
                }

                if (!string.IsNullOrEmpty(clockOut))
                {
                    txtClockOut.Text = Convert.ToDateTime(clockOut).ToString("HH:mm");
                }
                if (!string.IsNullOrEmpty(attendanceDate))
                {
                    DateTime parsedDate;
                    if (DateTime.TryParse(attendanceDate, out parsedDate))
                    {
                        // ✅ Show only the Date part (MM/dd/yyyy)
                        txtAttendanceDate.Text = parsedDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        // fallback if parsing fails
                        txtAttendanceDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    }
                }
            }

        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {

            string script = "closeDialog();";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", script, true);
        }

      

      
        protected void btnOk_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(EmployeeId))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "EmptyEmpId", "alert('Employee ID is required.');", true);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtClockIn.Text) || string.IsNullOrWhiteSpace(txtClockOut.Text))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "EmptyTimes", "alert('Both Clock In and Clock Out times are required.');", true);
                    return;
                }

                // Create DataTable for attendance
                DataTable dt = new DataTable();
                dt.Columns.Add("EmployeeId");
                dt.Columns.Add("EmployeeName");
                dt.Columns.Add("ClockIn");
                dt.Columns.Add("ClockOut");
                dt.Columns.Add("AttendanceDate");
                dt.Columns.Add("Remarks");

                DataRow row = dt.NewRow();
                row["EmployeeId"] = EmployeeId;
                row["EmployeeName"] = txtEmployeeName.Text.Trim();
                row["ClockIn"] = txtClockIn.Text.Trim();
                row["ClockOut"] = txtClockOut.Text.Trim();
                row["AttendanceDate"] = txtAttendanceDate.Text.Trim();
                row["Remarks"] = txtRemarks.Text;
                dt.Rows.Add(row);

                // Call business logic to mark attendance
                TASAttendanceRegister attendanceService = new TASAttendanceRegister();
                SysOperationResult_BOL result = attendanceService.markAttendance(dt);

                // Handle result
                if (result != null && result.isSuccess)
                {
                    // 🔄 Submit Logic Start
                    long createdRecId = result.RecId;

                    TASEmployeeAdjustmentLines eSSWorkflow = new TASEmployeeAdjustmentLines();
                    SysOperationResult_BOL submitResult = eSSWorkflow.pREmployeeAdjustmentRequest_Submit(new long[] { createdRecId });

                    if (submitResult != null && submitResult.isSuccess)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage",
                        //    $"alert('Attendance marked and submitted successfully!'); closeDialog(true);", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "SuccessMessage",
                          $"alert('Attendance Mark updated and submitted successfully!'); closeDialog(true);", true);
                       // Response.Redirect("TASAttendanceRegister_ListPage.aspx?msg=Attendance marked and submitted  successfully");
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "SubmitError", $"alert('Attendance marked, but failed to submit. Reason: {submitResult?.Message ?? "Unknown"}');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
                    }
                    // 🔄 Submit Logic End
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowError", $"alert('Error: {result?.Message ?? "Unknown error occurred."}');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowException", $"alert('Exception: {ex.Message}');", true);
            }
        }

       

    }
}