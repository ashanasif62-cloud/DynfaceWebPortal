using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeLeavesSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeLeave_ListPage : MainForm
    {
        private PREmployeeLeaveRequest pREmployeeLeaveRequest = new PREmployeeLeaveRequest();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = pREmployeeLeaveRequest.tableName;
                pageMenuId = "ESSPREmployeeLeaveHistory";

                base.Page_Load(sender, e);

                if (!isUserAuthenticated)
                    return;

                if (!IsPostBack)
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
            DataTable dt = pREmployeeLeaveRequest.retriveEmployeeReportees();
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
                    DataTable dataTable = pREmployeeLeaveRequest.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    dr["LeaveCategory"] = ((DropDownList)gridViewRow.FindControl("ddlLeaveCategory")).SelectedValue;
                    dr["LeaveCode"] = ((DropDownList)gridViewRow.FindControl("ddlLeaveCode")).SelectedValue;

                    dr["LeaveDays"] = ((TextBox)gridViewRow.FindControl("txtLeaveDays")).Text;
                    dr["LeaveReqDate"] = ((TextBox)gridViewRow.FindControl("txtLeaveReqDate")).Text;
                    dr["LeaveStartDate"] = ((TextBox)gridViewRow.FindControl("txtLeaveStartDate")).Text;
                    dr["Reason"] = ((TextBox)gridViewRow.FindControl("txtReason")).Text;

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);
                    dataTable.Rows.Add(dr);

                    SysOperationResult_BOL operationResult_BOL = pREmployeeLeaveRequest.update(dataTable, recId);
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
                SysOperationResult_BOL operationResult_BOL = pREmployeeLeaveRequest.delete(recordsId.ToArray());
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

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                DropDownList ddlLeaveCategory = gridViewRow.FindControl("ddlLeaveCategory") as DropDownList;
                ddlLeaveCategory.DataSource = Enum.GetNames(typeof(PRLeaveCategory));
                ddlLeaveCategory.DataBind();

                DropDownList ddlLeaveCode = gridViewRow.FindControl("ddlLeaveCode") as DropDownList;
                ddlLeaveCode.DataSource = ControlsHelper.retrieveAllPRLeaveCodes();
                ddlLeaveCode.DataTextField = "LeaveCode";
                ddlLeaveCode.DataValueField = "LeaveCode";
                ddlLeaveCode.DataBind();

                ddlLeaveCategory.SelectedValue = dataRowView["LeaveCategory"].ToString().Replace(" ", "");
                ddlLeaveCode.SelectedValue = dataRowView["LeaveCode"].ToString();
                //ddlLeaveCode.Items.FindByValue(dataRowView["LeaveCode"].ToString()).Selected = true;
            }

            string[] validStatus = new string[] { PRWFStatus.NotSubmitted.ToString() };
            controlsHelper.checkWFStatus(gridViewRow, "lblWFStatus", validStatus);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string leaveRequestId = (gridViewRow.FindControl("lblLeaveReqId") as Label).Text;
                    if (!string.IsNullOrEmpty(leaveRequestId))
                    {
                        recordsId.Add(leaveRequestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.pREmployeeLeaveRequests_Submit(recordsId.ToArray());
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

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

    }
}