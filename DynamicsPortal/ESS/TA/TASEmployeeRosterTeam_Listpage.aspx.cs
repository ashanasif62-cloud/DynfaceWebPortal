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
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;

namespace DynamicsPortal.ESS.TA
{
    public partial class TASEmployeeRosterTeam_Listpage : MainForm
    {
        private TASEmployeeRoster tASEmployeeRoster = new TASEmployeeRoster();

        // Store the selected EmployeeId coming from the previous page
        private string selectedEmployeeId = string.Empty;

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = tASEmployeeRoster.tableName;
                pageMenuId = "TASEmployeeRosterTeam_ListPage";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                // ========== GET SELECTED EMPLOYEE ID FROM QUERY STRING ==========
                if (!string.IsNullOrEmpty(Request.QueryString["EmpId"]))
                {
                    selectedEmployeeId = SecureQueryString.decrypt(Request.QueryString["EmpId"]);
                }

                // Fallback to current user if no EmpId was passed
                if (string.IsNullOrEmpty(selectedEmployeeId))
                {
                    selectedEmployeeId = SessionVariables.getCurrentEmployeeId();
                }

                if (!IsPostBack)
                {
                    txtFromDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                    reBindGrid();

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
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;

                FilterData(fromDate, toDate);

                HttpContext.Current.Session["FilterFromDate"] = fromDate;
                HttpContext.Current.Session["FilterToDate"] = toDate;
            }
            catch (Exception ex)
            {
                SysErrorLog objErrorLog = new SysErrorLog();
                objErrorLog.write($"{this.GetType().FullName}.{nameof(btnFilter_Click)}", ex);
            }
        }

        private void FilterData(string fromDate, string toDate)
        {
            // Pass the selected employee id
            DataTable filteredData = tASEmployeeRoster.retrieveEmployeeId(fromDate, toDate, selectedEmployeeId);

            if (filteredData != null && filteredData.Rows.Count > 0)
            {
                gridView.DataSource = filteredData;
                gridView.DataBind();
                SessionVariables.setSessionDataTable(filteredData);
            }
            else
            {
                gridView.DataSource = null;
                gridView.DataBind();
                SessionVariables.setSessionDataTable(null);
            }
        }

        private void getGridDataTable()
        {
            // Pass the selected employee id
            DataTable dt = tASEmployeeRoster.retrieveEmployeeId(txtFromDate.Text, txtToDate.Text, selectedEmployeeId);
            SessionVariables.setSessionDataTable(dt);
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
            e.Cancel = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<long> recordsId = new List<long>();

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
                    SysOperationResult_BOL operationResult_BOL = tASEmployeeRoster.delete(recordsId.ToArray());
                    bool result = operationResults(operationResult_BOL);

                    if (result)
                    {
                        gridView.EditIndex = -1;
                        reBindGrid();
                        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteSuccess",
                            "alert('Selected roster record(s) deleted successfully.');", true);
                    }
                    else
                    {
                        bindGrid();
                        ScriptManager.RegisterStartupScript(this, GetType(), "DeleteFailed",
                            "alert('Error occurred while deleting the record(s).');", true);
                    }
                }
                else
                {
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
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            if ((e.Row.RowState & DataControlRowState.Edit) == 0)
            {
                Label lblType = e.Row.FindControl("lblType") as Label;
                if (lblType != null && lblType.Text == "StandardTime")
                {
                    lblType.Text = "Standard Time";
                }
            }

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

        // ==================== HELPER METHODS ====================

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

            if (timeValue is DateTime t1)
            {
                dt = t1;
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
                    string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm", "hh:mm tt", "hh:mm:ss tt" };

                    if (!DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        if (!DateTime.TryParse(s, out dt))
                            return s;
                    }
                }
            }

            return dt.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        protected string FormatTimeNew(object timeValue)
        {
            if (timeValue == null || timeValue == DBNull.Value)
                return string.Empty;

            DateTime dt;

            if (timeValue is DateTime t1)
            {
                dt = t1;
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
                            return s;
                    }
                }
            }

            return dt.ToString("HH:mm", CultureInfo.InvariantCulture);
        }


        protected void btnBack_Click(object sender, EventArgs e)
        {
            // Go back to HR Employees Report To Me page
            Response.Redirect("~/ESS/HR/HREmployeesReportToMe_ListPage.aspx");
        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

        private static string FormatLabel(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName)) return fieldName;
            return System.Text.RegularExpressions.Regex.Replace(
                fieldName, "(?<=[a-z])([A-Z])", " $1").Trim();
        }

        [WebMethod(EnableSession = true)]
        public static string GetAllAvailableColumns()
        {
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