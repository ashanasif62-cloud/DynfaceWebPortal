using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.APEmployeeAppraisalPlanSvcReference;
using System.Data;

namespace DynamicsPortal.ESS.HR
{
    public partial class ESSEmployeeAppraisalPlan_List  : MainForm
    {
        private APEmployeeAppraisalPlan APEmployeeAppraisalPlan = new APEmployeeAppraisalPlan();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();
        protected override void Page_Load(object sender, EventArgs e)
        {

            try
            {
                tableId = APEmployeeAppraisalPlan.tableName;
                pageMenuId = "ESSAPEmployeeAppraisalsPlan";

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
            DataTable dt = APEmployeeAppraisalPlan.retriveEmployeeAppraisalsPlan();
           // DataTable dt = APEmployeeAppraisalPlan.retriveAll();

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
                    DataTable dataTable = APEmployeeAppraisalPlan.createDataTable();
                    DataRow dr = dataTable.NewRow();
                   
                   
                    dr["KPICode"] = ((TextBox)gridViewRow.FindControl("txtKPICode")).Text;
                    dr["Description"] = ((TextBox)gridViewRow.FindControl("txtDescription")).Text;
                    dr["KPIWeightage"] = ((TextBox)gridViewRow.FindControl("txtKPIWeightage")).Text;
                    dr["EmployeeId"] = ((Label)gridViewRow.FindControl("lblEmployeeId")).Text;
                    dr["AppraisalCode"] = ((Label)gridViewRow.FindControl("lblAppraisalCode")).Text;
                    dr["PlanWeightage"] = ((Label)gridViewRow.FindControl("lblPlanWeightage")).Text;
                    dr["ReviewType"] = ((Label)gridViewRow.FindControl("lblReviewType")).Text;
                   

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblrecid")).Text, out recId);


                    long kpiRecId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblKPIRecId")).Text, out kpiRecId);

                    dr["recId"] = recId;
                    dr["KPIRecId"] = kpiRecId;
                    dataTable.Rows.Add(dr);

                    SysOperationResult_BOL operationResult_BOL = APEmployeeAppraisalPlan.update(dataTable, kpiRecId);
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
            List<long> _KPIRecId = new List<long>();
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblKPIRecId")).Text, out recId);
                    if (recId > 0)
                    {
                        _KPIRecId.Add(recId);
                    }

                    SysOperationResult_BOL operationResult_BOL = APEmployeeAppraisalPlan.delete(recId);
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

            if (_KPIRecId.Count > 0) // will update after add list parameter in delete record of api.
            {
                //SysOperationResult_BOL operationResult_BOL = APEmployeeAppraisalPlan.delete(_KPIRecId);
                //bool result = operationResults(operationResult_BOL);
                //if (result)
                //{
                //    gridView.EditIndex = -1;
                //    reBindGrid();
                //}
                //else
                //{
                //    bindGrid();
                //}
            }
        }
        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;


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

        

        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

    }
}