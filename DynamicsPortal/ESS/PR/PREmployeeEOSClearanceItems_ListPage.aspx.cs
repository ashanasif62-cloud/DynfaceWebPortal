using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DynamicsPortal.ESS.PR
{
    public partial class PREmployeeEOSClearanceItems_ListPage : MainForm
    {
        private PREmployeeEOSClearanceItem clearanceservice = new PREmployeeEOSClearanceItem();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = clearanceservice.tableName;
                pageMenuId = "PREmployeeEOSClearanceItems_ListPage";
                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    var titleDiv = Master.FindControl("pageTitle") as System.Web.UI.HtmlControls.HtmlGenericControl;
                    if (titleDiv != null)
                    {
                        titleDiv.Style["font-family"] = "'Segoe UI', SegoeUI, Arial, sans-serif";
                        titleDiv.Style["font-size"] = "22px";          // ← bigger title
                        titleDiv.Style["font-weight"] = "600";
                        titleDiv.Style["color"] = "#323130";
                        titleDiv.Style["letter-spacing"] = "0.2px";
                        titleDiv.Style["margin"] = "8px 0 16px 0";
                        titleDiv.Style["cursor"] = "default";
                        titleDiv.Style["user-select"] = "none";
                        titleDiv.Style["-webkit-user-select"] = "none";
                        titleDiv.Style["outline"] = "none";
                        titleDiv.Attributes["tabindex"] = "-1";        // prevents focus
                    }

                    reBindGrid();
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write(GetType().FullName, ex);
            }
        }

        // ========== UPPER GRID ==========
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var lblActual = e.Row.FindControl("lblLastWorkingActual") as Label;
            var lblCalculated = e.Row.FindControl("lblLastWorkingCalculated") as Label;

            object actualObj = DataBinder.Eval(e.Row.DataItem, "LastWorkingDateActual");
            object calculatedObj = DataBinder.Eval(e.Row.DataItem, "LastWorkingDateCalculated");

            if (lblActual != null && actualObj != null && actualObj != DBNull.Value)
            {
                DateTime dt = Convert.ToDateTime(actualObj);
                // Force format: 1/08/2026
                lblActual.Text = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            }

            if (lblCalculated != null && calculatedObj != null && calculatedObj != DBNull.Value)
            {
                DateTime dt = Convert.ToDateTime(calculatedObj);
                lblCalculated.Text = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);
            }
        }

        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = clearanceservice.retrieveAll(employeeId);

            DataTable distinctDt = new DataTable();
            if (dt != null && dt.Rows.Count > 0)
            {
                distinctDt = dt.Clone();
                HashSet<string> uniqueRequestIds = new HashSet<string>();

                foreach (DataRow row in dt.Rows)
                {
                    string requestId = row["RequestId"]?.ToString() ?? "";
                    if (!uniqueRequestIds.Contains(requestId))
                    {
                        uniqueRequestIds.Add(requestId);
                        distinctDt.ImportRow(row);
                    }
                }
            }

            SessionVariables.setSessionDataTable(distinctDt);
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

        // ========== LOWER GRID ==========
        protected void gridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            DropDownList ddlStatus = e.Row.FindControl("ddlStatus") as DropDownList;
            if (ddlStatus == null) return;

            ddlStatus.Items.Clear();
            ddlStatus.Items.Add(new ListItem("No", "No"));
            ddlStatus.Items.Add(new ListItem("Yes", "Yes"));

            string status = DataBinder.Eval(e.Row.DataItem, "Status")?.ToString();
            if (!string.IsNullOrEmpty(status) && ddlStatus.Items.FindByValue(status) != null)
            {
                ddlStatus.SelectedValue = status;
            }
        }

        // ========== SELECTION (FAST) ==========
        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox clicked = sender as CheckBox;
            if (clicked == null) return;

            GridViewRow selectedRow = null;

            // Uncheck all other checkboxes
            foreach (GridViewRow row in gridView.Rows)
            {
                CheckBox chk = row.FindControl("chk_SelectSingle") as CheckBox;
                if (chk != null && chk != clicked)
                {
                    chk.Checked = false;
                }
                else if (chk != null && chk == clicked && chk.Checked)
                {
                    selectedRow = row;
                }
            }

            if (selectedRow != null)
            {
                string employeeId = SessionVariables.getCurrentEmployeeId();
                long recId = Convert.ToInt64(gridView.DataKeys[selectedRow.RowIndex].Value);

                // Load only the lower grid data
                DataTable dt = clearanceservice.retrieveItemWise(employeeId, recId);
                gridView1.DataSource = dt;
                gridView1.DataBind();
            }
            else
            {
                gridView1.DataSource = null;
                gridView1.DataBind();
            }

            // Update ONLY the lower panel → much faster, less overlay
            upGrid1.Update();
        }

        // ========== SAVE ==========
        protected void BtnSave_Header_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow row in gridView1.Rows)
                {
                    CheckBox chk = row.FindControl("chk_SelectSingle1") as CheckBox;
                    if (chk == null || !chk.Checked) continue;

                    long recId = Convert.ToInt64(gridView1.DataKeys[row.RowIndex].Value);
                    DropDownList ddlStatus = row.FindControl("ddlStatus") as DropDownList;
                    TextBox remarks = row.FindControl("lblRemarks") as TextBox;

                    string status = ddlStatus?.SelectedValue ?? "";
                    string newRemarks = remarks?.Text ?? "";

                    SysOperationResult_BOL result = clearanceservice.UpdateRecord(recId, status, newRemarks);
                    NotificationMessage.showMessage(result);

                    if (result.isSuccess)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "refreshPage",
                            "setTimeout(function(){ location.reload(); }, 2000);", true);
                    }
                }
            }
            catch (Exception ex)
            {
                new SysErrorLog().write("BtnSave_Header_Click", ex);
            }
        }
    }
}