using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeEOSRequestsSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeEOS_ListPage : MainForm
    {
        private PREmployeeEOS pREmployeeEOS = new PREmployeeEOS();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = pREmployeeEOS.tableName;
                pageMenuId = "ESSPREmployeeEOSHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
                {
                    reBindGrid();
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
            finally
            { }
        }

        private void getGridDataTable()
        {
            DataTable dt = pREmployeeEOS.retriveEmployeeReportees();
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
                    DataTable dataTable = pREmployeeEOS.createDataTable();
                    DataRow dr = dataTable.NewRow();

                    dr["EOSType"] = (gridViewRow.FindControl("ddlEOSType") as DropDownList).SelectedValue;
                    dr["EOSReasonCode"] = ((DropDownList)gridViewRow.FindControl("ddlEOSReasonCode")).SelectedValue;

                    dr["LastWorkingDate_Actual"] = ((TextBox)gridViewRow.FindControl("txtLastWorkingDate_Actual")).Text;
                    dr["EOSNotificationDate"] = ((TextBox)gridViewRow.FindControl("txtEOSNotificationDate")).Text;
                    dr["Remarks"] = ((TextBox)gridViewRow.FindControl("txtRemarks")).Text;
                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                    SysOperationResult_BOL operationResult_BOL = pREmployeeEOS.update(dataTable, recId);
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
            List<long> recordsId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = pREmployeeEOS.delete(recordsId.ToArray());
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
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            FormatDateLabel(e.Row, "lblEOSNotificationDate");
            FormatDateLabel(e.Row, "lblLastWorkingDate_Actual");
            FormatDateLabel(e.Row, "lblLastWorkingDate_Calculated");
            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                DropDownList ddlEOSType = e.Row.FindControl("ddlEOSType") as DropDownList;
                ddlEOSType.DataSource = Enum.GetNames(typeof(PREOSType));
                ddlEOSType.DataBind();

                DropDownList ddlReasonCode = e.Row.FindControl("ddlEOSReasonCode") as DropDownList;
                DataTable reasonCode = ControlsHelper.retrieveAllHcmReasonCode();
                ddlReasonCode.DataSource = reasonCode;
                ddlReasonCode.DataTextField = "Description";
                ddlReasonCode.DataValueField = "ReasonCodeId";
                ddlReasonCode.DataBind();

                ddlEOSType.SelectedValue = dataRowView["EOSType"].ToString();
                ddlReasonCode.SelectedValue = dataRowView["EOSReasonCode"].ToString();

                //SetDateTextBox(e.Row, "txtEOSNotificationDate", dataRowView["EOSNotificationDate"]);
                //SetDateTextBox(e.Row, "txtLastWorkingDate_Actual", dataRowView["LastWorkingDate_Actual"]);

                TextBox txtEOSNotificationDate = e.Row.FindControl("txtEOSNotificationDate") as TextBox;
                if (txtEOSNotificationDate != null)
                {
                    Label lblEOSNotificationDate = e.Row.FindControl("lblEOSNotificationDate") as Label;
                    if (lblEOSNotificationDate != null)
                        txtEOSNotificationDate.Text = lblEOSNotificationDate.Text;

                    Label lblLastWorkingDate_Calculated = e.Row.FindControl("lblLastWorkingDate_Calculated") as Label;
                    if (lblLastWorkingDate_Calculated != null)
                        txtEOSNotificationDate.Text = lblLastWorkingDate_Calculated.Text;
                    txtEOSNotificationDate.Enabled = false;
                }
            }

            string[] validStatus = new string[] { PRWFStatus.NotSubmitted.ToString() };
            controlsHelper.checkWFStatus(gridViewRow, "lblWFStatus", validStatus);

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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string leaveRequestId = (gridViewRow.FindControl("lblEOSRequestId") as Label).Text;
                    if (!string.IsNullOrEmpty(leaveRequestId))
                    {
                        recordsId.Add(leaveRequestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.pREmployeeEOSRequests_Submit(recordsId.ToArray());
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

        private void FormatDateLabel(GridViewRow row, string labelId)
        {
            Label lbl = row.FindControl(labelId) as Label;
            if (lbl != null && !string.IsNullOrWhiteSpace(lbl.Text))
            {
                DateTime dt;
                if (DateTime.TryParse(lbl.Text, out dt))
                {
                    lbl.Text = dt.ToString("dd/M/yyyy");   // Month/Date/Year  → 9/17/2026
                                                          // Use "MM/dd/yyyy" if you prefer leading zeros → 09/17/2026
                }
            }
        }


        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
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
            PREmployeeEOS pREmployeeEOS = new PREmployeeEOS();
            DataTable dt = pREmployeeEOS.retriveEmployeeReportees();

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