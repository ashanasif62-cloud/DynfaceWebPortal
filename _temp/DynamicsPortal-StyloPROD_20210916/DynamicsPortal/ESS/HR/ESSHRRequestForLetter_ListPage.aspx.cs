using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using PortalIntegration.ESSEmploymentCertificateSvcReference;

namespace DynamicsPortal
{
    public partial class ESSHRRequestForLetter_ListPage : MainForm
    {
        private HRRequestForLetters hRRequestForLetters = new HRRequestForLetters();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hRRequestForLetters.tableName;
                pageMenuId = "ESSHREmployeeLettersHistory";

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
            DataTable dt = hRRequestForLetters.retriveEmployeeReportees();
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
                    DataTable dataTable = hRRequestForLetters.createDataTable();
                    DataRow dr = dataTable.NewRow();
                    dr["Authentication"] = (gridViewRow.FindControl("txtAuthentication") as TextBox).Text;
                    dr["Remarks"] = (gridViewRow.FindControl("txtRemarks") as TextBox).Text;

                    dr["ReqestedFor"] = (gridViewRow.FindControl("ddlRequestedFor") as DropDownList).SelectedValue;
                    dr["Reason"] = (gridViewRow.FindControl("ddlReason") as DropDownList).SelectedValue;

                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                    SysOperationResult_BOL operationResult_BOL = hRRequestForLetters.update(dataTable, recId);
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
                SysOperationResult_BOL operationResult_BOL = hRRequestForLetters.delete(recordsId.ToArray());
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

                DropDownList ddlRequestedFor = gridViewRow.FindControl("ddlRequestedFor") as DropDownList;
                DataTable dtRequestedFor = ControlsHelper.retrieveAllESSRequestedFor();
                ddlRequestedFor.DataSource = dtRequestedFor;
                ddlRequestedFor.DataTextField = "RequestedForId";
                ddlRequestedFor.DataValueField = "RequestedForId";
                ddlRequestedFor.DataBind();

                DropDownList ddlReasonCode = gridViewRow.FindControl("ddlReason") as DropDownList;
                DataTable dtReasonCode = ControlsHelper.retrieveAllHcmReasonCode();
                ddlReasonCode.DataSource = dtReasonCode;
                ddlReasonCode.DataTextField = "Description";
                ddlReasonCode.DataValueField = "ReasonCodeId";
                ddlReasonCode.DataBind();

                ddlReasonCode.SelectedValue = dataRowView["Reason"].ToString();
                ddlRequestedFor.SelectedValue = dataRowView["ReqestedFor"].ToString();
            }

            string[] validStatus = new string[] { ESSWorkFlowStatus.NotSubmitted.ToString() };
            controlsHelper.checkWFStatus(gridViewRow, "lblWorkflowState", validStatus);
        }
        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> recordsId = new List<string>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lblCertificateId") as Label).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.hREmploymentCertificateRequest_Submit(recordsId.ToArray());
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