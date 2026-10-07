using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class HcmESSWorkerBankAccountDashboard : ModalForm
    {
        private EssWorkerBankAccount hcmbankaccount = new EssWorkerBankAccount();
        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hcmbankaccount.tableName;
                pageMenuId = "HcmESSWorkerBankAccountDashboard";
                showPageTitle = false;

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


        public bool setViewState_HCMESSWorkerBankAccountDashboard(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_EssWorkerBankAccount"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public DataTable getViewState_HCMESSWorkerBankAccountDashboard()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_EssWorkerBankAccount"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_EssWorkerBankAccount"] as DataTable).Copy();
            }
            else
            {
                getGridDataTable();
                dataTable = getViewState_HCMESSWorkerBankAccountDashboard();
            }
            return dataTable;
        }

        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = hcmbankaccount.findbyEmployee(employeeId);
            setViewState_HCMESSWorkerBankAccountDashboard(dt);
        }

        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = getViewState_HCMESSWorkerBankAccountDashboard();
            gridView.DataSource = dt;
            gridView.DataBind();
        }

        protected void gridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gridView.EditIndex = e.NewEditIndex;
            bindGrid();
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ControlsHelper controlsHelper = new ControlsHelper();
            GridViewRow gridViewRow = e.Row;
            if (gridViewRow.RowType != DataControlRowType.DataRow)
                return;

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
            }


            //string wfStatus = (gridViewRow.FindControl("lblApprovalStatus") as Label).Text.Replace(" ", "");

            string[] validStatus = new string[] { "NotSubmitted", "Draft", "" };
            controlsHelper.checkWFStatus(gridViewRow, "lblApprovalStatus", validStatus);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable = getViewState_HCMESSWorkerBankAccountDashboard();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.InsertAt(dr, 0);

                setViewState_HCMESSWorkerBankAccountDashboard(dataTable);
            }

            gridView.EditIndex = 0;
            bindGrid();
        }


        protected void Update_Click(object sender, EventArgs e)
        {
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    bool result;
                    SysOperationResult_BOL operationResult_BOL;
                    DataTable dataTable = hcmbankaccount.createDataTable();

                    DataRow dr = dataTable.NewRow();
                    string employeeId = SessionVariables.getCurrentEmployeeId();
                    long workerRecId = ControlsHelper.getWorkerId(employeeId);
                    dr["accountId"] = (gridViewRow.FindControl("txtAccountId") as TextBox).Text;
                    dr["name"] = (gridViewRow.FindControl("txtName") as TextBox).Text;
                    dr["accountNum"] = (gridViewRow.FindControl("txtAccountNum") as TextBox).Text;
                    dr["bankIBAN"] = (gridViewRow.FindControl("txtBankIBAN") as TextBox).Text;
                    dr["branchName"] = (gridViewRow.FindControl("txtBranchName") as TextBox).Text;
                    dr["branchNumber"] = (gridViewRow.FindControl("txtBranchNumber") as TextBox).Text;
                    dr["accountHolder"] = (gridViewRow.FindControl("txtAccountHolder") as TextBox).Text;
                    dr["Worker"] = workerRecId;

                    dataTable.Rows.Add(dr);

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    if (recId == 0 && rowIndex == 0)
                    {
                        operationResult_BOL = hcmbankaccount.create(dataTable);
                        result = operationResults(operationResult_BOL);
                    }
                    else
                    {
                        operationResult_BOL = hcmbankaccount.update(dataTable, Convert.ToInt64(recId));
                        result = operationResults(operationResult_BOL);
                    }


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
            GridViewRow gridViewRow = (sender as LinkButton).NamingContainer as GridViewRow;
            if (gridViewRow.RowType == DataControlRowType.DataRow)
            {
                if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
                {
                    int rowIndex = gridViewRow.RowIndex;
                    if (rowIndex == 0 && string.IsNullOrEmpty(gridView.DataKeys[rowIndex].Values[0].ToString()))
                    {
                        DataTable dataTable = new DataTable();
                        dataTable = getViewState_HCMESSWorkerBankAccountDashboard();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                setViewState_HCMESSWorkerBankAccountDashboard(dataTable);
                            }

                        }
                    }
                }
            }
            gridView.EditIndex = -1;
            bindGrid();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();
            bool includeEmptyRows = false;
            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow.Checked)
                {
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);
                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                    else
                    {
                        includeEmptyRows = true;
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = hcmbankaccount.delete(recordsId.ToArray());
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
                return;
            }
            else if (includeEmptyRows)
            {
                gridView.EditIndex = -1;
                reBindGrid();
            }

        }

        protected override void btnAttachment_Click(object sender, EventArgs e)
        {
            base.btnAttachment_Click(sender, e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<long> recordsId = new List<long>();

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;

                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    Label lbl = gridViewRow.FindControl("lblRecId") as Label;

                    if (lbl != null)
                    {
                        long recId;
                        if (long.TryParse(lbl.Text, out recId) && recId > 0)
                        {
                            recordsId.Add(recId);
                        }
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                long singleRecordId = recordsId[0]; // get first selected ID

                // 🔥 Pass SINGLE ID instead of LIST
                SysOperationResult_BOL operationResult_BOL = submitWFRequest(singleRecordId);

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

        private SysOperationResult_BOL submitWFRequest(long _recId)
        {
            long requestRecId = _recId;
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();
            ESSWorkflow eSSWorkflow = new ESSWorkflow();

            if (requestRecId > 0)
            {
                submitResult = eSSWorkflow.essWorkerBankAccount_SubmitByRecId_Submit(requestRecId);

                if (submitResult.isSuccess)
                    submitResult.Message = "Request successfully submitted.";
                else
                    submitResult.Message = "Failed to submit the request.";
            }
            else
            {
                submitResult.AlertType = AlertType.Error.ToString();
                submitResult.isSuccess = false;
                submitResult.Message = "Failed to submit the request.";
            }

            return submitResult;
        }
    }
}