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

            DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

            // ── Format date labels (read mode) ───────────────────────────────────
            FormatDateLabel(gridViewRow.FindControl("lblIssuedDate") as Label, dataRowView["IssuedDate"].ToString());
            FormatDateLabel(gridViewRow.FindControl("lblExpirationDate") as Label, dataRowView["ExpirationDate"].ToString());
            // ─────────────────────────────────────────────────────────────────────

            if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
            {
                //DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

                //DropDownList ddlIssuingAgency = gridViewRow.FindControl("ddlIssuingAgency") as DropDownList;
                //ddlIssuingAgency.DataSource = ControlsHelper.retrieveAllHcmIssuingAgency();
                //ddlIssuingAgency.DataTextField = "IssuingAgencyId";
                //ddlIssuingAgency.DataValueField = "RecId";
                //ddlIssuingAgency.DataBind();
                //string issuingAgency = dataRowView["IssuingAgency"].ToString();
                //if (string.IsNullOrEmpty(issuingAgency))
                //{
                //    ddlIssuingAgency.Items.Insert(0, new ListItem(String.Empty, String.Empty));
                //    ddlIssuingAgency.SelectedIndex = 0;
                //}
                //else
                //{
                //    ddlIssuingAgency.SelectedValue = issuingAgency;
                //}
                Label recId = gridViewRow.FindControl("lblRecId") as Label;
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

                TextBox txtIssuedDate = gridViewRow.FindControl("txtIssuedDate") as TextBox;
                if (txtIssuedDate != null)
                {
                    DateTime issuedDate;
                    if (DateTime.TryParse(dataRowView["IssuedDate"].ToString(), out issuedDate) && issuedDate > DateTime.MinValue)
                        txtIssuedDate.Text = issuedDate.ToString("yyyy-MM-dd"); // HTML date input requires yyyy-MM-dd
                    else
                        txtIssuedDate.Text = "";
                }

                TextBox txtExpirationDate = gridViewRow.FindControl("txtExpirationDate") as TextBox;
                if (txtExpirationDate != null)
                {
                    DateTime expirationDate;
                    if (DateTime.TryParse(dataRowView["ExpirationDate"].ToString(), out expirationDate) && expirationDate > DateTime.MinValue)
                        txtExpirationDate.Text = expirationDate.ToString("yyyy-MM-dd"); // HTML date input requires yyyy-MM-dd
                    else
                        txtExpirationDate.Text = "";
                }

                if(recId != null && !string.IsNullOrEmpty(recId.Text))
                {
                    ddlIdentificationType.Enabled = false;
                    TextBox identificationNumber = gridViewRow.FindControl("txtIdentificationNumber") as TextBox;
                    if (identificationNumber != null)
                        identificationNumber.Enabled = false;
                }
            }
            //string wfStatus = (gridViewRow.FindControl("lblApprovalStatus") as Label).Text.Replace(" ", "");

            string[] validStatus = new string[] { "NotSubmitted", "Draft", "" };
            controlsHelper.checkWFStatus(gridViewRow, "lblApprovalStatus", validStatus);

        }

        private void FormatDateLabel(Label lbl, string rawValue)
        {
            if (lbl == null) return;

            DateTime dt;
            lbl.Text = (DateTime.TryParse(rawValue, out dt) && dt.Year > 1900)
                ? dt.ToString("M/d/yyyy")
                : string.Empty;
        }

        //protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    ControlsHelper controlsHelper = new ControlsHelper();
        //    GridViewRow gridViewRow = e.Row;
        //    if (gridViewRow.RowType != DataControlRowType.DataRow)
        //        return;

        //    // ── Format date labels to MM/dd/yyyy ──
        //    DataRowView dataRowView = gridViewRow.DataItem as DataRowView;

        //    Label lblIssuedDate = gridViewRow.FindControl("lblIssuedDate") as Label;
        //    if (lblIssuedDate != null)
        //    {
        //        DateTime dt;
        //        if (DateTime.TryParse(dataRowView["IssuedDate"].ToString(), out dt) && dt > DateTime.MinValue)
        //            lblIssuedDate.Text = dt.ToString("MM/dd/yyyy");
        //        else
        //            lblIssuedDate.Text = "";
        //    }

        //    Label lblExpirationDate = gridViewRow.FindControl("lblExpirationDate") as Label;
        //    if (lblExpirationDate != null)
        //    {
        //        DateTime dt;
        //        if (DateTime.TryParse(dataRowView["ExpirationDate"].ToString(), out dt) && dt > DateTime.MinValue)
        //            lblExpirationDate.Text = dt.ToString("MM/dd/yyyy");
        //        else
        //            lblExpirationDate.Text = "";
        //    }

        //    // ── existing edit block ──
        //    if ((gridViewRow.RowState & DataControlRowState.Edit) > 0)
        //    {
        //        // ... your existing edit code stays here unchanged ...
        //    }

        //    string[] validStatus = new string[] { "NotSubmitted", "Draft", "" };
        //    controlsHelper.checkWFStatus(gridViewRow, "lblApprovalStatus", validStatus);
        //}

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

                    dr["EmployeeId"] = empId;
                    dr["Classification"] = (gridViewRow.FindControl("txtClassification") as TextBox).Text;
                    dr["Description"] = (gridViewRow.FindControl("txtDescription") as TextBox).Text;
                    dr["IssuedDate"] = (gridViewRow.FindControl("txtIssuedDate") as TextBox).Text;
                    dr["ExpirationDate"] = (gridViewRow.FindControl("txtExpirationDate") as TextBox).Text;
                    dr["IdentificationNumber"] = (gridViewRow.FindControl("txtIdentificationNumber") as TextBox).Text;
                    dr["IsPrimary"] = (gridViewRow.FindControl("ddlIsPrimary") as DropDownList).SelectedValue;
                    dr["IdentificationType"] = (gridViewRow.FindControl("ddlIdentificationType") as DropDownList).SelectedValue;
                    dr["WorkflowOperation"] = (gridViewRow.FindControl("lblWorkflowOperation") as Label).Text;
                    dr["ApprovalStatus"] = (gridViewRow.FindControl("lblApprovalStatus") as Label).Text;

                    int rowIndex = gridViewRow.RowIndex;
                    long recId = 0;
                    Int64.TryParse((gridViewRow.FindControl("lblRecId") as Label).Text, out recId);

                    Label lblRecVersion = gridViewRow.FindControl("lblRecVersion") as Label;
                    dr["PersonIdentificationNumberRecVersion"] = lblRecVersion?.Text;

                    // ── Date Validation ───────────────────────────────────────────────
                    DateTime minAllowableDate = new DateTime(1900, 1, 1);
                    DateTime maxAllowableDate = new DateTime(2154, 12, 31);

                    string issuedDateText = (gridViewRow.FindControl("txtIssuedDate") as TextBox).Text;
                    string expirationDateText = (gridViewRow.FindControl("txtExpirationDate") as TextBox).Text;


                    if (!string.IsNullOrWhiteSpace(issuedDateText))
                    {
                        if (DateTime.TryParse(issuedDateText, out DateTime issuedDate))
                        {
                            if (issuedDate < minAllowableDate)
                            {
                                SysOperationResult_BOL resultValidate = new SysOperationResult_BOL();
                                resultValidate.isSuccess = false;
                                resultValidate.Message = $"'{issuedDate.ToString("MM/dd/yyyy")}' is before the minimum allowable date 01/01/1900";
                                resultValidate.AlertType = AlertType.Error.ToString();
                                operationResults(resultValidate);
                                return;
                            }
                            if (issuedDate > maxAllowableDate)
                            {
                                SysOperationResult_BOL resultValidate = new SysOperationResult_BOL();
                                resultValidate.isSuccess = false;
                                resultValidate.Message = $"'{issuedDate.ToString("MM/dd/yyyy")}' exceeds the maximum allowable date 12/31/2154";
                                resultValidate.AlertType = AlertType.Error.ToString();
                                operationResults(resultValidate);
                                return;
                            }
                        }
                        else
                        {
                            SysOperationResult_BOL resultValidate = new SysOperationResult_BOL();
                            resultValidate.isSuccess = false;
                            resultValidate.Message = $"'{issuedDateText}' is not a valid date.";
                            resultValidate.AlertType = AlertType.Error.ToString();
                            operationResults(resultValidate);
                            return;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(expirationDateText))
                    {
                        if (DateTime.TryParse(expirationDateText, out DateTime expirationDate))
                        {
                            if (expirationDate < minAllowableDate)
                            {
                                SysOperationResult_BOL resultValidate = new SysOperationResult_BOL();
                                resultValidate.isSuccess = false;
                                resultValidate.Message = $"'{expirationDate.ToString("MM/dd/yyyy")}' is before the minimum allowable date 01/01/1900";
                                resultValidate.AlertType = AlertType.Error.ToString();
                                operationResults(resultValidate);
                                return;
                            }
                            if (expirationDate > maxAllowableDate)
                            {
                                SysOperationResult_BOL resultValidate = new SysOperationResult_BOL();
                                resultValidate.isSuccess = false;
                                resultValidate.Message = $"'{expirationDate.ToString("MM/dd/yyyy")}' exceeds the maximum allowable date 12/31/2154";
                                resultValidate.AlertType = AlertType.Error.ToString();
                                operationResults(resultValidate);
                                return;
                            }
                        }
                        else
                        {
                            SysOperationResult_BOL resultValidate = new SysOperationResult_BOL();
                            resultValidate.isSuccess = false;
                            resultValidate.Message = $"'{expirationDateText}' is not a valid date.";
                            resultValidate.AlertType = AlertType.Error.ToString();
                            operationResults(resultValidate);
                            return;
                        }
                    }
                    // ─────────────────────────────────────────────────────────────────

                    // ── Duplicate type check ──────────────────────────────────────────
                    //string selectedType = dr["IdentificationType"].ToString();
                    DataTable dtViewState = getViewState_HcmPersonIdentificationNumber();

                    DropDownList ddlIdentificationType = gridViewRow.FindControl("ddlIdentificationType") as DropDownList;
                    string selectedType = ddlIdentificationType?.SelectedValue ?? string.Empty;
                    string selectedTypeText = ddlIdentificationType?.SelectedItem?.Text ?? string.Empty;

                    for (int i = 0; i < dtViewState.Rows.Count; i++)
                    {
                        if (i == rowIndex) continue;

                        if (dtViewState.Rows[i]["IdentificationType"].ToString() == selectedType)
                        {
                            SysOperationResult_BOL resultValidate = new SysOperationResult_BOL();
                            resultValidate.isSuccess = false;
                            resultValidate.Message = $"'{selectedTypeText}' record already exists. Only one record per Identification Type is allowed.";
                            resultValidate.AlertType = AlertType.Error.ToString();
                            operationResults(resultValidate);
                            return;
                        }
                    }
                    // ─────────────────────────────────────────────────────────────────

                    dataTable.Rows.Add(dr);

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

            //btnDelete.Enabled = canDelete;
            //btnSubmit.Enabled = canDelete;
        }
    }
}