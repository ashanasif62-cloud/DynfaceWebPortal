using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.ESSHRHelpDeskRequestSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHRHelpDeskRequest_ListPage : MainForm
    {
        private ESSHRHelpDeskRequest eSSHRHelpDeskRequest = new ESSHRHelpDeskRequest();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = eSSHRHelpDeskRequest.tableName;
                pageMenuId = "ESSHRHelpDeskRequestHistory";

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
            DataTable dt = eSSHRHelpDeskRequest.retriveEmployeeReportees();
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
                    //eSSHRHelpDeskRequestSvcContract.Detail;               //- CU
                    //eSSHRHelpDeskRequestSvcContract.ESSDate;              //- CU
                    //eSSHRHelpDeskRequestSvcContract.ESSTypeOfProblem;     //- CU
                    DataTable dataTable = eSSHRHelpDeskRequest.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    dr["Detail"] = (gridViewRow.FindControl("txtDetail") as TextBox).Text;
                    dr["ESSDate"] = (gridViewRow.FindControl("txtESSDate") as TextBox).Text;
                    dr["ESSTypeOfProblem"] = (gridViewRow.FindControl("ddlESSTypeOfProblem") as DropDownList).SelectedValue;

                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                    SysOperationResult_BOL operationResult_BOL = eSSHRHelpDeskRequest.update(dataTable, recId);
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
                SysOperationResult_BOL operationResult_BOL = eSSHRHelpDeskRequest.delete(recordsId.ToArray());
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

                DropDownList ddlESSTypeOfProblem = e.Row.FindControl("ddlESSTypeOfProblem") as DropDownList;
                ddlESSTypeOfProblem.DataSource = Enum.GetNames(typeof(ESSTypeOfProblem));
                ddlESSTypeOfProblem.DataBind();

                ddlESSTypeOfProblem.SelectedValue = dataRowView["ESSTypeOfProblem"].ToString();
            }


            string[] validStatus = new string[] { ESSWorkFlowStatus.NotSubmitted.ToString() };
            controlsHelper.checkWFStatus(gridViewRow, "lblESSWorkFlowStatus", validStatus);
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lblESSHRHelpDeskRequestId") as Label).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.hRHelpDeskRequest_Submit(recordsId.ToArray());
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