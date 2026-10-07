using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.TA
{
    public partial class TASAttendanceRegister_ListPage : MainForm
    {
        private TASAttendanceRegister tASAttendanceRegister = new TASAttendanceRegister();

        //protected override void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        pageMenuId = "TASAttendanceRegister_ListPage";
        //        base.Page_Load(sender, e);

        //        if (!isUserAuthenticated)
        //            return;

        //        if (!IsPostBack)
        //        {
        //            // Set default date range
        //            txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        //            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

        //            // Load data with default date range
        //            LoadGridData();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowError(ex);
        //    }
        //}

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TASAttendanceRegister_ListPage";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    // Set default dates
                    txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                    // Force load after dates are set
                    LoadGridData();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        //private void LoadGridData()
        //{
        //    try
        //    {
        //        string fromDate = txtFromDate.Text;
        //        string toDate = txtToDate.Text;

        //        // Validate dates
        //        DateTime dtFrom, dtTo;
        //        if (!DateTime.TryParse(fromDate, out dtFrom) || !DateTime.TryParse(toDate, out dtTo))
        //        {
        //            NotificationMessage.showMessage("Please enter valid dates.");
        //            return;
        //        }

        //        // Get current employee ID
        //        string employeeId = SessionVariables.getCurrentEmployeeId();

        //        // Retrieve data
        //        DataTable dt = tASAttendanceRegister.retriveEmployeeReportees( fromDate, toDate);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            gridView.DataSource = dt;
        //            gridView.DataBind();
        //            SessionVariables.setSessionDataTable(dt);
        //        }
        //        else
        //        {
        //            gridView.DataSource = null;
        //            gridView.DataBind();
        //            SessionVariables.setSessionDataTable(null);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowError(ex);
        //    }
        //}

        private void LoadGridData()
        {
            try
            {
                // If dates are empty (can happen on first load), set defaults again
                if (string.IsNullOrWhiteSpace(txtFromDate.Text))
                    txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

                if (string.IsNullOrWhiteSpace(txtToDate.Text))
                    txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                string fromDate = txtFromDate.Text.Trim();
                string toDate = txtToDate.Text.Trim();

                DateTime dtFrom, dtTo;
                if (!DateTime.TryParse(fromDate, out dtFrom) || !DateTime.TryParse(toDate, out dtTo))
                {
                    NotificationMessage.showMessage("Please enter valid dates.");
                    return;
                }

                DataTable dt = tASAttendanceRegister.retriveEmployeeReportees(fromDate, toDate);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gridView.DataSource = dt;
                    gridView.DataBind();
                    SessionVariables.setSessionDataTable(dt);
                }
                else
                {
                    gridView.DataSource = new DataTable();
                    gridView.DataBind();
                    SessionVariables.setSessionDataTable(null);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate date range
                DateTime fromDate, toDate;
                if (!DateTime.TryParse(txtFromDate.Text, out fromDate) || !DateTime.TryParse(txtToDate.Text, out toDate))
                {
                    NotificationMessage.showMessage("Please enter valid dates.");
                    return;
                }

                if (fromDate > toDate)
                {
                    NotificationMessage.showMessage("From Date cannot be greater than To Date.");
                    return;
                }

                // Load filtered data
                LoadGridData();
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            LoadGridData();
        }

        protected void btnMarkAttendance_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selectedEmployeeIds = new List<string>();
                List<string> selectedEmployeeNames = new List<string>();
                List<string> selectedAttendanceDates = new List<string>();
                List<string> selectedClockIns = new List<string>();
                List<string> selectedClockOuts = new List<string>();

                foreach (GridViewRow gridViewRow in gridView.Rows)
                {
                    CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                    if (chkSelectRow != null && chkSelectRow.Checked)
                    {
                        string clockIn = ((Label)gridViewRow.FindControl("lblClockIn"))?.Text;
                        string clockOut = ((Label)gridViewRow.FindControl("lblClockOut"))?.Text;

                        // Check if record is absent (12:00:00 AM for both ClockIn and ClockOut)
                        if (clockIn == "12:00:00 AM" && clockOut == "12:00:00 AM")
                        {
                            selectedEmployeeIds.Add(((Label)gridViewRow.FindControl("lblEmployeeId"))?.Text);
                            selectedEmployeeNames.Add(((Label)gridViewRow.FindControl("lblEmployeeName"))?.Text);
                            selectedAttendanceDates.Add(((Label)gridViewRow.FindControl("lblAttendanceDate"))?.Text);
                            selectedClockIns.Add(clockIn);
                            selectedClockOuts.Add(clockOut);
                        }
                        else
                        {
                            NotificationMessage.showMessage("Please select only Absent Records (Clock In and Clock Out should be 12:00:00 AM).");
                            return;
                        }
                    }
                }

                if (selectedEmployeeIds.Count == 0)
                {
                    NotificationMessage.showMessage("Please select at least one absent record to mark attendance.");
                    return;
                }

                // Build query string for selected records
                StringBuilder queryStringBuilder = new StringBuilder();
                for (int i = 0; i < selectedEmployeeIds.Count; i++)
                {
                    queryStringBuilder.AppendFormat("EmployeeId={0}&EmployeeName={1}&ClockIn={2}&ClockOut={3}&AttendanceDate={4}&",
                        HttpUtility.UrlEncode(selectedEmployeeIds[i]),
                        HttpUtility.UrlEncode(selectedEmployeeNames[i]),
                        HttpUtility.UrlEncode(selectedClockIns[i]),
                        HttpUtility.UrlEncode(selectedClockOuts[i]),
                        HttpUtility.UrlEncode(selectedAttendanceDates[i]));
                }

                string finalQuery = queryStringBuilder.ToString().TrimEnd('&');
                string script = $"openPopupPanel('/ESS/TA/TASMarkAttendance.aspx?{finalQuery}')";
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenPopup", script, true);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        protected void btnEditAttendance_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> selectedEmployeeIds = new List<string>();
                List<string> selectedEmployeeNames = new List<string>();
                List<string> selectedAttendanceDates = new List<string>();
                List<string> selectedClockIns = new List<string>();
                List<string> selectedClockOuts = new List<string>();

                foreach (GridViewRow gridViewRow in gridView.Rows)
                {
                    CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                    if (chkSelectRow != null && chkSelectRow.Checked)
                    {
                        selectedEmployeeIds.Add(((Label)gridViewRow.FindControl("lblEmployeeId"))?.Text);
                        selectedEmployeeNames.Add(((Label)gridViewRow.FindControl("lblEmployeeName"))?.Text);
                        selectedAttendanceDates.Add(((Label)gridViewRow.FindControl("lblAttendanceDate"))?.Text);
                        selectedClockIns.Add(((Label)gridViewRow.FindControl("lblClockIn"))?.Text);
                        selectedClockOuts.Add(((Label)gridViewRow.FindControl("lblClockOut"))?.Text);
                    }
                }

                if (selectedEmployeeIds.Count == 0)
                {
                    NotificationMessage.showMessage("Please select at least one record to edit.");
                    return;
                }

                // Build query string for selected records
                StringBuilder queryStringBuilder = new StringBuilder();
                for (int i = 0; i < selectedEmployeeIds.Count; i++)
                {
                    queryStringBuilder.AppendFormat("EmployeeId={0}&EmployeeName={1}&ClockIn={2}&ClockOut={3}&AttendanceDate={4}&",
                        HttpUtility.UrlEncode(selectedEmployeeIds[i]),
                        HttpUtility.UrlEncode(selectedEmployeeNames[i]),
                        HttpUtility.UrlEncode(selectedClockIns[i]),
                        HttpUtility.UrlEncode(selectedClockOuts[i]),
                        HttpUtility.UrlEncode(selectedAttendanceDates[i]));
                }

                string finalQuery = queryStringBuilder.ToString().TrimEnd('&');
                string script = $"openPopupPanel('/ESS/TA/TASAttendanceRegister_EditRecord.aspx?{finalQuery}')";
                ScriptManager.RegisterStartupScript(this, GetType(), "OpenPopup", script, true);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            LoadGridData();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Add row data attributes for client-side operations
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (drv != null)
                {
                    var rowData = new Dictionary<string, string>();
                    foreach (DataColumn col in drv.Row.Table.Columns)
                    {
                        rowData[col.ColumnName] = drv[col.ColumnName]?.ToString() ?? "";
                    }
                    string json = new JavaScriptSerializer().Serialize(rowData);
                    e.Row.Attributes["data-rowjson"] = json;
                }

                Label lblAttendanceDate = e.Row.FindControl("lblAttendanceDate") as Label;

                if (lblAttendanceDate != null && !string.IsNullOrWhiteSpace(lblAttendanceDate.Text))
                {
                    DateTime planDate;

                    if (DateTime.TryParse(lblAttendanceDate.Text, out planDate))
                    {
                        lblAttendanceDate.Text = planDate.ToString("dd/M/yyyy");
                    }
                }
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
                if (gridViewRow.RowType == DataControlRowType.DataRow)
                {
                    if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                    {
                        // Implement update logic here
                        // This is a placeholder - you need to implement the actual update
                        NotificationMessage.showMessage("Update functionality to be implemented.");

                        gridView.EditIndex = -1;
                        LoadGridData();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            LoadGridData();
        }

        protected string FormatDate(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
                return string.Empty;

            if (DateTime.TryParse(dateValue.ToString(), out DateTime dt))
                return dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            return dateValue.ToString();
        }

        protected string FormatTime(object timeValue)
        {
            if (timeValue == null || timeValue == DBNull.Value)
                return string.Empty;

            DateTime dt;

            if (timeValue is DateTime)
            {
                dt = (DateTime)timeValue;
            }
            else
            {
                var s = timeValue.ToString().Trim();

                // Try parsing as seconds since midnight
                if (int.TryParse(s, out int totalSeconds))
                {
                    dt = DateTime.Today.AddSeconds(totalSeconds);
                }
                else
                {
                    // Try common time formats
                    string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm" };
                    if (!DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        if (!DateTime.TryParse(s, out dt))
                        {
                            return s;
                        }
                    }
                }
            }

            return dt.ToString("h:mm:ss tt", CultureInfo.InvariantCulture);
        }

        private void ShowError(Exception ex)
        {
            SysErrorLog objErrorLog = new SysErrorLog();
            System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
            string currentMethodName = currentMethod.DeclaringType.FullName;
            objErrorLog.write(currentMethodName, ex);

            NotificationMessage.showMessage("An error occurred: " + ex.Message);
        }
    }
}