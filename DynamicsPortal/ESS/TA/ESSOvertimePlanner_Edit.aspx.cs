using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.TASOvertimePlannerSvcReference;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.TA
{
    public partial class ESSOvertimePlanner_Edit : ModalForm
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSOvertimePlanner_Edit";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    LoadDataFromSession();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
            }
        }

        // -------------------------------------------------------
        // LOAD DATA FROM SESSION
        // -------------------------------------------------------
        private void LoadDataFromSession()
        {
            if (Session["OT_Edit_RecId"] == null)
            {
                NotificationMessage.showMessage("No record selected for edit.");
                return;
            }

            txtEmployeeId.Text = Session["OT_Edit_EmployeeId"]?.ToString() ?? "";
            txtEmployeeName.Text = Session["OT_Edit_EmployeeName"]?.ToString() ?? "";
            txtRequestDate.Text = Session["OT_Edit_RequestDate"]?.ToString() ?? "";
            txtWFStatus.Text = Session["OT_Edit_WFStatus"]?.ToString() ?? "";

            // Plan Date → convert to yyyy-MM-dd for TextMode="Date"
            if (DateTime.TryParse(Session["OT_Edit_PlanDate"]?.ToString(), out DateTime planDate))
            {
                txtPlanDate.Text = planDate.ToString("yyyy-MM-dd");
            }

            // Start Time & End Time - convert to 12-hour format with seconds
            txtStartTime.Text = FormatTimeForEdit(Session["OT_Edit_StartTime"]?.ToString());
            txtEndTime.Text = FormatTimeForEdit(Session["OT_Edit_EndTime"]?.ToString());
        }

        // -------------------------------------------------------
        // Format time for edit - ensures 12-hour format with seconds
        // Input could be "08:47 AM", "08:47:30 AM", "08:47:30", "08:47"
        // Output: "08:47:30 AM"
        // -------------------------------------------------------
        private string FormatTimeForEdit(string timeValue)
        {
            if (string.IsNullOrWhiteSpace(timeValue))
                return "";

            timeValue = timeValue.Trim();

            // Try to parse as DateTime with various formats
            string[] formats = {
                "hh:mm:ss tt", "h:mm:ss tt", "hh:mm tt", "h:mm tt",
                "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm"
            };

            if (DateTime.TryParseExact(timeValue, formats,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                // Return in 12-hour format with seconds: "08:47:30 AM"
                return dt.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
            }

            // Try parsing as seconds since midnight
            if (int.TryParse(timeValue, out int seconds))
            {
                DateTime timeFromSeconds = DateTime.Today.AddSeconds(seconds);
                return timeFromSeconds.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
            }

            // Try TimeSpan
            if (TimeSpan.TryParse(timeValue, out TimeSpan ts))
            {
                DateTime timeFromSpan = DateTime.Today.Add(ts);
                return timeFromSpan.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
            }

            // Return original if can't parse
            return timeValue;
        }

        // -------------------------------------------------------
        // SAVE / UPDATE
        // -------------------------------------------------------
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UpdateRecord(false);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            UpdateRecord(true);
        }

        private void UpdateRecord(bool submitAfterUpdate = false)
        {
            try
            {
                // ========== CHECK SESSION ==========
                if (Session["OT_Edit_RecId"] == null)
                {
                    NotificationMessage.showMessage("Session expired or RecId is missing. Please select the record again.");
                    return;
                }

                long recId = Convert.ToInt64(Session["OT_Edit_RecId"]);

                if (recId <= 0)
                {
                    NotificationMessage.showMessage("Invalid RecId: " + recId);
                    return;
                }

                // ========== VALIDATION ==========
                if (string.IsNullOrWhiteSpace(txtEmployeeId.Text))
                {
                    NotificationMessage.showMessage("Employee Id is missing.");
                    return;
                }

                if (!DateTime.TryParse(txtPlanDate.Text, out DateTime planDate))
                {
                    NotificationMessage.showMessage("Invalid Plan Date.");
                    return;
                }

                // Parse Start Time (supports 12-hour with seconds)
                if (!TryParseTimeToSeconds(txtStartTime.Text, out int startSeconds))
                {
                    NotificationMessage.showMessage("Invalid Start Time. Use format: HH:mm:ss AM/PM (e.g., 08:30:00 AM)");
                    return;
                }

                // Parse End Time (supports 12-hour with seconds)
                if (!TryParseTimeToSeconds(txtEndTime.Text, out int endSeconds))
                {
                    NotificationMessage.showMessage("Invalid End Time. Use format: HH:mm:ss AM/PM (e.g., 05:00:00 PM)");
                    return;
                }

                if (endSeconds <= startSeconds)
                {
                    NotificationMessage.showMessage("End Time must be greater than Start Time.");
                    return;
                }

                DateTime requestDate = DateTime.Today;
                DateTime.TryParse(txtRequestDate.Text, out requestDate);

                // ========== BUILD DATATABLE ==========
                DataTable dt = new DataTable();
                dt.Columns.Add("employeeId", typeof(string));
                dt.Columns.Add("employeeName", typeof(string));
                dt.Columns.Add("planDate", typeof(DateTime));
                dt.Columns.Add("startTime", typeof(int));
                dt.Columns.Add("endTime", typeof(int));
                dt.Columns.Add("requestDate", typeof(DateTime));
                dt.Columns.Add("recId", typeof(long));

                DataRow dr = dt.NewRow();
                dr["employeeId"] = txtEmployeeId.Text.Trim();
                dr["employeeName"] = txtEmployeeName.Text.Trim();
                dr["planDate"] = planDate;
                dr["startTime"] = startSeconds;
                dr["endTime"] = endSeconds;
                dr["requestDate"] = requestDate;
                dr["recId"] = recId;
                dt.Rows.Add(dr);

                // ========== CALL SERVICE ==========
                TASOvertimePlannersSvc svc = new TASOvertimePlannersSvc();
                var result = svc.UpdateGeneral(dt);

                NotificationMessage.showMessage(result);

                if (result != null && result.isSuccess)
                {
                    ClearEditSession();

                    string script = @"
                setTimeout(function() {
                    if (window.parent && typeof window.parent.refreshParentGrid === 'function') {
                        window.parent.refreshParentGrid();
                    }
                    closeDialog();
                }, 1500);";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseAndRefresh", script, true);
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
                NotificationMessage.showMessage("Error: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // TIME PARSER - supports 12-hour and 24-hour formats with seconds
        // -------------------------------------------------------
        private bool TryParseTimeToSeconds(string input, out int totalSeconds)
        {
            totalSeconds = 0;
            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim().ToUpper();

            // 12-hour formats with seconds: "08:47:30 AM"
            string[] twelveHourFormats = {
                "hh:mm:ss tt",
                "h:mm:ss tt",
                "hh:mm tt",
                "h:mm tt"
            };

            if (DateTime.TryParseExact(input, twelveHourFormats,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt12))
            {
                totalSeconds = (int)dt12.TimeOfDay.TotalSeconds;
                return true;
            }

            // 24-hour formats with seconds: "08:47:30" or "20:47:30"
            string[] twentyFourFormats = {
                "HH:mm:ss",
                "H:mm:ss",
                "HH:mm",
                "H:mm"
            };

            if (DateTime.TryParseExact(input, twentyFourFormats,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt24))
            {
                totalSeconds = (int)dt24.TimeOfDay.TotalSeconds;
                return true;
            }

            // Try TimeSpan parse
            if (TimeSpan.TryParse(input, out TimeSpan ts))
            {
                totalSeconds = (int)ts.TotalSeconds;
                return true;
            }

            return false;
        }

        private void ClearEditSession()
        {
            Session.Remove("OT_Edit_RecId");
            Session.Remove("OT_Edit_PlanId");
            Session.Remove("OT_Edit_EmployeeId");
            Session.Remove("OT_Edit_EmployeeName");
            Session.Remove("OT_Edit_RequestDate");
            Session.Remove("OT_Edit_PlanDate");
            Session.Remove("OT_Edit_StartTime");
            Session.Remove("OT_Edit_EndTime");
            Session.Remove("OT_Edit_WFStatus");
        }
    }
}