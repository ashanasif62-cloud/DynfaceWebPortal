using BussinessObject;
using GeneralAuxiliary;
using PortalIntegration;
using PortalIntegration.HcmPersonEducationSvcReference;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace DynamicsPortal
{
    public partial class ESSHcmPersonEducation : ModalForm
    {
        private HcmPersonEducation hcmPersonEducation = new HcmPersonEducation();
        private HcmEducationDiscipline hcmEducationDiscipline = new HcmEducationDiscipline();

        protected override void Page_Load(object sender, EventArgs e)
        {
            try
            {
                tableId = hcmPersonEducation.tableName;
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

        public bool setViewState_hcmPersonEducation(DataTable _dataTable)
        {
            bool isStored = false;
            DataTable dataTable = new DataTable();

            if (ViewState != null)
            {
                dataTable = _dataTable.Copy();
                ViewState["dataTable_hcmPersonEducation"] = dataTable;
                isStored = true;
            }
            return isStored;
        }

        public DataTable getViewState_hcmPersonEducation()
        {
            DataTable dataTable = null;

            if (ViewState["dataTable_hcmPersonEducation"] != null)
            {
                dataTable = new DataTable();
                dataTable = (ViewState["dataTable_hcmPersonEducation"] as DataTable).Copy();
            }
            else
            {
                getGridDataTable();
                dataTable = getViewState_hcmPersonEducation();
            }
            return dataTable;
        }

        private void getGridDataTable()
        {
            string employeeId = SessionVariables.getCurrentEmployeeId();
            DataTable dt = hcmPersonEducation.findByEmployee(employeeId);
            setViewState_hcmPersonEducation(dt);
        }
        protected void reBindGrid()
        {
            getGridDataTable();
            bindGrid();
        }
        protected void bindGrid()
        {
            DataTable dt = getViewState_hcmPersonEducation();
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

                DropDownList ddlEducationDiscipline = gridViewRow.FindControl("ddlEducationDiscipline") as DropDownList;
                ddlEducationDiscipline.DataSource = hcmEducationDiscipline.retrieveAllEducationDiscipline();
                ddlEducationDiscipline.DataTextField = "EducationDisciplineId";
                ddlEducationDiscipline.DataValueField = "RecId";
                ddlEducationDiscipline.DataBind();
                ddlEducationDiscipline.SelectedValue = dataRowView["EducationDiscipline"].ToString(); ;

                DropDownList ddlDurationUnit = gridViewRow.FindControl("ddlDurationUnit") as DropDownList;
                ddlDurationUnit.DataSource = Enum.GetNames(typeof(PeriodUnitPI));
                ddlDurationUnit.DataBind();
                ddlDurationUnit.SelectedValue = dataRowView["DurationUnit"].ToString();

                // Format date fields in edit mode
                TextBox txtStartDate = gridViewRow.FindControl("txtStartDate") as TextBox;
                if (txtStartDate != null && !string.IsNullOrEmpty(txtStartDate.Text) && DateTime.TryParse(txtStartDate.Text, out DateTime startDate))
                {
                    txtStartDate.Text = startDate.ToString("yyyy-MM-dd");
                }

                TextBox txtEndDate = gridViewRow.FindControl("txtEndDate") as TextBox;
                if (txtEndDate != null && !string.IsNullOrEmpty(txtEndDate.Text) && DateTime.TryParse(txtEndDate.Text, out DateTime endDate))
                {
                    txtEndDate.Text = endDate.ToString("yyyy-MM-dd");
                }
            }

            Label lblStartDate = gridViewRow.FindControl("lblStartDate") as Label;
            if (lblStartDate != null && !string.IsNullOrEmpty(lblStartDate.Text) && DateTime.TryParse(lblStartDate.Text, out DateTime gvStartDate))
            {
                lblStartDate.Text = gvStartDate.ToString("MM/dd/yyyy");
            }

            Label lblEndDate = gridViewRow.FindControl("lblEndDate") as Label;
            if (lblEndDate != null && !string.IsNullOrEmpty(lblEndDate.Text) && DateTime.TryParse(lblEndDate.Text, out DateTime gvEndDate))
            {
                lblEndDate.Text = gvEndDate.ToString("MM/dd/yyyy");
            }

            string[] validStatus = new string[] { "NotSubmitted", "Draft", "" };
            controlsHelper.checkWFStatus(gridViewRow, "lblApprovalStatus", validStatus);
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable = getViewState_hcmPersonEducation();

            if (dataTable != null)
            {
                DataRow dr = dataTable.NewRow();
                dataTable.Rows.InsertAt(dr, 0);

                setViewState_hcmPersonEducation(dataTable);
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
                    DataTable dataTable = hcmPersonEducation.createDataTable();

                    DataRow dr = dataTable.NewRow();

                    string employeeId = SessionVariables.getCurrentEmployeeId();
                    dr["EmployeeId"] = employeeId;

                    dr["Description"] = (gridViewRow.FindControl("txtDescription") as TextBox).Text;
                    dr["Notes"] = (gridViewRow.FindControl("txtNotes") as TextBox).Text;
                    dr["StartDate"] = (gridViewRow.FindControl("txtStartDate") as TextBox).Text;
                    dr["EndDate"] = (gridViewRow.FindControl("txtEndDate") as TextBox).Text;
                    dr["Duration"] = (gridViewRow.FindControl("txtDuration") as TextBox).Text;

                    dr["DurationUnit"] = (gridViewRow.FindControl("ddlDurationUnit") as DropDownList).SelectedValue;
                    dr["EducationDiscipline"] = (gridViewRow.FindControl("ddlEducationDiscipline") as DropDownList).SelectedValue;
                    //dr["EducationDisciplineId"] = (gridViewRow.FindControl("txtEducationDisciplineId") as TextBox).Text;

                    //dr["WorkflowOperation"] = (gridViewRow.FindControl("lblWorkflowOperation") as Label).Text;
                    dr["ApprovalStatus"] = (gridViewRow.FindControl("lblApprovalStatus") as Label).Text;


                    dataTable.Rows.Add(dr);

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);   //gridView.DataKeys[rowIndex].Values[0]



                    if (recId == 0 && rowIndex == 0)
                    {

                        operationResult_BOL = hcmPersonEducation.create(dataTable);
                        result = operationResults(operationResult_BOL);
                    }
                    else
                    {
                        operationResult_BOL = hcmPersonEducation.update(dataTable, Convert.ToInt64(recId));
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
                        dataTable = getViewState_hcmPersonEducation();

                        if (dataTable != null)
                        {
                            DataRow dr = dataTable.Rows[rowIndex];
                            if (string.IsNullOrEmpty(dr["RecId"].ToString()))   //dr.RowState == DataRowState.Added || 
                            {
                                dataTable.Rows[rowIndex].Delete();
                                dataTable.AcceptChanges();

                                setViewState_hcmPersonEducation(dataTable);
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
            List<GeneralContract> recordDetails = new List<GeneralContract>();
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
                        string approvalStatus = (gridViewRow.FindControl("lblApprovalStatus") as Label).Text;

                        GeneralContract generalContract = new GeneralContract();
                        generalContract.RecId = recId;
                        generalContract.Message = approvalStatus;

                        recordDetails.Add(generalContract);
                        //Array.Resize(ref recordsId, recordsId.Length + 1);
                        //recordsId[recordsId.Length - 1] = recId;
                    }
                    else
                    {
                        includeEmptyRows = true;
                    }
                }
            }

            if (recordDetails.Count > 0)
            {
                //SysOperationResult_BOL operationResult_BOL = hcmPersonEducation.delete(recordsId.ToArray());
                SysOperationResult_BOL operationResult_BOL = hcmPersonEducation.deleteRecords(recordDetails.ToArray());
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
                submitResult = eSSWorkflow.eSSPersonEducation_Submit(requestRecId);
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

        protected void chk_SelectSingle_CheckedChanged(object sender, EventArgs e)
        {
            bool canDelete = false;

            foreach (GridViewRow gridViewRow in gridView.Rows)
            {
                CheckBox chkSelectRow = gridViewRow.FindControl("chk_SelectSingle") as CheckBox;
                if (chkSelectRow != null && chkSelectRow.Checked)
                {
                    Label lblApprovalStatus = gridViewRow.FindControl("lblApprovalStatus") as Label;
                    if (lblApprovalStatus != null && lblApprovalStatus.Text.Trim() == "Draft")
                    {
                        canDelete = true;
                    }
                    else
                    {
                        canDelete = false;
                        break;
                    }
                }
            }

            btnDelete.Enabled = canDelete;
        }

    }
}