using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.TASOvertimePlannerSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.TA
{
    public partial class ESSOvertimePlanner_ListPage : MainForm
    {
        // -------------------------------------------------------
        // PAGE LOAD
        // -------------------------------------------------------
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pageMenuId = "ESSOvertimePlanner_ListPage";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!isPageAuthorizated)
                    return;

                if (!IsPostBack)
                {
                    Page.Title = "Overtime Planner";

                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.InnerText = "Overtime Planner";
                        titleDiv.Style["font-weight"] = "bold";
                    }

                    reBindGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
            }
        }

        // -------------------------------------------------------
        // REBIND GRID
        // -------------------------------------------------------
        protected void reBindGrid()
        {
            DataTable dt = getGridDataTable();
            bindGrid(dt);
        }

        protected void bindGrid(DataTable dt)
        {
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        // -------------------------------------------------------
        // GET DATA
        // -------------------------------------------------------
        private DataTable getGridDataTable()
        {
            try
            {
                TASOvertimePlannersSvc svc = new TASOvertimePlannersSvc();
                string employeeId = SessionVariables.getCurrentEmployeeId();

                DataTable dt = svc.retrieveAll(employeeId);

                SessionVariables.setSessionDataTable(dt);

                return dt;
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
                return new DataTable();
            }
        }

        // -------------------------------------------------------
        // FORMAT TIME — used in ItemTemplate (view mode)
        // Converts seconds-since-midnight → "08:47 AM" display
        // -------------------------------------------------------
        //protected string FormatTime(object timeValue)
        //{
        //    if (timeValue == null || timeValue == DBNull.Value)
        //        return string.Empty;

        //    DateTime dt;

        //    if (timeValue is DateTime t1)
        //    {
        //        dt = t1;
        //    }
        //    else
        //    {
        //        var s = timeValue.ToString().Trim();

        //        // Seconds-since-midnight integer?
        //        if (int.TryParse(s, out int totalSeconds))
        //        {
        //            dt = DateTime.Today.AddSeconds(totalSeconds);
        //        }
        //        else
        //        {
        //            // Try known string formats
        //            string[] formats = { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm", "hh:mm tt", "hh:mm:ss tt" };
        //            if (!DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        //            {
        //                if (!DateTime.TryParse(s, out dt))
        //                    return s; // give up, return as-is
        //            }
        //        }
        //    }

        //    // Always display in 12-hour format: "08:47 AM"
        //    return dt.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        //}


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

                // Seconds-since-midnight integer?
                if (int.TryParse(s, out int totalSeconds))
                {
                    dt = DateTime.Today.AddSeconds(totalSeconds);
                }
                else
                {
                    // Try known string formats
                    string[] formats = {
                        "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm",
                        "hh:mm:ss tt", "h:mm:ss tt", "hh:mm tt", "h:mm tt"
                    };
                    if (!DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        if (!DateTime.TryParse(s, out dt))
                            return s; // give up, return as-is
                    }
                }
            }

            // Display in 12-hour format: "08:47:30 AM"
            return dt.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
        }


        // -------------------------------------------------------
        // CONVERT SECONDS TO TIME — used in EditItemTemplate
        // Returns "08:47 AM" so user sees 12-hour in the textbox
        // -------------------------------------------------------
        //protected string ConvertSecondsToTime(object value)
        //{
        //    if (value == null || value == DBNull.Value)
        //        return "";

        //    if (!int.TryParse(value.ToString(), out int seconds))
        //        return "";

        //    DateTime dt = DateTime.Today.AddSeconds(seconds);

        //    // Returns "08:47 AM" or "03:46 PM"
        //    return dt.ToString("hh:mm tt", CultureInfo.InvariantCulture);
        //}


        // -------------------------------------------------------
        protected string ConvertSecondsToTime(object value)
        {
            if (value == null || value == DBNull.Value)
                return "";

            if (!int.TryParse(value.ToString(), out int seconds))
                return "";

            DateTime dt = DateTime.Today.AddSeconds(seconds);

            // Returns "08:47:30 AM" in 12-hour format
            return dt.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture);
        }

        // -------------------------------------------------------
        // TRY PARSE TIME TO SECONDS
        // Handles: "08:47 AM", "3:46 PM", "08:47", "8:47", "08:47:00"
        // Returns total seconds since midnight as int
        // -------------------------------------------------------
        //private bool TryParseTimeToSeconds(string input, out int totalSeconds)
        //{
        //    totalSeconds = 0;

        //    if (string.IsNullOrWhiteSpace(input))
        //        return false;

        //    input = input.Trim();

        //    // ── Step 1: Try 12-hour format first (what the textbox shows) ──
        //    // Handles: "08:47 AM", "8:47 AM", "03:46 PM", "3:46 PM"
        //    string[] twelveHourFormats = {
        //        "hh:mm tt",
        //        "h:mm tt",
        //        "hh:mm:ss tt",
        //        "h:mm:ss tt"
        //    };

        //    if (DateTime.TryParseExact(input, twelveHourFormats,
        //            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDt))
        //    {
        //        totalSeconds = (int)parsedDt.TimeOfDay.TotalSeconds;
        //        return true;
        //    }

        //    // ── Step 2: Try 24-hour format fallback ──
        //    // Handles: "08:47", "8:47", "08:47:00"
        //    string cleaned = input.ToUpper()
        //                          .Replace(" AM", "")
        //                          .Replace(" PM", "");

        //    if (TimeSpan.TryParseExact(cleaned,
        //            new[] { @"hh\:mm", @"h\:mm", @"hh\:mm\:ss", @"h\:mm\:ss" },
        //            CultureInfo.InvariantCulture, out TimeSpan ts))
        //    {
        //        totalSeconds = (int)ts.TotalSeconds;
        //        return true;
        //    }

        //    // ── Step 3: General fallback ──
        //    if (TimeSpan.TryParse(cleaned, out ts))
        //    {
        //        totalSeconds = (int)ts.TotalSeconds;
        //        return true;
        //    }

        //    return false;
        //}



        private bool TryParseTimeToSeconds(string input, out int totalSeconds)
        {
            totalSeconds = 0;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            // ── Step 1: Try 12-hour format with seconds ──
            string[] twelveHourFormats = {
                "hh:mm:ss tt",
                "h:mm:ss tt",
                "hh:mm tt",
                "h:mm tt"
            };

            if (DateTime.TryParseExact(input, twelveHourFormats,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDt))
            {
                totalSeconds = (int)parsedDt.TimeOfDay.TotalSeconds;
                return true;
            }

            // ── Step 2: Try 24-hour format with seconds ──
            string[] twentyFourHourFormats = {
                "HH:mm:ss",
                "H:mm:ss",
                "HH:mm",
                "H:mm"
            };

            if (DateTime.TryParseExact(input, twentyFourHourFormats,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDt))
            {
                totalSeconds = (int)parsedDt.TimeOfDay.TotalSeconds;
                return true;
            }

            // ── Step 3: Try TimeSpan parse ──
            if (TimeSpan.TryParse(input, out TimeSpan ts))
            {
                totalSeconds = (int)ts.TotalSeconds;
                return true;
            }

            return false;
        }



        // -------------------------------------------------------
        // DELETE
        // -------------------------------------------------------
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                List<long> recIds = new List<long>();

                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;

                    if (chk != null && chk.Checked)
                    {
                        long recId = Convert.ToInt64(gridView.DataKeys[row.RowIndex].Value);
                        recIds.Add(recId);
                    }
                }

                if (recIds.Count > 0)
                {
                    TASOvertimePlannersSvc svc = new TASOvertimePlannersSvc();
                    var result = svc.delete(recIds.ToArray());

                    NotificationMessage.showMessage(result);
                    upGrid.Update();
                    reBindGrid();
                }
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
                NotificationMessage.showMessage("Error occurred while deleting record.");
            }
        }

        // -------------------------------------------------------
        // ROW EDITING
        // -------------------------------------------------------
        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            reBindGrid();
        }

        // -------------------------------------------------------
        // ROW CANCELING EDIT
        // -------------------------------------------------------
        protected void gridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gridView.EditIndex = -1;
            reBindGrid();
        }

        // -------------------------------------------------------
        // ROW UPDATING
        // -------------------------------------------------------
        protected void gridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                GridViewRow row = gridView.Rows[e.RowIndex];
                long recId = Convert.ToInt64(gridView.DataKeys[e.RowIndex].Value);

                // ── Find controls ──
                TextBox txtPlanDate = row.FindControl("txtPlanDate") as TextBox;
                TextBox txtStartTime = row.FindControl("txtStartTime") as TextBox;
                TextBox txtEndTime = row.FindControl("txtEndTime") as TextBox;

                // FIX: These labels now exist in EditItemTemplate so FindControl works
                Label lblEmployeeId = row.FindControl("lblEmployeeId") as Label;
                Label lblEmployeeName = row.FindControl("lblEmployeeName") as Label;
                Label lblRequestDate = row.FindControl("lblRequestDate") as Label;

                // ── Null checks ──
                if (txtPlanDate == null || txtStartTime == null || txtEndTime == null
                    || lblEmployeeId == null || lblEmployeeName == null || lblRequestDate == null)
                {
                    NotificationMessage.showMessage("Error: Could not read form fields. Please try again.");
                    return;
                }

                // ── Parse start time → seconds ──
                // Accepts "08:47 AM", "3:46 PM", "08:47", "8:47"
                if (!TryParseTimeToSeconds(txtStartTime.Text, out int startSeconds))
                {
                    NotificationMessage.showMessage("Invalid start time. Use format: 08:30 AM or 08:30");
                    return;
                }

                // ── Parse end time → seconds ──
                if (!TryParseTimeToSeconds(txtEndTime.Text, out int endSeconds))
                {
                    NotificationMessage.showMessage("Invalid end time. Use format: 05:00 PM or 17:00");
                    return;
                }

                // ── Validation ──
                if (endSeconds <= startSeconds)
                {
                    NotificationMessage.showMessage("End time must be greater than start time.");
                    return;
                }

                // ── Build DataTable ──
                DataTable dt = new DataTable();
                dt.Columns.Add("employeeId", typeof(string));
                dt.Columns.Add("employeeName", typeof(string));
                dt.Columns.Add("planDate", typeof(DateTime));
                dt.Columns.Add("startTime", typeof(int));
                dt.Columns.Add("endTime", typeof(int));
                dt.Columns.Add("requestDate", typeof(DateTime));
                dt.Columns.Add("recId", typeof(long));

                DataRow dr = dt.NewRow();
                dr["employeeId"] = lblEmployeeId.Text;
                dr["employeeName"] = lblEmployeeName.Text;
                dr["planDate"] = Convert.ToDateTime(txtPlanDate.Text);
                dr["requestDate"] = Convert.ToDateTime(lblRequestDate.Text);
                dr["startTime"] = startSeconds;   // ✅ int seconds e.g. 30600
                dr["endTime"] = endSeconds;     // ✅ int seconds e.g. 61200
                dr["recId"] = recId;
                dt.Rows.Add(dr);

                // ── Call service ──
                TASOvertimePlannersSvc svc = new TASOvertimePlannersSvc();
                var result = svc.UpdateGeneral(dt);

                NotificationMessage.showMessage(result);

                // ── Exit edit mode ──
                gridView.EditIndex = -1;
                reBindGrid();
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
                NotificationMessage.showMessage("Error occurred while updating record.");
            }
        }


        // -------------------------------------------------------
        // EDIT BUTTON – Store selected row in Session and open form
        // -------------------------------------------------------
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow selectedRow = null;
                int selectedCount = 0;

                foreach (GridViewRow row in gridView.Rows)
                {
                    CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                    if (chk != null && chk.Checked)
                    {
                        selectedRow = row;
                        selectedCount++;
                    }
                }

                if (selectedCount == 0)
                {
                    NotificationMessage.showMessage("Please select a record to edit.");
                    return;
                }

                if (selectedCount > 1)
                {
                    NotificationMessage.showMessage("Please select only one record to edit.");
                    return;
                }

                // ========== GET RECID SAFELY ==========
                long recId = 0;

                // Method 1: from DataKeys
                if (gridView.DataKeys[selectedRow.RowIndex] != null &&
                    gridView.DataKeys[selectedRow.RowIndex].Value != null &&
                    gridView.DataKeys[selectedRow.RowIndex].Value != DBNull.Value)
                {
                    long.TryParse(gridView.DataKeys[selectedRow.RowIndex].Value.ToString(), out recId);
                }

                // Method 2: fallback from the hidden label (more reliable)
                if (recId <= 0)
                {
                    Label lblRecId = selectedRow.FindControl("lblRecId") as Label;
                    if (lblRecId != null && !string.IsNullOrWhiteSpace(lblRecId.Text))
                    {
                        long.TryParse(lblRecId.Text, out recId);
                    }
                }

                if (recId <= 0)
                {
                    NotificationMessage.showMessage("Could not find RecId of the selected record. Please check DataKeyNames / column name.");
                    return;
                }

                // ========== GET OTHER VALUES ==========
                Label lblPlanId = selectedRow.FindControl("lblPlanId") as Label;
                Label lblEmployeeId = selectedRow.FindControl("lblEmployeeId") as Label;
                Label lblEmployeeName = selectedRow.FindControl("lblEmployeeName") as Label;
                Label lblRequestDate = selectedRow.FindControl("lblRequestDate") as Label;
                Label lblPlanDate = selectedRow.FindControl("lblPlanDate") as Label;
                Label lblStartTime = selectedRow.FindControl("lblStartTime") as Label;
                Label lblEndTime = selectedRow.FindControl("lblEndTime") as Label;
                Label lblWFStatus = selectedRow.FindControl("lblWFStatus") as Label;

                // Store in Session
                Session["OT_Edit_RecId"] = recId;
                Session["OT_Edit_PlanId"] = lblPlanId?.Text ?? "";
                Session["OT_Edit_EmployeeId"] = lblEmployeeId?.Text ?? "";
                Session["OT_Edit_EmployeeName"] = lblEmployeeName?.Text ?? "";
                Session["OT_Edit_RequestDate"] = lblRequestDate?.Text ?? "";
                Session["OT_Edit_PlanDate"] = lblPlanDate?.Text ?? "";
                Session["OT_Edit_StartTime"] = lblStartTime?.Text ?? "";
                Session["OT_Edit_EndTime"] = lblEndTime?.Text ?? "";
                Session["OT_Edit_WFStatus"] = lblWFStatus?.Text ?? "";

                // Open the Edit form
                string script = "openPopupPanel('/ESS/TA/ESSOvertimePlanner_Edit.aspx');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenEditForm", script, true);
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
                NotificationMessage.showMessage("Error occurred while opening edit form: " + ex.Message);
            }
        }

        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            try
            {
                reBindGrid();
                upGrid.Update();          // important for UpdatePanel
            }
            catch (Exception ex)
            {
                SysErrorLog log = new SysErrorLog();
                log.write(GetType().FullName, ex);
            }
        }
        //protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
        //        {
        //            TextBox txtPlanDate = e.Row.FindControl("txtPlanDate") as TextBox;

        //            if (txtPlanDate != null)
        //            {
        //                DataRowView drv = (DataRowView)e.Row.DataItem;

        //                if (drv["planDate"] != DBNull.Value)
        //                {
        //                    DateTime planDate = Convert.ToDateTime(drv["planDate"]);

        //                    txtPlanDate.Text = planDate.ToString("yyyy-MM-dd");
        //                }
        //            }
        //        }
        //    }
        //}


        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            // Format Plan Date as M/d/yyyy
            Label lblPlanDate = e.Row.FindControl("lblPlanDate") as Label;
            Label lblRequestDate = e.Row.FindControl("lblRequestDate") as Label;

            if (lblPlanDate != null && !string.IsNullOrWhiteSpace(lblPlanDate.Text))
            {
                DateTime planDate;

                if (DateTime.TryParse(lblPlanDate.Text, out planDate))
                {
                    lblPlanDate.Text = planDate.ToString("dd/M/yyyy");
                }
            }

            if (lblRequestDate != null && !string.IsNullOrWhiteSpace(lblRequestDate.Text))
            {
                DateTime planDate;

                if (DateTime.TryParse(lblRequestDate.Text, out planDate))
                {
                    lblRequestDate.Text = planDate.ToString("dd/M/yyyy");
                }
            }

            // Keep Plan Date in yyyy-MM-dd format inside the HTML date picker
            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                TextBox txtPlanDate = e.Row.FindControl("txtPlanDate") as TextBox;

                if (txtPlanDate != null)
                {
                    DataRowView drv = e.Row.DataItem as DataRowView;

                    if (drv != null && drv["planDate"] != DBNull.Value)
                    {
                        DateTime planDate = Convert.ToDateTime(drv["planDate"]);

                        // HTML5 date input requires yyyy-MM-dd
                        txtPlanDate.Text = planDate.ToString("yyyy-MM-dd");
                    }
                }
            }
        }


    }
}
