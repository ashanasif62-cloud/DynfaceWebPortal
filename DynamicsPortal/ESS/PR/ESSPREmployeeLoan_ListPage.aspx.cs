using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.PREmployeeLoanRequestSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSPREmployeeLoan_ListPage : MainForm
    {
        private PREmployeeLoan pREmployeeLoan = new PREmployeeLoan();
        private ESSWorkflow eSSWorkflow = new ESSWorkflow();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = pREmployeeLoan.tableName;
                pageMenuId = "ESSPREmployeeLoanHistory";

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
            DataTable dt = pREmployeeLoan.retriveEmployeeReportees();
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
                    DataTable dataTable = pREmployeeLoan.createDataTable();
                    DataRow dr = dataTable.NewRow();

                    //DropDownList cddlGuarantor1 = gridViewRow.FindControl("cddlGuarantor1") as DropDownList;
                    //string guarantor1 = (cddlGuarantor1.FindControl("txtEmployeeId") as TextBox).Text;

                    //DropDownList cddlGuarantor2 = gridViewRow.FindControl("cddlGuarantor2") as DropDownList;
                    //string guarantor2 = (cddlGuarantor2.FindControl("txtEmployeeId") as TextBox).Text;

                    //dr["Guarantor1"] = (gridViewRow.FindControl("ddlGuarantor1") as DropDownList).SelectedValue;      //guarantor1;
                    //dr["Guarantor2"] = (gridViewRow.FindControl("ddlGuarantor2") as DropDownList).SelectedValue;      //guarantor2;
                    dr["LoanTypeCode"] = (gridViewRow.FindControl("ddlLoanTypeCode") as DropDownList).SelectedValue;

                    dr["LoanAmount"] = (gridViewRow.FindControl("txtLoanAmount") as TextBox).Text;
                    dr["RequestDate"] = (gridViewRow.FindControl("txtRequestDate") as TextBox).Text;
                    dr["LoanDescription"] = (gridViewRow.FindControl("txtLoanDescription") as TextBox).Text;
                    dr["RecoveryStartDate"] = (gridViewRow.FindControl("txtRecoveryStartDate") as TextBox).Text;
                    dr["RequestedPaymentDate"] = (gridViewRow.FindControl("txtRequestedPaymentDate") as TextBox).Text;
                    dr["RequestedInstallments"] = (gridViewRow.FindControl("txtRequestedInstallments") as TextBox).Text;
                    dr["RequestedInstallmentAmount"] = (gridViewRow.FindControl("txtRequestedInstallmentAmount") as TextBox).Text;

                    dataTable.Rows.Add(dr);

                    long recId = 0;
                    Int64.TryParse(((Label)gridViewRow.FindControl("lblRecId")).Text, out recId);

                    SysOperationResult_BOL operationResult_BOL = pREmployeeLoan.update(dataTable, recId);
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
                SysOperationResult_BOL operationResult_BOL = pREmployeeLoan.delete(recordsId.ToArray());
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

                DropDownList ddlLoanTypeCode = gridViewRow.FindControl("ddlLoanTypeCode") as DropDownList;
                DataTable dtLoanTypes = ControlsHelper.retrieveAllPRLoanTypes();
                ddlLoanTypeCode.DataSource = dtLoanTypes;
                ddlLoanTypeCode.DataTextField = "AdvanceTypeCode";
                ddlLoanTypeCode.DataValueField = "AdvanceTypeCode";
                ddlLoanTypeCode.DataBind();

                //DropDownList ddlGuarantor1 = gridViewRow.FindControl("ddlGuarantor1") as DropDownList;
                //DataTable dtAllEmployees = ControlsHelper.retrieveAllEmployee();
                //ddlGuarantor1.DataSource = dtAllEmployees;
                //ddlGuarantor1.DataTextField = "EmployeeId";
                //ddlGuarantor1.DataValueField = "EmployeeId";
                //ddlGuarantor1.DataBind();

                //DropDownList ddlGuarantor2 = gridViewRow.FindControl("ddlGuarantor2") as DropDownList;
                //ddlGuarantor2.DataSource = dtAllEmployees;
                //ddlGuarantor2.DataTextField = "EmployeeId";
                //ddlGuarantor2.DataValueField = "EmployeeId";
                //ddlGuarantor2.DataBind();


                ddlLoanTypeCode.SelectedValue = dataRowView["LoanTypeCode"].ToString();
                //ddlGuarantor1.SelectedValue = dataRowView["Guarantor1"].ToString();
                //ddlGuarantor2.SelectedValue = dataRowView["Guarantor2"].ToString();
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
                    string requestId = (gridViewRow.FindControl("lblLoanRequestId") as Label).Text;
                    if (!string.IsNullOrEmpty(requestId))
                    {
                        recordsId.Add(requestId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = eSSWorkflow.pREmployeeLoanRequests_Submit(recordsId.ToArray());
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