using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using static System.Net.Mime.MediaTypeNames;

namespace DynamicsPortal.ESS.TA
{
    public partial class TASEmployeeAdjustmentLines_ListPage : MainForm
    {
        private TASEmployeeAdjustmentLines tASEmployeeAdjutmentlines = new TASEmployeeAdjustmentLines();
        //private TASEmployeeAdjustments tASEmployeeAdjutment = new TASEmployeeAdjustments();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = tASEmployeeAdjutmentlines.tablename;
                pageMenuId = "TASEmployeeAdjustmentLines_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                    // Initially load the data for the default date range
                    reBindGrid();

                    // ✅ Save defaults to session on first load
                    HttpContext.Current.Session["FilterFromDate"] = txtFromDate.Text;
                    HttpContext.Current.Session["FilterToDate"] = txtToDate.Text;
                }

                if (Request["__EVENTTARGET"] == "RefreshGrid")
                {
                    reBindGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                System.Reflection.MethodBase currentMethod = System.Reflection.MethodBase.GetCurrentMethod();
                string currentMethodName = currentMethod.DeclaringType.FullName;
                objErrorLog.write(currentMethodName, ex);
            }
            finally { }



        }
        private void getGridDataTable()
        {
            DataTable dt = tASEmployeeAdjutmentlines.retriveEmployeeReportees(txtFromDate.Text, txtToDate.Text);

            SessionVariables.setSessionDataTable(dt);
        }



        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;

                // You can then use these values as needed, for example:
                // Call a method that filters the data based on these dates
                FilterData(fromDate, toDate);
                // ✅ Save defaults to session on first load
                HttpContext.Current.Session["FilterFromDate"] = txtFromDate.Text;
                HttpContext.Current.Session["FilterToDate"] = txtToDate.Text;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You can log the error or display a message to the user
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        private void FilterData(string fromDate, string toDate)
        {
            DataTable filteredData = tASEmployeeAdjutmentlines.retriveEmployeeReportees(fromDate, toDate);

            if (filteredData != null && filteredData.Rows.Count > 0)
            {
                gridView.DataSource = filteredData;
                gridView.DataBind();
                SessionVariables.setSessionDataTable(filteredData);
            }
            else
            {
                // Clear GridView when there's no data
                gridView.DataSource = null;
                gridView.DataBind();
                SessionVariables.setSessionDataTable(null);
            }
        }

        protected string FormatTime(object timeValue)
        {
            if (timeValue == null || timeValue == DBNull.Value)
                return String.Empty;

            DateTime dt;

            // 1) Already a DateTime?
            if (timeValue is DateTime t1)
            {
                dt = t1;
            }
            else
            {
                var s = timeValue.ToString().Trim();

                // 2) Seconds-since-midnight?
                if (int.TryParse(s, out int totalSeconds))
                {
                    dt = DateTime.Today.AddSeconds(totalSeconds);
                }
                else
                {
                    // 3) Known string formats
                    string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm" };
                    if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        // parsed successfully
                    }
                    else if (!DateTime.TryParse(s, out dt))
                    {
                        // 4) Give up
                        return s;
                    }
                }
            }

            // ✅ Format with space before AM/PM
            return dt.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        protected string FormatTimeNew(object timeValue)
        {
            if (timeValue == null || timeValue == DBNull.Value)
                return String.Empty;

            DateTime dt;

            // 1) Already a DateTime?
            if (timeValue is DateTime t1)
                dt = t1;
            else
            {
                var s = timeValue.ToString().Trim();
                // 2) Seconds-since-midnight?
                if (int.TryParse(s, out int totalSeconds))
                    dt = DateTime.Today.AddSeconds(totalSeconds);
                else
                {
                    // 3) Known string formats?
                    string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm" };
                    if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        // parsed
                    }
                    else if (!DateTime.TryParse(s, out dt))
                    {
                        // 4) Give up
                        return s;
                    }
                }
            }

            // Final: 24-hour format with hours and minutes only
            return dt.ToString("HH:mm", CultureInfo.InvariantCulture);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }

        protected void bindGrid()
        {
            DataTable dt = SessionVariables.getSessionDataTable();
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;

            // Check if the row is a DataRow
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            // Apply the date filters to the data source
            DateTime fromDate = DateTime.Parse(txtFromDate.Text); // Get FromDate
            DateTime toDate = DateTime.Parse(txtToDate.Text); // Get ToDate



            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                //// Convert DateTime to string (format as needed, e.g., "yyyy-MM-dd")
                string fromDateString = fromDate.ToString("yyyy-MM-dd");
                string toDateString = toDate.ToString("yyyy-MM-dd");

                // Filter the data source based on the date range before binding
                var filteredData = tASEmployeeAdjutmentlines.retrieveAll(fromDateString, toDateString);
                DropDownList ddlShiftId = gridViewRow.FindControl("ddlShiftId") as DropDownList;
                // ddlShiftId.DataSource = filteredData;
                ddlShiftId.DataBind();

                DropDownList ddlType = gridViewRow.FindControl("ddlType") as DropDownList;
                ddlType.DataSource = Enum.GetNames(typeof(Type));
                ddlType.DataBind();

                // Bind the selected values based on the data row
                ddlShiftId.SelectedValue = dataRowView["ShiftId"].ToString();
                ddlType.SelectedValue = dataRowView["Type"].ToString();
            }
            else
            {
                // Handle invalid date range
                // Optionally show an error message or clear the dropdowns
            }
            if (e.Row.RowType == DataControlRowType.DataRow &&
                 (e.Row.RowState & DataControlRowState.Edit) == 0) // not edit mode
            {
                Label lblWFSTATUS = e.Row.FindControl("lblWFSTATUS") as Label;
                if (lblWFSTATUS != null && lblWFSTATUS.Text == "PendingApproval")
                {
                    lblWFSTATUS.Text = "Pending Approval";
                }
                else if (lblWFSTATUS != null && lblWFSTATUS.Text == "NotSubmitted")
                {
                    lblWFSTATUS.Text = "Draft";
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (drv != null)
                {
                    var rowData = new System.Collections.Generic.Dictionary<string, string>();
                    foreach (DataColumn col in drv.Row.Table.Columns)
                    {
                        rowData[col.ColumnName] = drv[col.ColumnName]?.ToString() ?? "";
                    }
                    string json = new System.Web.Script.Serialization.JavaScriptSerializer()
                                      .Serialize(rowData);
                    e.Row.Attributes["data-rowjson"] = json;
                }
            }
        }




        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);

        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    List<long> recordsId = new List<long>();
        //    foreach (GridViewRow gridViewRow in gridView.Rows)
        //    {
        //        CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
        //        if (chkSelectRow != null && chkSelectRow.Checked)
        //        {
        //            int rowIndex = gridViewRow.RowIndex;
        //            if (rowIndex >= 0 && rowIndex < gridView.DataKeys.Count)
        //            {
        //                long recId = 0;
        //                Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
        //                if (recId > 0)
        //                {
        //                    recordsId.Add(recId);
        //                }
        //            }
        //        }
        //    }

        //    if (recordsId.Count > 0)
        //    {
        //        SysOperationResult_BOL operationResult_BOL = eSSWorkflow.pREmployeeAdjustmentRequest_Submit(recordsId.ToArray());
        //        bool result = operationResults(operationResult_BOL);
        //        if (result)
        //        {
        //            gridView.EditIndex = -1;
        //            reBindGrid();
        //        }
        //        else
        //        {
        //            bindGrid();
        //        }
        //    }
        //}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                List<long> lineRecIdsToSubmit = new List<long>();

                // Loop through GridView rows and get selected ones
                foreach (GridViewRow gridViewRow in gridView.Rows)
                {
                    CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                    if (chkSelectRow != null && chkSelectRow.Checked)
                    {
                        // Get RecId of the selected line
                        string recIdStr = ((Label)gridViewRow.FindControl("lblRecId")).Text;
                        if (long.TryParse(recIdStr, out long recId) && !lineRecIdsToSubmit.Contains(recId))
                        {
                            lineRecIdsToSubmit.Add(recId);
                        }
                    }
                }

                // Submit selected RecIds
                if (lineRecIdsToSubmit.Count > 0)
                {
                    SysOperationResult_BOL operationResult_BOL = tASEmployeeAdjutmentlines.pREmployeeAdjustmentRequest_Submit(lineRecIdsToSubmit.ToArray());
                    bool result = operationResults(operationResult_BOL);

                    if (result)
                    {
                        gridView.EditIndex = -1;
                        reBindGrid();
                        ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                            $"alert('{lineRecIdsToSubmit.Count} records submitted successfully!');", true);
                    }
                    else
                    {
                        bindGrid();
                        ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                            "alert( " + operationResult_BOL.Message +");", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showWarning",
                        "alert('Please select at least one record to submit.');", true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "showException",
                    $"alert('Error: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }
        protected string FormatDate(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
                return string.Empty;

            if (DateTime.TryParse(dateValue.ToString(), out DateTime dt))
                return dt.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            return dateValue.ToString();
        }

        protected string FormatDates(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
                return string.Empty;

            if (DateTime.TryParse(dateValue.ToString(), out DateTime dt))
                return dt.ToString("M/d/yyyy", CultureInfo.InvariantCulture);

            return dateValue.ToString();
        }

        // Put these in your code-behind class
        private static readonly TimeZoneInfo PkTz = GetPakistanTz();
        private const bool ASSUME_UNSPECIFIED_IS_UTC = false; // set true if your DB stores UTC without a 'Z'

        private static TimeZoneInfo GetPakistanTz()
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time"); } // Windows/IIS
            catch { return TimeZoneInfo.FindSystemTimeZoneById("Asia/Karachi"); }        // Linux/containers
        }

        protected string FormatDateTime(object value)
        {
            if (value == null || value == DBNull.Value) return string.Empty;

            DateTime local;

            // Already a DateTime?
            if (value is DateTime dt)
            {
                if (dt.Kind == DateTimeKind.Utc)
                    local = TimeZoneInfo.ConvertTimeFromUtc(dt, PkTz);
                else if (dt.Kind == DateTimeKind.Unspecified && ASSUME_UNSPECIFIED_IS_UTC)
                    local = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(dt, DateTimeKind.Utc), PkTz);
                else
                    local = TimeZoneInfo.ConvertTime(dt, PkTz);
            }
            // DateTimeOffset keeps the original offset/Z
            else if (value is DateTimeOffset dto)
            {
                local = TimeZoneInfo.ConvertTime(dto, PkTz).DateTime;
            }
            else
            {
                var s = value.ToString().Trim();

                // Try offset-aware first (e.g., 2025-08-27T09:30:00Z or +03:00)
                if (DateTimeOffset.TryParse(s, null, DateTimeStyles.RoundtripKind, out var parsedDto))
                {
                    local = TimeZoneInfo.ConvertTime(parsedDto, PkTz).DateTime;
                }
                // Then try plain DateTime; assume local unless you flip the flag
                else if (DateTime.TryParse(s, CultureInfo.InvariantCulture,
                                           ASSUME_UNSPECIFIED_IS_UTC ? DateTimeStyles.AssumeUniversal : DateTimeStyles.AssumeLocal,
                                           out var parsedDt))
                {
                    if ((ASSUME_UNSPECIFIED_IS_UTC && parsedDt.Kind != DateTimeKind.Utc) || parsedDt.Kind == DateTimeKind.Utc)
                        local = TimeZoneInfo.ConvertTimeFromUtc(parsedDt.ToUniversalTime(), PkTz);
                    else
                        local = TimeZoneInfo.ConvertTime(parsedDt, PkTz);
                }
                else
                {
                    return s; // give up — show raw
                }
            }

            // Final: 12-hour with AM/PM
            return local.ToString("MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
        }


        protected void lnkEmployeeId_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnk = (LinkButton)sender;
                string employeeId = lnk.CommandArgument;
                GridViewRow row = (GridViewRow)lnk.NamingContainer;

                // Store EmployeeId in session
                Session["SelectedEmployeeId"] = employeeId;
                string shiftId = ((Label)row.FindControl("lblShiftId"))?.Text ?? string.Empty;
                string employeeName = ((Label)row.FindControl("lblEmployeeName"))?.Text ?? string.Empty;
                string clockInDate = ((Label)row.FindControl("lblClockInDate"))?.Text ?? string.Empty;
                string generationType = ((Label)row.FindControl("lblGenerationType"))?.Text ?? string.Empty;
                string wfStatus = ((Label)row.FindControl("lblWFSTATUS"))?.Text ?? string.Empty;
                string attendanceStatus = ((Label)row.FindControl("lblAttendanceStatus"))?.Text ?? string.Empty; //
                string attendanceDate = ((Label)row.FindControl("lblDate"))?.Text ?? string.Empty; //

                // Store FromDate and ToDate in session
                Session["SelectedFromDate"] = txtFromDate.Text;
                Session["SelectedToDate"] = txtToDate.Text;
                Session["SelectedEmployeeName"] = employeeName;
                Session["SelectedShiftId"] = shiftId;
                Session["SelectedClockInDate"] = clockInDate;
                Session["SelectedGenerationType"] = generationType;
                Session["SelectedWFStatus"] = wfStatus;
                Session["SelectedAttendanceStatus"] = attendanceStatus;
                Session["SelectedAttendanceDate"] = attendanceDate;


                // Redirect to TASEmployeeAdjustments_ListPage.aspx
                Response.Redirect("~/ESS/TA/TASEmployeeAdjustments_ListPage.aspx");
            }
            catch (Exception ex)

            {
                // Optional: log error
                Console.WriteLine("Error in Employee link click: " + ex.Message);
                ScriptManager.RegisterStartupScript(this, GetType(), "showException",
                    $"alert('Error: {ex.Message.Replace("'", "\\'")}');", true);
            }
        }




        private static string FormatLabel(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return fieldName;
            // Fix: insert space before capitals that follow a lowercase letter
            // prevents leading space on first capital
            return System.Text.RegularExpressions.Regex.Replace(
                fieldName, "(?<=[a-z])([A-Z])", " $1").Trim();
        }

        [WebMethod(EnableSession = true)]
        public static string GetAllAvailableColumns()
        {
            // Read from Session — already set by FilterData() / reBindGrid()
            string fromDate = HttpContext.Current.Session["FilterFromDate"]?.ToString()
                              ?? DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            string toDate = HttpContext.Current.Session["FilterToDate"]?.ToString()
                              ?? DateTime.Now.ToString("yyyy-MM-dd");

            TASEmployeeAdjustmentLines tASEmployeeAdjutmentlines = new TASEmployeeAdjustmentLines();
            DataTable dt = tASEmployeeAdjutmentlines.retriveEmployeeReportees(fromDate, toDate);

            var columns = dt.Columns
                .Cast<DataColumn>()
                .Select(col => new
                {
                    Field = col.ColumnName,
                    Label = FormatLabel(col.ColumnName),
                    Visible = true
                })
                .ToList();

            return new JavaScriptSerializer().Serialize(columns);
        }

    }
}