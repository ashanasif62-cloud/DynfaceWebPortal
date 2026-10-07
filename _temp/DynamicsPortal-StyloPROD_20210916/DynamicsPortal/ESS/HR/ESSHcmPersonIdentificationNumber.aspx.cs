using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HcmPersonIdentificationNumberSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHcmPersonIdentificationNumber : ModalForm
    {
        private HcmPersonIdentificationNumber hcmPersonIdentificationNumber = new HcmPersonIdentificationNumber();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hcmPersonIdentificationNumber.tableName;
                pageMenuId = "ESSHRPersonalDetailsHistory";
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

        public bool setViewState_HcmPersonIdentificationNumber(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_HcmPersonIdentificationNumber"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public DataTable getViewState_HcmPersonIdentificationNumber()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_HcmPersonIdentificationNumber"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_HcmPersonIdentificationNumber"] as DataTable).Copy();
            }
            else
            {
                getGridDataTable();
                dataTable = getViewState_HcmPersonIdentificationNumber();
            }
            return dataTable;
        }

        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = hcmPersonIdentificationNumber.retrieveByEmployee(employeeId);
            setViewState_HcmPersonIdentificationNumber(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = getViewState_HcmPersonIdentificationNumber();
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
                DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                DropDownList ddlIssuingAgency = gridViewRow.FindControl("ddlIssuingAgency") as DropDownList;
                ddlIssuingAgency.DataSource = ControlsHelper.retrieveAllHcmIssuingAgency();
                ddlIssuingAgency.DataTextField = "IssuingAgencyId";
                ddlIssuingAgency.DataValueField = "RecId";
                ddlIssuingAgency.DataBind();
                string issuingAgency = dataRowView["IssuingAgency"].ToString();
                if (string.IsNullOrEmpty(issuingAgency))
                {
                    ddlIssuingAgency.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                    ddlIssuingAgency.SelectedIndex = 0;
                }
                else
                {
                    ddlIssuingAgency.SelectedValue = issuingAgency;
                }

                DropDownList ddlIdentificationType = gridViewRow.FindControl("ddlIdentificationType") as DropDownList;
                ddlIdentificationType.DataSource = ControlsHelper.retrieveAllHcmIdentificationType();
                ddlIdentificationType.DataTextField = "IdentificationTypeId";
                ddlIdentificationType.DataValueField = "RecId";
                ddlIdentificationType.DataBind();

                DropDownList ddlIsPrimary = gridViewRow.FindControl("ddlIsPrimary") as DropDownList;
                ddlIsPrimary.DataSource = Enum.GetNames(typeof(NoYes));
                ddlIsPrimary.DataBind();

                ddlIdentificationType.SelectedValue = dataRowView["IdentificationType"].ToString();
                ddlIsPrimary.SelectedValue = dataRowView["IsPrimary"].ToString();
            }
                //string wfStatus = (gridViewRow.FindControl("lblApprovalStatus") as Label).Text.Replace(" ", "");

                string[] validStatus = new string[] { "NotSubmitted", "Draft", "" };
                controlsHelper.checkWFStatus(gridViewRow, "lblApprovalStatus", validStatus);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable = getViewState_HcmPersonIdentificationNumber();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.InsertAt(dr, 0);

                setViewState_HcmPersonIdentificationNumber(dataTable);
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
                    DataTable dataTable = hcmPersonIdentificationNumber.createDataTable();

                    DataRow dr = dataTable.NewRow();

                    string employeeId = SessionVariables.getCurrentEmployeeId();
                    long empId = ControlsHelper.getWorkerId(employeeId);

                    dr["EmployeeId"] = empId;                              //C
                    dr["Classification"] = (gridViewRow.FindControl("txtClassification") as TextBox).Text;
                    dr["Description"] = (gridViewRow.FindControl("txtDescription") as TextBox).Text;
                    dr["IssuedDate"] = (gridViewRow.FindControl("txtIssuedDate") as TextBox).Text;
                    dr["ExpirationDate"] = (gridViewRow.FindControl("txtExpirationDate") as TextBox).Text;
                    dr["IdentificationNumber"] = (gridViewRow.FindControl("txtIdentificationNumber") as TextBox).Text;

                    dr["IsPrimary"] = (gridViewRow.FindControl("ddlIsPrimary") as DropDownList).SelectedValue;
                    dr["IssuingAgency"] = (gridViewRow.FindControl("ddlIssuingAgency") as DropDownList).SelectedValue;
                    dr["IdentificationType"] = (gridViewRow.FindControl("ddlIdentificationType") as DropDownList).SelectedValue;

                    dr["WorkflowOperation"] = (gridViewRow.FindControl("lblWorkflowOperation") as Label).Text;
                    dr["ApprovalStatus"] = (gridViewRow.FindControl("lblApprovalStatus") as Label).Text;

                    dataTable.Rows.Add(dr);

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    if (recId == 0 && rowIndex == 0)
                    {
                        operationResult_BOL = hcmPersonIdentificationNumber.create(dataTable);
                        result = operationResults(operationResult_BOL);
                    }
                    else
                    {
                        operationResult_BOL = hcmPersonIdentificationNumber.update(dataTable, Convert.ToInt64(recId));
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
                        dataTable = getViewState_HcmPersonIdentificationNumber();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                setViewState_HcmPersonIdentificationNumber(dataTable);
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
                SysOperationResult_BOL operationResult_BOL = hcmPersonIdentificationNumber.delete(recordsId.ToArray());
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
                if (chkSelectRow.Checked)
                {
                    string requestId = (gridViewRow.FindControl("lblRecId") as Label).Text;
                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse(requestId, out recId);   //gridView.DataKeys[rowIndex].Values[0]

                    if (recId > 0)
                    {
                        recordsId.Add(recId);
                    }
                }
            }

            if (recordsId.Count > 0)
            {
                SysOperationResult_BOL operationResult_BOL = submitWFRequest(recordsId.ToArray());
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
        
        private SysOperationResult_BOL submitWFRequest(long[] _recId)
        {
            long[] requestRecId = _recId;
            SysOperationResult_BOL submitResult = new SysOperationResult_BOL();
            ESSWorkflow eSSWorkflow = new ESSWorkflow();

            if (requestRecId.Length > 0)
            {
                submitResult = eSSWorkflow.eSSPersonIdentificaitonNumber_Submit(requestRecId);
                if (submitResult.isSuccess)
                {
                    submitResult.Message = " Request successfully submitted.";
                }
                else
                {
                    submitResult.Message = " Failed to submit the request.";
                }
            }
            else
            {
                submitResult.AlertType = AlertType.Error.ToString();
                submitResult.isSuccess = false;
                submitResult.Message = " Failed to submit the request.";
            }

            return submitResult;
        }


    }
}