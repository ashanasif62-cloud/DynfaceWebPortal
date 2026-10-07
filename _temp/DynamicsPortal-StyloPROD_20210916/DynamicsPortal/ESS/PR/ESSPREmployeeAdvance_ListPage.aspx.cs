using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeAdvanceRequestSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeAdvance_ListPage : MainForm
    {
        private PREmployeeAdvances pREmployeeAdvances = new PREmployeeAdvances();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = pREmployeeAdvances.tableName;
                pageMenuId = "ESSPREmployeeAdvanceHistory";

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
            DataTable dt = pREmployeeAdvances.retriveEmployeeReportees();
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
                    DataTable dataTable = pREmployeeAdvances.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    dr["AdvanceDescription"] = (gridViewRow.FindControl("txtAdvanceDescription") as TextBox).Text;
                    dr["AdvanceAmount"] = (gridViewRow.FindControl("txtAdvanceAmount") as TextBox).Text;
                    dr["RequestDate"] = (gridViewRow.FindControl("txtRequestDate") as TextBox).Text;
                    dr["AdvanceTypeCode"] = (gridViewRow.FindControl("ddlAdvanceTypeCode") as DropDownList).SelectedValue;

                    string guarantor1 = (gridViewRow.FindControl("txtGuarantor1") as TextBox).Text;
                    string guarantor2 = (gridViewRow.FindControl("txtGuarantor1") as TextBox).Text;
                    dr["Guarantor1"] = guarantor1;
                    dr["Guarantor2"] = guarantor2;

                    dr["RequestedPaymentDate"] = (gridViewRow.FindControl("txtRequestedPaymentDate") as TextBox).Text;
                    dr["RecoveryStartDate"] = (gridViewRow.FindControl("txtRecoveryStartDate") as TextBox).Text;
                    dr["RequestedInstallments"] = (gridViewRow.FindControl("txtRequestedInstallments") as TextBox).Text;
                    dr["RequestedInstallmentAmount"] = (gridViewRow.FindControl("txtRequestedInstallmentAmount") as TextBox).Text;

                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                    SysOperationResult_BOL operationResult_BOL = pREmployeeAdvances.update(dataTable, recId);
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
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = pREmployeeAdvances.delete(recordsId.ToArray());
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

                DropDownList ddlAdvanceTypeCode = gridViewRow.FindControl("ddlAdvanceTypeCode") as DropDownList;
                DataTable dtAdvanceTypes = ControlsHelper.retrieveAllPRAdvanceTypes();
                ddlAdvanceTypeCode.DataSource = dtAdvanceTypes;
                ddlAdvanceTypeCode.DataTextField = "AdvanceTypeCode";
                ddlAdvanceTypeCode.DataValueField = "AdvanceTypeCode";
                ddlAdvanceTypeCode.DataBind();

                ddlAdvanceTypeCode.SelectedValue = dataRowView["AdvanceTypeCode"].ToString();
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
                    string requestId = (gridViewRow.FindControl("lblAdvanceRequestId") as Label).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.pREmployeeAdvanceRequests_Submit(recordsId.ToArray());
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