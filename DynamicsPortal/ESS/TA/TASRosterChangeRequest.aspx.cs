using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.TA
{
    public partial class TASRosterChangeRequest : MainForm
    {
        private TASRosterChangeRequestsSvc tASRosterChangeRequestsSvc = new TASRosterChangeRequestsSvc();

        protected override void Page_Load(object sender, EventArgs e)
        {
            pageMenuId = "EmployeeRosterRequest";

            if (!IsPostBack)
            {
                Page.Title = "Roster Change Request";

                // Same as Overtime Planner – this makes the title appear in the UI
                var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                if (titleDiv != null)
                {
                    titleDiv.InnerText = "Roster Change Request";
                    titleDiv.Style["font-weight"] = "bold";
                }

                txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                BindGrid();
                Session["selectedRecid"] = null;
                gvRosterRecord.DataSource = new DataTable();
                gvRosterRecord.DataBind();
            }
        }
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                BindGrid();
                Session["selectedRecid"] = null;
                gvRosterRecord.DataSource = null;
                gvRosterRecord.DataBind();
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
        }

        private void BindGrid()
        {
            string fromDate = txtFromDate.Text;
            string toDate = txtToDate.Text;

            DataTable dt = tASRosterChangeRequestsSvc.retrieveAllwithFilters(fromDate, toDate);

            if (dt != null && dt.Rows.Count > 0)
            {
                gvRosterChangeRequests.DataSource = dt;
                gvRosterChangeRequests.DataBind();
            }
            else
            {
                gvRosterChangeRequests.DataSource = dt ?? new DataTable();
                gvRosterChangeRequests.DataBind();
            }
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;

            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            DateTime fromDate = DateTime.Parse(txtFromDate.Text);
            DateTime toDate = DateTime.Parse(txtToDate.Text);

            if (fromDate <= toDate)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                    string fromDateString = fromDate.ToString("yyyy-MM-dd");
                    string toDateString = toDate.ToString("yyyy-MM-dd");

                    var filteredData = tASRosterChangeRequestsSvc.retrieveAllwithFilters(fromDateString, toDateString);
                }
            }
        }

        // ── Date formatting helper ──
        // Handles real DateTime, AX/D365 service-reference "Date" proxy classes that
        // expose Year/Month/Day (public OR private), and proxies that wrap their value
        // in a private "_value" field (which may itself be DateTime, a numeric OLE date,
        // or another nested proxy object).
        protected string FormatDate(object dateValue)
        {
            DateTime? resolved = ResolveToDateTime(dateValue, 0);
            return resolved.HasValue
                ? resolved.Value.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)
                : string.Empty;
        }

        private DateTime? ResolveToDateTime(object rawValue, int depth)
        {
            if (rawValue == null || rawValue == DBNull.Value || depth > 3)
                return null;

            if (rawValue is DateTime dt)
                return dt;

            if (rawValue is string s)
            {
                return DateTime.TryParse(s, out DateTime parsed) ? parsed : (DateTime?)null;
            }

            Type t = rawValue.GetType();

            // Try Year/Month/Day (public or private)
            int? year = TryGetIntMember(rawValue, t, "Year", "year", "yearField", "_year");
            int? month = TryGetIntMember(rawValue, t, "Month", "month", "monthField", "_month");
            int? day = TryGetIntMember(rawValue, t, "Day", "day", "dayField", "_day");

            if (year.HasValue && month.HasValue && day.HasValue && year.Value > 1 && month.Value >= 1 && day.Value >= 1)
            {
                try { return new DateTime(year.Value, month.Value, day.Value); }
                catch { /* invalid combo, fall through */ }
            }

            // Try a wrapped "_value"/"value"/"Value" member, which may itself need resolving
            object underlying = TryGetMemberValue(rawValue, t, "_value", "value", "Value", "_Value", "m_value");
            if (underlying != null)
            {
                if (underlying is double || underlying is float || underlying is int || underlying is long)
                {
                    double numeric = Convert.ToDouble(underlying);
                    if (numeric >= 18000 && numeric <= 80000)
                    {
                        try { return DateTime.FromOADate(numeric); }
                        catch { /* fall through */ }
                    }
                }
                else
                {
                    DateTime? nested = ResolveToDateTime(underlying, depth + 1);
                    if (nested.HasValue)
                        return nested;
                }
            }

            return null;
        }

        private int? TryGetIntMember(object instance, Type t, params string[] candidateNames)
        {
            foreach (string name in candidateNames)
            {
                PropertyInfo p = t.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (p != null)
                {
                    try { return Convert.ToInt32(p.GetValue(instance)); } catch { }
                }

                FieldInfo f = t.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (f != null)
                {
                    try { return Convert.ToInt32(f.GetValue(instance)); } catch { }
                }
            }
            return null;
        }

        private object TryGetMemberValue(object instance, Type t, params string[] candidateNames)
        {
            foreach (string name in candidateNames)
            {
                PropertyInfo p = t.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (p != null)
                {
                    try { return p.GetValue(instance); } catch { }
                }

                FieldInfo f = t.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (f != null)
                {
                    try { return f.GetValue(instance); } catch { }
                }
            }
            return null;
        }

        // ── Time formatting helper (12-hour with seconds) ──
        protected string FormatTime(object timeValue)
        {
            if (timeValue == null || timeValue == DBNull.Value)
                return string.Empty;

            if (timeValue is TimeSpan ts)
                return DateTime.Today.Add(ts).ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);

            if (timeValue is DateTime dtVal)
                return dtVal.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);

            // Fall back to the same Year/Month/Day-style resolver, in case Time In
            // is actually a Date-typed field at the service layer too.
            DateTime? resolved = ResolveToDateTime(timeValue, 0);
            if (resolved.HasValue)
                return resolved.Value.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);

            // Try Hour/Minute/Second directly (public or private)
            Type t = timeValue.GetType();
            int? hour = TryGetIntMember(timeValue, t, "Hour", "hour", "hourField", "_hour");
            int? minute = TryGetIntMember(timeValue, t, "Minute", "minute", "minuteField", "_minute");
            int? second = TryGetIntMember(timeValue, t, "Second", "second", "secondField", "_second");

            if (hour.HasValue && minute.HasValue)
            {
                DateTime composed = DateTime.Today.AddHours(hour.Value).AddMinutes(minute.Value).AddSeconds(second ?? 0);
                return composed.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        //protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        //{
        //    SysErrorLog objErrorLog = new SysErrorLog();
        //    string currentMethodName = $"{this.GetType().FullName}.{nameof(chk_SelectSingle_CheckedChanged)}";

        //    try
        //    {
        //        CheckBox chkClicked = (CheckBox)sender;

        //        foreach (GridViewRow row in gvRosterChangeRequests.Rows)
        //        {
        //            if (row.RowType == DataControlRowType.DataRow)
        //            {
        //                CheckBox chkInRow = (CheckBox)row.FindControl("chk_SelectSingle");
        //                if (chkInRow != null && chkInRow != chkClicked)
        //                {
        //                    chkInRow.Checked = false;
        //                }
        //            }
        //        }

        //        if (chkClicked.Checked)
        //        {
        //            GridViewRow selectedRow = (GridViewRow)chkClicked.NamingContainer;
        //            int rowIndex = selectedRow.RowIndex;

        //            object recIdValue = gvRosterChangeRequests.DataKeys[rowIndex].Value;

        //            if (recIdValue != null && long.TryParse(recIdValue.ToString(), out long selectedRecId))
        //            {
        //                Session["selectedRecid"] = selectedRecId;
        //                BindRosterRecordGrid();
        //            }
        //            else
        //            {
        //                // FIXED: Create an exception with the error message
        //                objErrorLog.write(currentMethodName, new Exception($"Could not parse DataKey value '{recIdValue}' to long"));
        //            }
        //        }
        //        else
        //        {
        //            Session["selectedRecid"] = null;
        //            gvRosterRecord.DataSource = null;
        //            gvRosterRecord.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.write(currentMethodName, ex);
        //    }
        //}


        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(chk_SelectSingle_CheckedChanged)}";

            try
            {
                CheckBox chkClicked = (CheckBox)sender;

                foreach (GridViewRow row in gvRosterChangeRequests.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkInRow = (CheckBox)row.FindControl("chk_SelectSingle");
                        if (chkInRow != null && chkInRow != chkClicked)
                        {
                            chkInRow.Checked = false;
                        }
                    }
                }

                if (chkClicked.Checked)
                {
                    GridViewRow selectedRow = (GridViewRow)chkClicked.NamingContainer;
                    int rowIndex = selectedRow.RowIndex;
                    object recIdValue = gvRosterChangeRequests.DataKeys[rowIndex].Value;

                    if (recIdValue != null && long.TryParse(recIdValue.ToString(), out long selectedRecId))
                    {
                        Session["selectedRecid"] = selectedRecId;
                        Session["selectedLineRecid"] = null;
                        Label lblStatus = selectedRow.FindControl("lblWorkflowStatus") as Label;
                        string workflowStatus = lblStatus != null
                            ? lblStatus.Text.Trim()
                            : string.Empty;

                        // Enable/disable delete buttons based on status
                        SetButtonState(workflowStatus);
                        BindRosterRecordGrid();
                    }
                    else
                    {
                        objErrorLog.write(currentMethodName, new Exception($"Could not parse DataKey value '{recIdValue}' to long"));
                    }
                }
                else
                {
                    Session["selectedRecid"] = null;
                    Session["selectedLineRecid"] = null; // Clear line selection as well
                    gvRosterRecord.DataSource = null;
                    gvRosterRecord.DataBind();
                    btnDeleteHeader.Enabled = false;
                    btnDeleteLine.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.write(currentMethodName, ex);
            }
        }

        // Add this method for line record selection
        // Add this method for line record selection
        protected void chk_SelectLine_CheckedChanged(object sender, EventArgs e)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(chk_SelectLine_CheckedChanged)}";

            try
            {
                CheckBox chkClicked = (CheckBox)sender;

                // Uncheck all other checkboxes in the line grid
                foreach (GridViewRow row in gvRosterRecord.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkInRow = (CheckBox)row.FindControl("chk_SelectLine");
                        if (chkInRow != null && chkInRow != chkClicked)
                        {
                            chkInRow.Checked = false;
                        }
                    }
                }

                if (chkClicked.Checked)
                {
                    GridViewRow selectedRow = (GridViewRow)chkClicked.NamingContainer;
                    int rowIndex = selectedRow.RowIndex;

                    // Get the DataKey value from the grid
                    object recIdValue = gvRosterRecord.DataKeys[rowIndex].Value;

                    if (recIdValue != null && long.TryParse(recIdValue.ToString(), out long selectedLineRecId))
                    {
                        Session["selectedLineRecid"] = selectedLineRecId;
                        // Don't clear header selection - keep it to refresh the grid after delete

                        // Uncheck all header checkboxes
                        foreach (GridViewRow row in gvRosterChangeRequests.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                CheckBox chkInRow = (CheckBox)row.FindControl("chk_SelectSingle");
                                if (chkInRow != null)
                                {
                                    chkInRow.Checked = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        objErrorLog.write(currentMethodName, new Exception($"Could not parse line DataKey value '{recIdValue}' to long"));
                    }
                }
                else
                {
                    Session["selectedLineRecid"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.write(currentMethodName, ex);
            }
        }

        // Add this method for delete button click event
        //protected void btnDelete_Click(object sender, EventArgs e)
        //{
        //    SysErrorLog objErrorLog = new SysErrorLog();
        //    string currentMethodName = $"{this.GetType().FullName}.{nameof(btnDelete_Click)}";

        //    try
        //    {
        //        // Check if a line record is selected first (line takes precedence)
        //        if (Session["selectedLineRecid"] != null)
        //        {
        //            long lineRecId = Convert.ToInt64(Session["selectedLineRecid"]);
        //            long[] recIds = new long[] { lineRecId };

        //            SysOperationResult_BOL result = tASRosterChangeRequestsSvc.deleteline(recIds);

        //            if (result != null && result.isSuccess) // Use IsSuccess property instead
        //            {
        //                Session["selectedLineRecid"] = null;

        //                // Refresh the line grid if header is still selected
        //                if (Session["selectedRecid"] != null)
        //                {
        //                    BindRosterRecordGrid();
        //                }

        //                // Show success message
        //                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //                    "alert('Line record deleted successfully.');", true);
        //            }
        //            else
        //            {
        //                string errorMessage = result != null ? result.Message : "Unknown error occurred"; // Use Message property
        //                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //                    $"alert('Roster cannot be deleted while dependent Roster exist. Delete dependent Roster and try again.: {errorMessage}');", true);
        //            }
        //        }
        //        // Check if a header record is selected
        //        else if (Session["selectedRecid"] != null)
        //        {
        //            long headerRecId = Convert.ToInt64(Session["selectedRecid"]);
        //            long[] recIds = new long[] { headerRecId };

        //            SysOperationResult_BOL result = tASRosterChangeRequestsSvc.delete(recIds);

        //            if (result != null && result.isSuccess) // Use IsSuccess property instead
        //            {
        //                Session["selectedRecid"] = null;

        //                // Clear line grid
        //                gvRosterRecord.DataSource = null;
        //                gvRosterRecord.DataBind();

        //                // Refresh header grid
        //                BindGrid();

        //                // Show success message
        //                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //                    "alert('Header record deleted successfully.');", true);
        //            }
        //            else
        //            {
        //                string errorMessage = result != null ? result.Message : "Unknown error occurred"; // Use Message property
        //                ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //                    $"alert('Failed to delete header record: {errorMessage}');", true);
        //            }
        //        }
        //        else
        //        {
        //            // No record selected
        //            ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //                "alert('Please select a record to delete.');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.write(currentMethodName, ex);
        //        ScriptManager.RegisterStartupScript(this, GetType(), "alert",
        //            $"alert('Error deleting record: {ex.Message}');", true);
        //    }
        //}



        // Add this method for delete button click event
        // Delete Header button click event - uses delete method
        protected void btnDeleteHeader_Click(object sender, EventArgs e)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(btnDeleteHeader_Click)}";

            try
            {
                // Check if a header record is selected
                if (Session["selectedRecid"] != null)
                {
                    long headerRecId = Convert.ToInt64(Session["selectedRecid"]);
                    long[] recIds = new long[] { headerRecId };

                    SysOperationResult_BOL result = tASRosterChangeRequestsSvc.delete(recIds);

                    if (result != null && result.isSuccess)
                    {
                        Session["selectedRecid"] = null;

                        // Clear line grid
                        gvRosterRecord.DataSource = null;
                        gvRosterRecord.DataBind();

                        // Refresh header grid
                        BindGrid();

                        // Show success notification
                        result.AlertType = AlertType.Success.ToString();
                        result.Message = "Header record deleted successfully.";
                        NotificationMessage.showMessage(result);
                    }
                    else
                    {
                        // Show error notification
                        SysOperationResult_BOL errorResult = new SysOperationResult_BOL();
                        errorResult.AlertType = AlertType.Error.ToString();
                        errorResult.isSuccess = false;
                        errorResult.Message = result != null && !string.IsNullOrEmpty(result.Message)
                            ? result.Message
                            : "Failed to delete header record. Header cannot be deleted while dependent lines exist.";
                        NotificationMessage.showMessage(errorResult);
                    }
                }
                else
                {
                    // No header record selected - show warning notification
                    SysOperationResult_BOL warningResult = new SysOperationResult_BOL();
                    warningResult.AlertType = AlertType.Warning.ToString();
                    warningResult.isSuccess = false;
                    warningResult.Message = "Please select a header record to delete.";
                    NotificationMessage.showMessage(warningResult);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.write(currentMethodName, ex);

                // Show error notification
                SysOperationResult_BOL errorResult = new SysOperationResult_BOL();
                errorResult.AlertType = AlertType.Error.ToString();
                errorResult.isSuccess = false;
                errorResult.Message = $"Error deleting header record: {ex.Message}";
                NotificationMessage.showMessage(errorResult);
            }
        }

        // Delete Line button click event - uses deleteline method
        protected void btnDeleteLine_Click(object sender, EventArgs e)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(btnDeleteLine_Click)}";

            try
            {
                // Check if a line record is selected
                if (Session["selectedLineRecid"] != null)
                {
                    long lineRecId = Convert.ToInt64(Session["selectedLineRecid"]);
                    long[] recIds = new long[] { lineRecId };

                    SysOperationResult_BOL result = tASRosterChangeRequestsSvc.deleteline(recIds);

                    if (result != null && result.isSuccess)
                    {
                        Session["selectedLineRecid"] = null;

                        // Refresh the line grid
                        BindRosterRecordGrid();

                        // Show success notification
                        result.AlertType = AlertType.Success.ToString();
                        result.Message = "Line record deleted successfully.";
                        NotificationMessage.showMessage(result);
                    }
                    else
                    {
                        // Show error notification
                        SysOperationResult_BOL errorResult = new SysOperationResult_BOL();
                        errorResult.AlertType = AlertType.Error.ToString();
                        errorResult.isSuccess = false;
                        errorResult.Message = result != null && !string.IsNullOrEmpty(result.Message)
                            ? result.Message
                            : "Failed to delete line record.";
                        NotificationMessage.showMessage(errorResult);
                    }
                }
                else
                {
                    // No line record selected - show warning notification
                    SysOperationResult_BOL warningResult = new SysOperationResult_BOL();
                    warningResult.AlertType = AlertType.Warning.ToString();
                    warningResult.isSuccess = false;
                    warningResult.Message = "Please select a line record to delete.";
                    NotificationMessage.showMessage(warningResult);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.write(currentMethodName, ex);

                // Show error notification
                SysOperationResult_BOL errorResult = new SysOperationResult_BOL();
                errorResult.AlertType = AlertType.Error.ToString();
                errorResult.isSuccess = false;
                errorResult.Message = $"Error deleting line record: {ex.Message}";
                NotificationMessage.showMessage(errorResult);
            }
        }
        private void BindRosterRecordGrid()
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(BindRosterRecordGrid)}";

            try
            {
                if (Session["selectedRecid"] == null)
                {
                    gvRosterRecord.DataSource = null;
                    gvRosterRecord.DataBind();
                    return;
                }

                long recId = Convert.ToInt64(Session["selectedRecid"]);

                DataTable dt = tASRosterChangeRequestsSvc.retrieveLines(recId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvRosterRecord.DataSource = dt;
                    gvRosterRecord.DataBind();
                }
                else
                {
                    gvRosterRecord.DataSource = null;
                    gvRosterRecord.DataBind();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.write(currentMethodName, ex);
                gvRosterRecord.DataSource = null;
                gvRosterRecord.DataBind();
            }
        }

        private void SetButtonState(string workflowStatus)
        {
            bool canSubmit = false;
            bool canDelete = true;

            if (!string.IsNullOrWhiteSpace(workflowStatus))
            {
                string status = workflowStatus.Trim();

                // Enable Submit only for "NotSubmitted" status
                if (status.Equals("NotSubmitted", StringComparison.OrdinalIgnoreCase) ||
                    status.Equals("Not Submitted", StringComparison.OrdinalIgnoreCase) ||
                    status.Equals("Draft", StringComparison.OrdinalIgnoreCase))
                {
                    canSubmit = true;
                    canDelete = true;
                }
                // Disable Submit and Delete for "Submitted" or "Approved" status
                else if (status.Equals("Submitted", StringComparison.OrdinalIgnoreCase) ||
                         status.Equals("Approved", StringComparison.OrdinalIgnoreCase) ||
                         status.Equals("Completed", StringComparison.OrdinalIgnoreCase) ||
                         status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                {
                    canSubmit = false;
                    canDelete = false;
                }
            }

            btnSubmit.Enabled = canSubmit;
            btnDeleteHeader.Enabled = canDelete;
            btnDeleteLine.Enabled = canDelete;
        }



        // Submit button click event
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            string currentMethodName = $"{this.GetType().FullName}.{nameof(btnSubmit_Click)}";

            try
            {
                // Check if a header record is selected
                if (Session["selectedRecid"] != null)
                {
                    long headerRecId = Convert.ToInt64(Session["selectedRecid"]);
                    long[] recIds = new long[] { headerRecId }; 

                     SysOperationResult_BOL result = tASRosterChangeRequestsSvc.RosterChange_Submit(recIds);

                    if (result != null && result.isSuccess)
                    {
                        // Show success notification
                        result.AlertType = AlertType.Success.ToString();
                        result.Message = "Roster Change Request submitted successfully.";
                        NotificationMessage.showMessage(result);

                        // Refresh the grid to show updated status
                        BindGrid();

                        // Clear selection
                        Session["selectedRecid"] = null;
                        gvRosterRecord.DataSource = null;
                        gvRosterRecord.DataBind();
                    }
                    else
                    {
                        // Show error notification
                        SysOperationResult_BOL errorResult = new SysOperationResult_BOL();
                        errorResult.AlertType = AlertType.Error.ToString();
                        errorResult.isSuccess = false;
                        errorResult.Message = result != null && !string.IsNullOrEmpty(result.Message)
                            ? result.Message
                            : "Failed to submit Roster Change Request.";
                        NotificationMessage.showMessage(errorResult);
                    }
                }
                else
                {
                    // No header record selected - show warning notification
                    SysOperationResult_BOL warningResult = new SysOperationResult_BOL();
                    warningResult.AlertType = AlertType.Warning.ToString();
                    warningResult.isSuccess = false;
                    warningResult.Message = "Please select a record to submit.";
                    NotificationMessage.showMessage(warningResult);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.write(currentMethodName, ex);

                // Show error notification
                SysOperationResult_BOL errorResult = new SysOperationResult_BOL();
                errorResult.AlertType = AlertType.Error.ToString();
                errorResult.isSuccess = false;
                errorResult.Message = $"Error submitting record: {ex.Message}";
                NotificationMessage.showMessage(errorResult);
            }
        }

        public static string SecondsToHoursFormat(object secondsObj)
        {
            if (secondsObj == null || secondsObj == DBNull.Value)
                return "00:00";

            double totalSeconds;
            if (!double.TryParse(secondsObj.ToString(), out totalSeconds))
                return "00:00";

            TimeSpan ts = TimeSpan.FromSeconds(totalSeconds);

            // If hours can exceed 24, don't use ts.Hours (it resets at 24)
            int totalHours = (int)ts.TotalHours;

            return string.Format("{0:D2}:{1:D2}", totalHours, ts.Minutes);
        }
    }
}