using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.TASEmployeeRosterSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class TASEmployeeRoster_ListPage : MainForm
    {
        private TASEmployeeRoster tASEmployeeRoster = new TASEmployeeRoster();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = tASEmployeeRoster.tableName;
                pageMenuId = "TASEmployeeRoster_ListPage";

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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;

                // You can then use these values as needed, for example:
                // Call a method that filters the data based on these dates
                FilterData(fromDate, toDate);
                // Save so WebMethod can read them without parameters
                HttpContext.Current.Session["FilterFromDate"] = fromDate;
                HttpContext.Current.Session["FilterToDate"] = toDate;
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
            DataTable filteredData = tASEmployeeRoster.retrieveEmployeeData(fromDate, toDate);

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


        private void getGridDataTable()
        {
            DataTable dt = tASEmployeeRoster.retrieveEmployeeData(txtFromDate.Text, txtToDate.Text);
            SessionVariables.setSessionDataTable(dt);
        }
        protected string FormatTime(object timeValue)
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
                    string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm", "hh:mm tt", "hh:mm:ss tt" };
                    if (DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        // parsed
                    }
                    else if (!DateTime.TryParse(s, out dt))
                    {
                        // 4) Give up if cannot parse
                        return s;
                    }
                }
            }

            // ✅ Always return in 12-hour format with seconds and space before AM/PM
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

        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    DataTable dataTable = tASEmployeeRoster.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    dr["EmployeeId"] = ((TextBox)gridViewRow.FindControl("txtEmployeeId")).Text;
                    dr["ShiftId"] = ((DropDownList)gridViewRow.FindControl("ddlShiftId")).SelectedValue;
                    dr["FromDate"] = ((TextBox)gridViewRow.FindControl("txtFromDate")).Text;
                    dr["ToDate"] = ((TextBox)gridViewRow.FindControl("txtToDate")).Text;
                    dr["ShiftStartTime"] = ((TextBox)gridViewRow.FindControl("txtShiftStartTime")).Text;
                    dr["ShiftEndTime"] = ((TextBox)gridViewRow.FindControl("txtShiftEndTime")).Text;
                    dr["ShiftBreakTime"] = ((TextBox)gridViewRow.FindControl("txtShiftBreakTime")).Text;
                    dr["ShiftGraceTime"] = ((TextBox)gridViewRow.FindControl("txtShiftGraceTime")).Text;
                    dr["ShiftHours"] = ((TextBox)gridViewRow.FindControl("txtShiftHours")).Text;
                    dr["Location"] = ((TextBox)gridViewRow.FindControl("txtLocation")).Text;
                    dr["GENERATIONTYPE"] = ((TextBox)gridViewRow.FindControl("txtLocation")).Text;
                    // dr["Type"] = ((DropDownList)gridViewRow.FindControl("ddlType")).SelectedValue;

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    dataTable.Rows.Add(dr);

                    SysOperationResult_BOL operationResult_BOL = tASEmployeeRoster.update(dataTable, recId);
                    bool result = operationResults(operationResult_BOL);

                    if (result)
                    {
                        gridView.EditIndex = -1;
                        reBindGrid();
                    }
                    else
                    {
                        bindGrid();
                    }
                }
            }
        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            gridView.EditIndex = -1;
            bindGrid();
        }

      
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<long> recordsId = new List<long>();

                // ✅ Collect selected record IDs
                foreach (GridViewRow gridViewRow in gridView.Rows)
                {
                    CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                    if (chkSelectRow != null && chkSelectRow.Checked)
                    {
                        Label lblRecId = gridViewRow.FindControl("lblRecId") as Label;
                        if (lblRecId != null)
                        {
                            long recId = 0;
                            if (Int64.TryParse(lblRecId.Text, out recId) && recId > 0)
                            {
                                recordsId.Add(recId);
                            }
                        }
                    }
                }

                if (recordsId.Count > 0)
                {
                    // ✅ Call delete service
                    SysOperationResult_BOL operationResult_BOL = tASEmployeeRoster.delete(recordsId.ToArray());

                    bool result = operationResults(operationResult_BOL);
                    if (result)
                    {
                        // ✅ Success - refresh grid
                        gridView.EditIndex = -1;
                        reBindGrid();

                        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteSuccess",
                            "alert('Selected roster record(s) deleted successfully.');", true);
                    }
                    else
                    {
                        // ✅ Failed - reload data
                        bindGrid();

                        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteFailed",
                            "alert('Error occurred while deleting the record(s).');", true);
                    }
                }
                else
                {
                    // ✅ No record selected
                    ScriptManager.RegisterStartupScript(this, GetType(), "NoSelection",
                        "alert('Please select at least one record to delete.');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "DeleteError",
                    $"alert('An error occurred: {ex.Message}');", true);
            }
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

            // Only filter if the dates are valid (fromDate <= toDate)
            if (fromDate <= toDate)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                    // Convert DateTime to string (format as needed, e.g., "yyyy-MM-dd")
                    string fromDateString = fromDate.ToString("yyyy-MM-dd");
                    string toDateString = toDate.ToString("yyyy-MM-dd");

                    // Filter the data source based on the date range before binding
                    var filteredData = tASEmployeeRoster.retrieveAllwithFilters(fromDateString, toDateString);

                    DropDownList ddlShiftId = gridViewRow.FindControl("ddlShiftId") as DropDownList;
                    ddlShiftId.DataSource = filteredData;
                    ddlShiftId.DataBind();

                    DropDownList ddlType = gridViewRow.FindControl("ddlType") as DropDownList;
                    ddlType.DataSource = Enum.GetNames(typeof(Type));
                    ddlType.DataBind();

                    // Bind the selected values based on the data row
                    ddlShiftId.SelectedValue = dataRowView["ShiftId"].ToString();
                    ddlType.SelectedValue = dataRowView["Type"].ToString();


                }
            }
            else
            {
                // Handle invalid date range
                // Optionally show an error message or clear the dropdowns
            }
            if (e.Row.RowType == DataControlRowType.DataRow &&
       (e.Row.RowState & DataControlRowState.Edit) == 0) // not edit mode
            {
                Label lblType = e.Row.FindControl("lblType") as Label;
                if (lblType != null && lblType.Text == "StandardTime")
                {
                    lblType.Text = "Standard Time";
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

        protected string GetGridViewRowSelectionScript()
        {
            var script = new StringBuilder();
            foreach (GridViewRow row in gridView.Rows)
            {
                var chk = row.FindControl("chk_SelectSingle") as CheckBox;
                var lbl = row.FindControl("lblRecId") as Label;
                if (chk != null && lbl != null)
                {
                    script.AppendLine($"if (document.getElementById('{chk.ClientID}').checked) {{");
                    script.AppendLine($"  selectedIds.push('{lbl.Text}');");
                    script.AppendLine("}");
                }
            }
            return script.ToString();
        }


        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);

        }
        protected string FormatDate(object dateValue)
        {
            if (dateValue == null || dateValue == DBNull.Value)
                return string.Empty;

            if (DateTime.TryParse(dateValue.ToString(), out DateTime dt))
                return dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

            return dateValue.ToString();
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
            TASEmployeeRoster tASEmployeeRoster = new TASEmployeeRoster();
            DataTable dt = tASEmployeeRoster.retrieveEmployeeReportees(fromDate, toDate);

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
