using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal.ESS.TA
{
    public partial class TASAttendanceRegisterTeam_ListPage : MainForm
    {
        private TASAttendanceRegister tASAttendanceRegister = new TASAttendanceRegister();

        // Store the selected EmployeeId coming from the previous page
        private string selectedEmployeeId = string.Empty;

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "TASAttendanceRegisterTeam_ListPage";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                // ========== GET SELECTED EMPLOYEE ID FROM QUERY STRING ==========
                selectedEmployeeId = string.Empty;

                if (!string.IsNullOrEmpty(Request.QueryString["EmpId"]))
                {
                    try
                    {
                        selectedEmployeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
                    }
                    catch
                    {
                        selectedEmployeeId = string.Empty;
                    }
                }

                // Fallback to current user if no EmpId was passed or decryption failed
                if (string.IsNullOrWhiteSpace(selectedEmployeeId))
                {
                    selectedEmployeeId = SessionVariables.getCurrentEmployeeId();
                }

                if (!IsPostBack)
                {
                    txtFromDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                    LoadGridData();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void LoadGridData()
        {
            try
            {
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;

                DateTime dtFrom, dtTo;
                if (!DateTime.TryParse(fromDate, out dtFrom) || !DateTime.TryParse(toDate, out dtTo))
                {
                    NotificationMessage.showMessage("Please enter valid dates.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(selectedEmployeeId))
                {
                    NotificationMessage.showMessage("Employee ID is missing.");
                    return;
                }

                // Call the service with the selected EmployeeId
                DataTable dt = tASAttendanceRegister.retriveEmployeeID(fromDate, toDate, selectedEmployeeId);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gridView.DataSource = dt;
                    gridView.DataBind();
                    SessionVariables.setSessionDataTable(dt);
                }
                else
                {
                    gridView.DataSource = null;
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

                if (int.TryParse(s, out int totalSeconds))
                {
                    dt = DateTime.Today.AddSeconds(totalSeconds);
                }
                else
                {
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




        //protected void btnBack_Click(object sender, EventArgs e)
        //{
        //    // Go back to HR Employees Report To Me page
        //    Response.Redirect("~/ESS/HR/HREmployeesReportToMe_ListPage.aspx");
        //}

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